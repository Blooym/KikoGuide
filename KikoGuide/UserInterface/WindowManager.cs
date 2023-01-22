using System;
using System.Collections.Generic;
using Dalamud.Interface.Windowing;
using KikoGuide.UserInterface.Windows;
using Sirensong;
using Sirensong.UserInterface.Windowing;

namespace KikoGuide.UserInterface
{
    public class WindowManager : IDisposable
    {
        /// <summary>
        ///     The singleton instance of <see cref="WindowManager" />.
        /// </summary>
        private static WindowManager? instance;

        /// <summary>
        ///     Gets the singleton instance of <see cref="WindowManager" />.
        /// </summary>
        public static WindowManager Instance => instance ??= new WindowManager();

        /// <summary>
        ///     The windowing system.
        /// </summary>
        public WindowingSystem WindowingSystem { get; } = SirenCore.GetOrCreateService<WindowingSystem>();

        /// <summary>
        ///     All windows to add to the windowing system.
        /// </summary>
        private readonly Dictionary<Window, bool> windows = new()
        {
            { new GuideEditorWindow(), false },
            { new GuideListWindow(), false },
            { new GuideViewerWindow(), false },
            { new SettingsWindow(), true },
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowManager" /> class.
        /// </summary>
        private WindowManager()
        {
            foreach (var (window, isSettings) in this.windows)
            {
                this.WindowingSystem.AddWindow(window, isSettings);
            }
        }

        /// <summary>
        ///     Disposes of the window manager.
        /// </summary>
        public void Dispose()
        {
            this.WindowingSystem.Dispose();
            instance = null;
            GC.SuppressFinalize(this);
        }
    }
}
