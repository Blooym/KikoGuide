using System;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Internal.Notifications;
using Dalamud.Logging;
using KikoGuide.Attributes;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.Windows.Settings;
using KikoGuide.Utils;

namespace KikoGuide.UI.Windows.GuideViewer
{
    internal sealed class GuideViewerPresenter : IDisposable
    {
        internal GuideViewerPresenter() => PluginService.Framework.Update += this.OnFrameworkUpdate;

        public void Dispose() => PluginService.Framework.Update -= this.OnFrameworkUpdate;

        /// <summary>
        ///     Toggles the settings window.
        /// </summary>
        internal static void ToggleSettingsWindow()
        {
            if (PluginService.WindowManager.GetWindow(TWindowNames.Settings) is SettingsWindow window)
            {
                window.IsOpen ^= true;
            }
        }

        /// <summary>
        ///     Gets the current plugin version.
        /// </summary>
        internal static string PluginVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? TGenerics.Unknown;

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration Configuration => PluginService.Configuration;

        /// <summary>
        ///     The currently selected guide to show in the GuideViewer window.
        /// </summary>
        internal Guide? SelectedGuide { get; private set; }

        /// <summary>
        ///     The linked note to the currently selected guide.
        /// </summary>
        internal Note? LinkedNote { get; private set; }

        internal void SetSelectedGuide(Guide? guide)
        {
            this.SelectedGuide = guide;

            if (guide != null)
            {
                this.LinkedNote = Note.CreateOrLoad(guide.InternalName, Path.Combine(Note.DefaultLocationBase, guide.Type.GetPluralNameAttribute()));
                PluginLog.Debug($"GuideViewerPresenter(SetSelectedGuide): Note: {this.LinkedNote.Contents}");
            }
            else
            {
                this.LinkedNote = null;
            }
        }

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
        /// <param name="e"></param>
        internal void OnFrameworkUpdate(object? e)
        {
            var currentTerritory = PluginService.ClientState.TerritoryType;

            if (currentTerritory != this.currentTerritory)
            {
                this.currentTerritory = currentTerritory;
                var playerGuide = GuideUtil.GetGuideForCurrentTerritory();
                PluginLog.Debug($"GuideViewerPresenter(OnFrameworkUpdate): Player changed territory to {this.currentTerritory}. (Guide for Territory: {playerGuide?.Name ?? "None"})");

                if (playerGuide != null && playerGuide?.Sections?.Count > 0)
                {
                    this.SetSelectedGuide(playerGuide);
                    this.lastAutoSelectedGuide = playerGuide;
                    if (PluginService.Configuration.Display.AutoToggleGuideForDuty)
                    {
                        if (PluginService.WindowManager.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow window)
                        {
                            PluginLog.Debug($"GuideViewerPresenter(OnTerritoryChange): Toggling guide viewer to open and displaying guide for {playerGuide.Name}.");
                            window.IsOpen = true;
                        }
                    }
                    else if (PluginService.WindowManager.GetWindow(TWindowNames.GuideViewer)?.IsOpen == false)
                    {
                        Notifications.ShowToast(message: TGuideViewer.GuideAvailableForDuty, type: NotificationType.Info);
                    }
                }
                else if (playerGuide == null && this.lastAutoSelectedGuide != null && this.lastAutoSelectedGuide == this.SelectedGuide)
                {
                    this.SetSelectedGuide(null);
                    if (PluginService.WindowManager.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow window)
                    {
                        PluginLog.Debug("GuideViewerPresenter(OnTerritoryChange): Toggling guide viewer to closed - no guide data found for territory and last auto-selected guide is the same as the current selected guide.");
                        window.IsOpen = false;
                    }
                }
            }
        }
    }
}
