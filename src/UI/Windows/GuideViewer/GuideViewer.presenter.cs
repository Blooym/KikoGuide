using System;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.Windows.Settings;

namespace KikoGuide.UI.Windows.GuideViewer
{
    public sealed class GuideViewerPresenter : IDisposable
    {
        public GuideViewerPresenter() => PluginService.ClientState.TerritoryChanged += this.OnTerritoryChange;

        public void Dispose() => PluginService.ClientState.TerritoryChanged -= this.OnTerritoryChange;

        /// <summary>
        ///     The currently selected guide to show in the GuideViewer window.
        /// </summary>
        internal Guide? SelectedGuide;

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration Configuration => PluginService.Configuration;

        /// <summary>
        ///     Toggles the settings window.
        /// </summary>
        internal static void ToggleSettingsWindow()
        {
            if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.Settings) is SettingsWindow window)
            {
                window.IsOpen ^= true;
            }
        }

        /// <summary>
        ///     Handles territory change even and changes the UI state accordingly.
        /// </summary>
        public void OnTerritoryChange(object? sender, ushort e)
        {
            var playerGuide = GuideManager.GetGuideForCurrentTerritory();
            PluginLog.Information($"GuideViewerPresenter(OnTerritoryChange): Player changed territory to {e}. (Guide for Territory: {playerGuide?.Name ?? "None"})");

            // If the player has entered a duty with a guide and has the setting enabled, show the guide viewer window.
            if (playerGuide != null && playerGuide?.Sections?.Count > 0)
            {
                this.SelectedGuide = playerGuide;
                if (PluginService.Configuration.Display.AutoToggleGuideForDuty)
                {
                    if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow window)
                    {
                        PluginLog.Information($"GuideViewerPresenter(OnTerritoryChange): Toggling guide viewer to open and displaying guide for {playerGuide.Name}.");
                        window.IsOpen = true;
                    }
                }
            }

            // If the player has entered a territory that does not have any data, deselect the guide & hide the UI
            else if (playerGuide == null)
            {
                this.SelectedGuide = null;
                if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow window)
                {
                    PluginLog.Debug($"GuideViewerPresenter(OnTerritoryChange): Toggling guide viewer to closed - no guide data found for territory {e}.");
                    window.IsOpen = false;
                }
            }
        }
    }
}
