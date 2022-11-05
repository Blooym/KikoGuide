using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.UI.Windows.DutyInfo;
using KikoGuide.UI.Windows.DutyList;
using KikoGuide.UI.Windows.Editor;
using KikoGuide.UI.Windows.Settings;

namespace KikoGuide.Managers
{
    /// <summary>
    ///     Initializes and manages all windows and window-events for the plugin.
    /// </summary>
    internal sealed class WindowManager : IDisposable
    {
        public static readonly string SettingsWindowName = $"{PluginConstants.PluginName} - Settings";
        public static readonly string DutyListWindowName = $"{PluginConstants.PluginName} - Duty Finder";
        public static readonly string DutyInfoWindowName = $"{PluginConstants.PluginName} - Duty Info";
        public static readonly string EditorWindowName = $"{PluginConstants.PluginName} - Editor";

        /// <summary>
        ///     The windowing system service provided by Dalamud.
        /// </summary>
        public readonly WindowSystem WindowSystem = new(PluginConstants.PluginName);

        /// <summary>
        ///     All windows managed by the WindowManager.
        /// </summary>
        private readonly List<Window> windows = new()
        {
            new SettingsWindow(),
            new DutyListWindow(),
            new EditorWindow(),
            new DutyInfoWindow()
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
                this.WindowSystem.AddWindow(window);
            }

            PluginService.PluginInterface.UiBuilder.Draw += this.OnDrawUI;
            PluginService.PluginInterface.UiBuilder.OpenConfigUi += this.OnOpenConfigUI;

            PluginLog.Debug("WindowManager(WindowManager): Successfully initialized.");
        }

        /// <summary>
        ///     Draws all windows for the draw event.
        /// </summary>
        private void OnDrawUI() => this.WindowSystem.Draw();

        /// <summary>
        ///     Opens/Closes the plugin configuration window. 
        /// </summary> 
        private void OnOpenConfigUI()
        {
            if (this.WindowSystem.GetWindow(SettingsWindowName) is SettingsWindow window)
            {
                window.IsOpen = !window.IsOpen;
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

            this.WindowSystem.RemoveAllWindows();

            PluginLog.Debug("WindowManager(Dispose): Successfully disposed.");
        }
    }
}