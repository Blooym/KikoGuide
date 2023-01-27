using System;
using System.Collections.Generic;
using Dalamud.Interface.Windowing;
using KikoGuide.UserInterface.Windows.GuideList;
using KikoGuide.UserInterface.Windows.GuideViewer;
using KikoGuide.UserInterface.Windows.Settings;
using Sirensong;
using Sirensong.UserInterface.Windowing;

namespace KikoGuide.UserInterface
{
    internal sealed class WindowManager : IDisposable
    {
        /// <summary>
        /// Gets the singleton instance of <see cref="WindowManager" />.
        /// </summary>
        public static WindowManager Instance { get; } = new();

        /// <summary>
        /// The windowing system.
        /// </summary>
        public WindowingSystem WindowingSystem { get; } = SirenCore.GetOrCreateService<WindowingSystem>();

        /// <summary>
        /// All windows to add to the windowing system.
        /// </summary>
        private readonly Dictionary<Window, bool> windows = new()
        {
            { new GuideListWindow(), false },
            { new GuideViewerWindow(), false },
            { new SettingsWindow(), true },
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
            this.WindowingSystem.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets the guide viewer window visibility.
        /// </summary>
        /// <param name="visible">Whether or not the guide viewer window should be visible.</param>
        public void SetGuideViewerVisibility(bool visible)
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
        /// Sets the settings window visibility.
        /// </summary>
        /// <param name="visible">Whether or not the settings window should be visible.</param>
        public void SetSettingsWindowVisibility(bool visible)
        {
            if (this.WindowingSystem.TryGetWindow<SettingsWindow>(out var window))
            {
                window.IsOpen = visible;
            }
        }

        /// <summary>
        /// Toggles the settings window visibility.
        /// </summary>
        /// <param name="visible">Whether or not the settings window should be visible.</param>
        public void ToggleSettingsWindow()
        {
            if (this.WindowingSystem.TryGetWindow<SettingsWindow>(out var window))
            {
                window.Toggle();
            }
        }
    }
}
