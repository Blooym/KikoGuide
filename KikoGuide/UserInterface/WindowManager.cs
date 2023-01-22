using System;
using System.Linq;
using System.Reflection;
using Dalamud.Interface.Windowing;
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
        ///     Initializes a new instance of the <see cref="WindowManager" /> class.
        /// </summary>
        private WindowManager()
        {
            foreach (var windowType in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Window)) && !t.IsAbstract))
            {
                var window = (Window?)Activator.CreateInstance(windowType);
                if (window == null)
                {
                    continue;
                }
                this.WindowingSystem.AddWindow(window);
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
