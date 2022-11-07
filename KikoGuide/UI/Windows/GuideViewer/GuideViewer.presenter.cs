using System;
using System.Reflection;
using Dalamud.Interface.Internal.Notifications;
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
        public GuideViewerPresenter() => PluginService.Framework.Update += this.OnFrameworkUpdate;

        public void Dispose() => PluginService.Framework.Update -= this.OnFrameworkUpdate;

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
        ///     Gets the current plugin version.
        /// </summary>
        internal static string PluginVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration Configuration => PluginService.Configuration;

        /// <summary>
        ///     The currently selected guide to show in the GuideViewer window.
        /// </summary>
        internal Guide? SelectedGuide;

        /// <summary>
        ///     Last auto-selected guide.
        /// </summary>
        private Guide? lastAutoSelectedGuide;

        /// <summary>
        ///     The current player territory, used to determine if the zone has changed.
        /// </summary>
        private uint currentTerritory;

        /// <summary>
        ///     Detect when the player has changed zones and update the guide viewer accordingly through the game framework update event.
        /// </summary>
        public void OnFrameworkUpdate(object? e)
        {
            var currentTerritory = PluginService.ClientState.TerritoryType;
            if (currentTerritory != this.currentTerritory)
            {
                this.currentTerritory = currentTerritory;
                var playerGuide = GuideManager.GetGuideForCurrentTerritory();
                PluginLog.Debug($"GuideViewerPresenter(OnFrameworkUpdate): Player changed territory to {this.currentTerritory}. (Guide for Territory: {playerGuide?.Name ?? "None"})");

                if (playerGuide != null && playerGuide?.Sections?.Count > 0)
                {
                    this.SelectedGuide = playerGuide;
                    this.lastAutoSelectedGuide = playerGuide;
                    if (PluginService.Configuration.Display.AutoToggleGuideForDuty)
                    {
                        if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow window)
                        {
                            PluginLog.Debug($"GuideViewerPresenter(OnTerritoryChange): Toggling guide viewer to open and displaying guide for {playerGuide.Name}.");
                            window.IsOpen = true;
                        }
                    }
                    else if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideViewer)?.IsOpen == false)
                    {
                        PluginService.PluginInterface.UiBuilder.AddNotification(TGuideViewer.GuideAvailableForDuty, PluginConstants.PluginName, NotificationType.Info);
                    }
                }

                else if (playerGuide == null && this.lastAutoSelectedGuide != null && this.lastAutoSelectedGuide == this.SelectedGuide)
                {
                    this.SelectedGuide = null;
                    if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow window)
                    {
                        PluginLog.Debug($"GuideViewerPresenter(OnTerritoryChange): Toggling guide viewer to closed - no guide data found for territory and last auto-selected guide is the same as the current selected guide.");
                        window.IsOpen = false;
                    }
                }
            }
        }
    }
}
