using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;
using static xMenuClient.CommonFunctions;
using static xMenuShared.PermissionsManager;

namespace xMenuClient
{
    public class About
    {
        // Variables
        private Menu menu;

        private void CreateMenu()
        {
            // Create the menu.
            menu = new Menu("xMenu", "About xMenu");

            // Create menu items.
            MenuItem version = new MenuItem("xMenu Version", $"This server is using xMenu ~b~~h~{MainMenu.Version}~h~~s~.")
            {
                Label = $"~h~{MainMenu.Version}~h~"
            };
            MenuItem credits = new MenuItem("About xMenu / Credits", "xMenu is made by ~b~Ecliptics#1664~s~. For more info, checkout ~b~https://discord.gg/CTUapzMA2N~s~. Thank you to: Vespura for code.https://github.com/TomGrobbe/vMenu");

            string serverInfoMessage = xMenuShared.ConfigManager.GetSettingsString(xMenuShared.ConfigManager.Setting.xmenu_server_info_message);
            if (!string.IsNullOrEmpty(serverInfoMessage))
            {
                MenuItem serverInfo = new MenuItem("Server Info", serverInfoMessage);
                string siteUrl = xMenuShared.ConfigManager.GetSettingsString(xMenuShared.ConfigManager.Setting.vmenu_server_info_website_url);
                if (!string.IsNullOrEmpty(siteUrl))
                {
                    serverInfo.Label = $"{siteUrl}";
                }
                menu.AddMenuItem(serverInfo);
            }
            menu.AddMenuItem(version);
            menu.AddMenuItem(credits);
        }

        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }
    }
}
