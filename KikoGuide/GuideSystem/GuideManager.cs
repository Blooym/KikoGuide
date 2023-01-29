using System;
using System.Collections.Generic;
using KikoGuide.Common;
using KikoGuide.Enums;

namespace KikoGuide.GuideSystem
{
    /// <summary>
    /// The guide manager is the authoritative source for all guide management.
    /// </summary>
    internal sealed class GuideManager : IDisposable
    {
        /// <summary>
        /// The singleton instance of <see cref="GuideManager" />.
        /// </summary>
        public static GuideManager Instance { get; } = new();

        /// <summary>
        /// All loaded guides.
        /// </summary>
        private readonly HashSet<GuideBase> guides = new();

        /// <summary>
        /// All loaded guides, grouped by content type.
        /// </summary>
        private readonly Dictionary<ContentTypeModified, HashSet<GuideBase>> guidesByContentType = new();

        /// <summary>
        /// The currently selected guide.
        /// </summary>
        public GuideBase? SelectedGuide { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuideManager" /> class.
        /// </summary>
        private GuideManager() => this.guides = LoadGuides();

        /// <summary>
        /// Disposes of all guides.
        /// </summary>
        public void Dispose()
        {
            foreach (var guide in this.guides)
            {
                guide.Dispose();
            }
            this.guides.Clear();
            this.guidesByContentType.Clear();
        }

        /// <summary>
        /// Gets all available guides that are loaded.
        /// </summary>
        /// <returns>A <see cref="HashSet{T}" /> of all loaded guides.</returns>
        public HashSet<GuideBase> GetGuides() => this.guides;

        /// <summary>
        /// Gets all available guides for a given content type.
        /// </summary>
        /// <param name="contentType">The content type to get guides for.</param>
        /// <returns>A <see cref="HashSet{T}" /> of all loaded guides for the given content type.</returns>
        public HashSet<GuideBase> GetGuides(ContentTypeModified contentType)
        {
            if (this.guidesByContentType.TryGetValue(contentType, out var guides))
            {
                return guides;
            }

            guides = new HashSet<GuideBase>();
            foreach (var guide in this.guides)
            {
                if (guide.ContentType == contentType)
                {
                    guides.Add(guide);
                }
            }
            this.guidesByContentType.Add(contentType, guides);
            return guides;
        }

        /// <summary>
        /// Loads all guides from the assembly into a <see cref="HashSet{T}" />.
        /// </summary>
        private static HashSet<GuideBase> LoadGuides()
        {
            var loadedGuides = new HashSet<GuideBase>();
            foreach (var type in typeof(GuideManager).Assembly.GetTypes())
            {
                try
                {
                    if (!type.IsAbstract && type.IsSubclassOf(typeof(GuideBase)))
                    {
                        if (type.GetConstructor(Array.Empty<Type>()) == null)
                        {
                            BetterLog.Warning($"Guide {type.Name} does not have a parameterless constructor.");
                            continue;
                        }

                        BetterLog.Debug($"[{type.BaseType?.Name}] Loading guide from class {type.Name}.");
                        loadedGuides.Add((GuideBase)type.GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>()));
                    }
                }
                catch (Exception e)
                {
                    BetterLog.Warning($"Failed to load guide {type.Name}: {e}");
                }
            }
            return loadedGuides;
        }
    }
}
