namespace KikoGuide.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KikoGuide.Base;
    using KikoGuide.UI.Windows.Settings;
    using KikoGuide.UI.Windows.DutyList;
    using KikoGuide.UI.Windows.Editor;
    using KikoGuide.UI.Windows.DutyInfo;
    using Dalamud.Logging;
    using Dalamud.Interface.Windowing;

    /// <summary>
    ///     Initializes and manages all windows and window-events for the plugin.
    /// </summary>
    internal sealed class WindowManager : IDisposable
    {
        /// <summary>
        ///     The windowing system service provided by Dalamud.
        /// </summary>
        public readonly WindowSystem windowSystem = new WindowSystem(PStrings.pluginName);

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
                this.windowSystem.AddWindow(window);
            }

            PluginService.PluginInterface.UiBuilder.Draw += OnDrawUI;
            PluginService.PluginInterface.UiBuilder.OpenConfigUi += OnOpenConfigUI;

            PluginLog.Debug("WindowManager(WindowManager): Successfully initialized.");
        }

        /// <summary>
        ///     Draws all windows for the draw event.
        /// </summary>
        private void OnDrawUI() => windowSystem.Draw();

        /// <summary>
        ///     Opens/Closes the plugin configuration window. 
        /// </summary> 
        private void OnOpenConfigUI()
        {
            if (this.windowSystem.GetWindow("Settings") is SettingsWindow window)
                window.IsOpen = !window.IsOpen;
        }

        /// <summary>
        ///     Disposes of the WindowManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginService.PluginInterface.UiBuilder.Draw -= OnDrawUI;
            PluginService.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUI;

            foreach (var window in this.windows.OfType<IDisposable>())
            {
                PluginLog.Debug($"WindowManager(Dispose): Disposing of {window.GetType().Name}...");
                window.Dispose();
            }

            this.windowSystem.RemoveAllWindows();

            PluginLog.Debug("WindowManager(Dispose): Successfully disposed.");
        }
    }
}