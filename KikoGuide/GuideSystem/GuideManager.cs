using System;
using System.Collections.Generic;
using KikoGuide.Common;
using KikoGuide.Enums;
using KikoGuide.UserInterface.Windows.GuideViewer;

namespace KikoGuide.GuideSystem
{
    internal sealed class GuideManager : IDisposable
    {
        /// <summary>
        /// The singleton instance of <see cref="GuideManager" />.
        /// </summary>
        public static GuideManager Instance { get; } = new();

        /// <summary>
        /// All loaded guides.
        /// </summary>
        public HashSet<GuideBase> Guides { get; private set; } = new();

        /// <summary>
        /// All loaded guides by type.
        /// </summary>
        private readonly Dictionary<ContentTypeModified, HashSet<GuideBase>> guidesByType = new();

        /// <summary>
        /// The currently active guide.
        /// </summary>
        public GuideBase? SelectedGuide { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuideManager" /> class.
        /// </summary>
        private GuideManager() => this.LoadGuides();

        /// <summary>
        /// Disposes of all guides.
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
        /// Sets the currently active guide.
        /// </summary>
        /// <param name="guide"></param>
        public void SetSelectedGuide(GuideBase guide, bool openViewer)
        {
            this.SelectedGuide = guide;
            if (openViewer)
            {
                var window = Services.WindowManager.WindowingSystem.GetWindow<GuideViewerWindow>();
                if (window != null)
                {
                    window.IsOpen = true;
                }
            }
        }

        /// <summary>
        /// Clears the currently active guide.
        /// </summary>
        public void ClearSelectedGuide()
        {
            if (this.SelectedGuide == null)
            {
                return;
            }
            this.SelectedGuide = null;
        }

        /// <summary>
        /// Gets all guides for a given type.
        /// </summary>
        /// <param name="type">The type to get guides for.</param>
        /// <returns>A <see cref="HashSet{T}" /> of <see cref="GuideBase" />.</returns>
        public HashSet<GuideBase> GetGuidesForType(ContentTypeModified type)
        {
            if (this.guidesByType.TryGetValue(type, out var guides))
            {
                return guides;
            }

            guides = new HashSet<GuideBase>();
            foreach (var guide in this.Guides)
            {
                if (guide.ContentType == type)
                {
                    guides.Add(guide);
                }
            }

            this.guidesByType.Add(type, guides);
            return guides;
        }

        /// <summary>
        /// Loads all guides.
        /// </summary>
        private void LoadGuides()
        {
            foreach (var type in typeof(GuideManager).Assembly.GetTypes())
            {
                try
                {
                    if (!type.IsAbstract && type.IsSubclassOf(typeof(GuideBase)))
                    {
                        BetterLog.Debug($"[{type.BaseType?.Name}] Loading guide from class {type.Name}.");
                        var guide = (GuideBase)type.GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>());
                        this.Guides.Add(guide);
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    BetterLog.Warning($"Failed to load guide {type.Name}: {e}");
#else
                    BetterLog.Warning($"Failed to load guide {type.Name}: {e.InnerException?.Message}");
#endif
                }
            }
        }
    }
}
