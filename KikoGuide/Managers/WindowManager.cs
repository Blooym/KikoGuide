using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.UI.Windows.Editor;
using KikoGuide.UI.Windows.GuideList;
using KikoGuide.UI.Windows.GuideViewer;
using KikoGuide.UI.Windows.Settings;

namespace KikoGuide.Managers
{
    /// <summary>
    ///     Initializes and manages all windows and window-events for the plugin.
    /// </summary>
    internal sealed class WindowManager : IDisposable
    {
        /// <summary>
        ///     The windowing system service provided by Dalamud.
        /// </summary>
        private readonly WindowSystem windowSystem = new(PluginConstants.PluginName);

        /// <summary>
        ///     All windows managed by the WindowManager.
        /// </summary>
        private readonly List<Window> windows = new()
        {
            new SettingsWindow(),
            new GuideListWindow(),
            new EditorWindow(),
            new GuideViewerWindow()
        };

        /// <summary>
        ///     Initializes the WindowManager and associated resources.
        /// </summary>
        internal WindowManager()
        {
            PluginLog.Debug("WindowManager(WindowManager): Initializing...");

            foreach (var window in this.windows)
            {
                PluginLog.Debug($"WindowManager(WindowManager): Registering window: {window.GetType().Name}");
                this.windowSystem.AddWindow(window);
            }

            PluginService.PluginInterface.UiBuilder.Draw += this.OnDrawUI;
            PluginService.PluginInterface.UiBuilder.OpenConfigUi += this.OnOpenConfigUI;
            PluginService.ClientState.Logout += this.OnLogout;

            PluginLog.Debug("WindowManager(WindowManager): Successfully initialized.");
        }

        /// <summary>
        ///     Draws all windows for the draw event.
        /// </summary>
        private void OnDrawUI() => this.windowSystem.Draw();

        /// <summary>
        ///     Opens/Closes the plugin configuration window.
        /// </summary>
        private void OnOpenConfigUI()
        {
            if (this.windowSystem.GetWindow(TWindowNames.Settings) is SettingsWindow window)
            {
                window.IsOpen = !window.IsOpen;
            }
        }

        /// <summary>
        ///    Handles the OnLogout event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args"></param>
        public void OnLogout(object? e, EventArgs args)
        {
            foreach (var window in this.windows)
            {
                window.IsOpen = false;
            }
        }

        /// <summary>
        ///     Disposes of the WindowManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginService.PluginInterface.UiBuilder.Draw -= this.OnDrawUI;
            PluginService.PluginInterface.UiBuilder.OpenConfigUi -= this.OnOpenConfigUI;

            foreach (var window in this.windows.OfType<IDisposable>())
            {
                PluginLog.Debug($"WindowManager(Dispose): Disposing of {window.GetType().Name}...");
                window.Dispose();
            }

            this.windowSystem.RemoveAllWindows();

            PluginLog.Debug("WindowManager(Dispose): Successfully disposed.");
        }

        /// <summary>
        ///     Gets a window by its name.
        /// </summary>
        /// <param name="name"> The name of the window to get. </param>
        /// <returns> The window with the given name. </returns>
        public Window? GetWindow(string name) => this.windowSystem.GetWindow(name);
    }
}
