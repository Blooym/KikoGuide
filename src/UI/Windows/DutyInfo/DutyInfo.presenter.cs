namespace KikoGuide.UI.Windows.DutyInfo
{
    using System;
    using KikoGuide.Base;
    using KikoGuide.Types;
    using KikoGuide.Managers;

    sealed public class DutyInfoPresenter : IDisposable
    {
        public DutyInfoPresenter()
        {
            PluginService.ClientState.TerritoryChanged += this.OnTerritoryChange;
        }

        public void Dispose()
        {
            PluginService.ClientState.TerritoryChanged -= this.OnTerritoryChange;
        }

        /// <summary> 
        ///     The currently selected duty to show in the info window.
        /// </summary>
        public Duty? selectedDuty = null;

        /// <summary>
        ///     Handles territory change even and changes the UI state accordingly.
        /// </summary>
        public void OnTerritoryChange(object? sender, ushort e)
        {
            var playerDuty = DutyManager.GetPlayerDuty();

            // If the player has entered a duty with data and has the setting enabled, show the duty info window.
            if (playerDuty != null && playerDuty?.Sections?.Count > 0)
            {
                this.selectedDuty = playerDuty;
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
                this.selectedDuty = null;
                if (PluginService.WindowManager.windowSystem.GetWindow("Info") is DutyInfoWindow window)
                {
                    window.IsOpen = false;
                }
            }
        }
    }
}