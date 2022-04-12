using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace xMenuShared
{
    public static class ConfigManager
    {
        public enum Setting
        {
            // General settings
            xmenu_use_permissions,
            xmenu_menu_staff_only,
            xmenu_menu_toggle_key,
            xmenu_noclip_toggle_key,
            xmenu_keep_spawned_vehicles_persistent,
            xmenu_use_els_compatibility_mode,
            xmenu_handle_invisibility,
            xmenu_quit_session_in_rockstar_editor,
            xmenu_server_info_message,
            xmenu_server_info_website_url,
            xmenu_teleport_to_wp_keybind_key,
            xmenu_disable_spawning_as_default_character,
            xmenu_enable_animals_spawn_menu,
            xmenu_pvp_mode,
            xmenu_disable_server_info_convars,
            xmenu_player_names_distance,
            xmenu_disable_entity_outlines_tool,
            xmenu_disable_player_stats_setup,

            // Kick & ban settings
            xmenu_default_ban_message_information,
            xmenu_auto_ban_cheaters,
            xmenu_auto_ban_cheaters_ban_message,
            xmenu_log_ban_actions,
            xmenu_log_kick_actions,
            
            // Weather settings
            xmenu_enable_weather_sync,
            xmenu_enable_dynamic_weather,
            xmenu_dynamic_weather_timer,
            xmenu_current_weather,
            xmenu_blackout_enabled,
            xmenu_weather_change_duration,
            xmenu_enable_snow,

            // Time settings
            xmenu_enable_time_sync,
            xmenu_freeze_time,
            xmenu_ingame_minute_duration,
            xmenu_current_hour,
            xmenu_current_minute,
            xmenu_sync_to_machine_time,
        }

        /// <summary>
        /// Get a boolean setting.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static bool GetSettingsBool(Setting setting)
        {
            return GetConvar(setting.ToString(), "false") == "true";
        }

        /// <summary>
        /// Get an integer setting.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static int GetSettingsInt(Setting setting)
        {
            int convarInt = GetConvarInt(setting.ToString(), -1);
            if (convarInt == -1)
            {
                if (int.TryParse(GetConvar(setting.ToString(), "-1"), out int convarIntAlt))
                {
                    return convarIntAlt;
                }
            }
            return convarInt;
        }

        /// <summary>
        /// Get a float setting.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static float GetSettingsFloat(Setting setting)
        {
            if (float.TryParse(GetConvar(setting.ToString(), "-1.0"), out float result))
            {
                return result;
            }
            return -1f;
        }

        /// <summary>
        /// Get a string setting.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static string GetSettingsString(Setting setting, string defaultValue = null)
        {
            var value = GetConvar(setting.ToString(), defaultValue ?? "");
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return value;
        }

        /// <summary>
        /// Debugging mode
        /// </summary>
        public static bool DebugMode => IsDuplicityVersion() ? IsServerDebugModeEnabled() : IsClientDebugModeEnabled();

        /// <summary>
        /// Default value for server debugging mode.
        /// </summary>
        /// <returns></returns>
        public static bool IsServerDebugModeEnabled()
        {
            return GetResourceMetadata("xMenu", "server_debug_mode", 0).ToLower() == "true";
        }

        /// <summary>
        /// Default value for client debugging mode.
        /// </summary>
        /// <returns></returns>
        public static bool IsClientDebugModeEnabled()
        {
            return GetResourceMetadata("xMenu", "client_debug_mode", 0).ToLower() == "true";
        }

        #region Get saved locations from the locations.json
        /// <summary>
        /// Gets the locations.json data.
        /// </summary>
        /// <returns></returns>
        public static Locations GetLocations()
        {
            Locations data = new Locations();

            string jsonFile = LoadResourceFile(GetCurrentResourceName(), "config/locations.json");
            try
            {
                if (string.IsNullOrEmpty(jsonFile))
                {
#if CLIENT
                    xMenuClient.Notify.Error("The locations.json file is empty or does not exist, please tell the server owner to fix this.");
#endif
#if SERVER
                    xMenuServer.DebugLog.Log("The locations.json file is empty or does not exist, please fix this.", xMenuServer.DebugLog.LogLevel.error);
#endif
                }
                else
                {
                    data = JsonConvert.DeserializeObject<Locations>(jsonFile);
                }
            }
            catch (Exception e)
            {
#if CLIENT
                xMenuClient.Notify.Error("An error occurred while processing the locations.json file. Teleport Locations and Location Blips will be unavailable. Please correct any errors in the locations.json file.");
#endif
                Debug.WriteLine($"[xMenu] json exception details: {e.Message}\nStackTrace:\n{e.StackTrace}");
            }

            return data;
        }

        /// <summary>
        /// Gets just the teleport locations data from the locations.json.
        /// </summary>
        /// <returns></returns>
        public static List<TeleportLocation> GetTeleportLocationsData()
        {
            return GetLocations().teleports;
        }

        /// <summary>
        /// Gets just the blips data from the locations.json.
        /// </summary>
        /// <returns></returns>
        public static List<LocationBlip> GetLocationBlipsData()
        {
            return GetLocations().blips;
        }

        /// <summary>
        /// Struct used for deserializing json only.
        /// </summary>
        public struct Locations
        {
            public List<TeleportLocation> teleports;
            public List<LocationBlip> blips;
        }

        /// <summary>
        /// Teleport location struct.
        /// </summary>
        public struct TeleportLocation
        {
            public string name;
            public Vector3 coordinates;
            public float heading;

            public TeleportLocation(string name, Vector3 coordinates, float heading)
            {
                this.name = name;
                this.coordinates = coordinates;
                this.heading = heading;
            }
        }

        /// <summary>
        /// Location blip struct.
        /// </summary>
        public struct LocationBlip
        {
            public string name;
            public Vector3 coordinates;
            public int spriteID;
            public int color;

            public LocationBlip(string name, Vector3 coordinates, int spriteID, int color)
            {
                this.name = name;
                this.coordinates = coordinates;
                this.spriteID = spriteID;
                this.color = color;
            }
        }
        #endregion
    }




}
