using System;
using System.Collections.Generic;
using Dalamud.Interface.Windowing;
using KikoGuide.UserInterface.Windows.GuideList;
using KikoGuide.UserInterface.Windows.GuideSettings;
using KikoGuide.UserInterface.Windows.GuideViewer;
using KikoGuide.UserInterface.Windows.IntegrationSettings;
using KikoGuide.UserInterface.Windows.PluginSettings;
using Sirensong;
using Sirensong.UserInterface.Windowing;

namespace KikoGuide.UserInterface
{
    internal sealed class WindowManager : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The windowing system.
        /// </summary>
        public WindowingSystem WindowingSystem { get; } = SirenCore.GetOrCreateService<WindowingSystem>();

        /// <summary>
        /// All windows to add to the windowing system.
        /// </summary>
        private readonly Dictionary<Window, bool> windows = new()
        {
            { new GuideListWindow(), true },
            { new GuideViewerWindow(), false },
            { new IntegrationsWindow(), false},
            { new GuideSettingsWindow(), false },
            { new PluginSettingsWindow(), false },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowManager" /> class.
        /// </summary>
        private WindowManager()
        {
            foreach (var (window, isSettings) in this.windows)
            {
                this.WindowingSystem.AddWindow(window, isSettings);
            }
        }

        /// <summary>
        /// Disposes of the window manager.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                this.WindowingSystem.Dispose();
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Sets the guide viewer window visibility.
        /// </summary>
        /// <param name="visible">Whether or not the guide viewer window should be visible.</param>
        public void SetGuideViewerWindowVis(bool visible)
        {
            if (this.WindowingSystem.TryGetWindow<GuideViewerWindow>(out var window))
            {
                window.IsOpen = visible;
            }
        }

        /// <summary>
        /// Toggles the guide viewer window visibility.
        /// </summary>
        public void ToggleGuideViewerWindow()
        {
            if (this.WindowingSystem.TryGetWindow<GuideViewerWindow>(out var window))
            {
                window.Toggle();
            }
        }

        /// <summary>
        /// Toggles the settings window visibility.
        /// </summary>
        /// <param name="visible">Whether or not the settings window should be visible.</param>
        public void ToggleSettingsWindow()
        {
            if (this.WindowingSystem.TryGetWindow<PluginSettingsWindow>(out var window))
            {
                window.Toggle();
            }
        }

        /// <summary>
        /// Toggles the guide viewer window visibility.
        /// </summary>
        public void ToggleGuideListWindow()
        {
            if (this.WindowingSystem.TryGetWindow<GuideListWindow>(out var window))
            {
                window.Toggle();
            }
        }

        /// <summary>
        /// Toggles the integration settings window visibility.
        /// </summary>
        public void ToggleIntegrationSettingsWindow()
        {
            if (this.WindowingSystem.TryGetWindow<IntegrationsWindow>(out var window))
            {
                window.Toggle();
            }
        }

        /// <summary>
        /// Toggles the guide config settings window visibility.
        /// </summary>
        public void ToggleGuideConfigSettingsWindow()
        {
            if (this.WindowingSystem.TryGetWindow<GuideSettingsWindow>(out var window))
            {
                window.Toggle();
            }
        }
    }
}
