using System;
using KikoGuide.Base;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.Windows.Settings;

namespace KikoGuide.UI.Windows.DutyInfo
{
    public sealed class DutyInfoPresenter : IDisposable
    {
        public DutyInfoPresenter()
        {
            PluginService.ClientState.TerritoryChanged += OnTerritoryChange;
        }

        public void Dispose()
        {
            PluginService.ClientState.TerritoryChanged -= OnTerritoryChange;
        }

        /// <summary> 
        ///     The currently selected duty to show in the info window.
        /// </summary>
        public Duty? selectedDuty;

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration Configuration => PluginService.Configuration;

        /// <summary>
        ///     Toggles the settings window.
        /// </summary>
        internal static void ToggleSettingsWindow()
        {
            if (PluginService.WindowManager.windowSystem.GetWindow(WindowManager.SettingsWindowName) is SettingsWindow window)
            {
                window.IsOpen ^= true;
            }
        }

        /// <summary>
        ///     Handles territory change even and changes the UI state accordingly.
        /// </summary>
        public void OnTerritoryChange(object? sender, ushort e)
        {
            Duty? playerDuty = PluginService.DutyManager.GetPlayerDuty();

            // If the player has entered a duty with data and has the setting enabled, show the duty info window.
            if (playerDuty != null && playerDuty?.Sections?.Count > 0)
            {
                selectedDuty = playerDuty;
                if (PluginService.Configuration.Display.AutoToggleGuideForDuty)
                {
                    if (PluginService.WindowManager.windowSystem.GetWindow("Info") is DutyInfoWindow window)
                    {
                        window.IsOpen = true;
                    }
                }
            }

            // If the player has entered a territory that does not have any data, deselect the duty & hide the UI
            else if (playerDuty == null)
            {
                selectedDuty = null;
                if (PluginService.WindowManager.windowSystem.GetWindow("Info") is DutyInfoWindow window)
                {
                    window.IsOpen = false;
                }
            }
        }
    }
}