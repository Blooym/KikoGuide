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
        public static readonly string SettingsWindowName = $"{PluginConstants.pluginName} - Settings";
        public static readonly string DutyListWindowName = $"{PluginConstants.pluginName} - Duty Finder";
        public static readonly string DutyInfoWindowName = $"{PluginConstants.pluginName} - Duty Info";
        public static readonly string EditorWindowName = $"{PluginConstants.pluginName} - Editor";

        /// <summary>
        ///     The windowing system service provided by Dalamud.
        /// </summary>
        public readonly WindowSystem windowSystem = new(PluginConstants.pluginName);

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


            foreach (Window window in windows)
            {
                PluginLog.Debug($"WindowManager(WindowManager): Registering window: {window.GetType().Name}");
                windowSystem.AddWindow(window);
            }

            PluginService.PluginInterface.UiBuilder.Draw += OnDrawUI;
            PluginService.PluginInterface.UiBuilder.OpenConfigUi += OnOpenConfigUI;

            PluginLog.Debug("WindowManager(WindowManager): Successfully initialized.");
        }

        /// <summary>
        ///     Draws all windows for the draw event.
        /// </summary>
        private void OnDrawUI()
        {
            windowSystem.Draw();
        }

        /// <summary>
        ///     Opens/Closes the plugin configuration window. 
        /// </summary> 
        private void OnOpenConfigUI()
        {
            if (windowSystem.GetWindow(SettingsWindowName) is SettingsWindow window)
            {
                window.IsOpen = !window.IsOpen;
            }
        }

        /// <summary>
        ///     Disposes of the WindowManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginService.PluginInterface.UiBuilder.Draw -= OnDrawUI;
            PluginService.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUI;

            foreach (IDisposable window in windows.OfType<IDisposable>())
            {
                PluginLog.Debug($"WindowManager(Dispose): Disposing of {window.GetType().Name}...");
                window.Dispose();
            }

            windowSystem.RemoveAllWindows();

            PluginLog.Debug("WindowManager(Dispose): Successfully disposed.");
        }
    }
}