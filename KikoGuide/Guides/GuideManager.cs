using System;
using System.Collections.Generic;
using KikoGuide.Common;

namespace KikoGuide.Guides
{
    public class GuideManager : IDisposable
    {
        /// <summary>
        ///     The singleton instance of <see cref="GuideManager" />.
        /// </summary>
        public static GuideManager Instance { get; } = new();

        /// <summary>
        ///     The current guide.
        /// </summary>
        public Guide? CurrentGuide { get; set; }

        /// <summary>
        ///     All loaded guides.
        /// </summary>
        public List<Guide> Guides { get; private set; } = new();

        /// <summary>
        ///     Initializes a new instance of the <see cref="GuideManager" /> class.
        /// </summary>
        private GuideManager() => this.LoadGuides();

        /// <summary>
        ///     Disposes of all guides.
        /// </summary>
        public void Dispose()
        {
            foreach (var guide in this.Guides)
            {
                guide.Dispose();
            }
            this.Guides.Clear();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Loads all guides.
        /// </summary>
        private void LoadGuides()
        {
            foreach (var type in typeof(GuideManager).Assembly.GetTypes())
            {
                try
                {
                    if (type.IsSubclassOf(typeof(Guide)))
                    {
                        if (type.GetProperty("NoLoad")?.GetValue(null) is bool noLoad && noLoad)
                        {
                            continue;
                        }
                        this.Guides.Add((Guide)type.GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>()));
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    BetterLog.Warning($"Failed to load guide {type.Name}: {e}");
#else
                    BetterLog.Warning($"Failed to load guide {type.Name}: {e.InnerException.Message}");
#endif
                }
            }
        }

        /// <summary>
        ///     Reloads all guides.
        /// </summary>
        public void ReloadGuides()
        {
            this.Guides.Clear();
            this.LoadGuides();
        }
    }
}
