using System;
using System.Collections.Generic;
using System.Linq;
using KikoGuide.Common;
using Sirensong.Game.Enums;

namespace KikoGuide.GuideSystem
{
    /// <summary>
    /// The guide manager is the authoritative source for all guide management.
    /// </summary>
    internal sealed class GuideManager : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// All loaded guides.
        /// </summary>
        private readonly HashSet<GuideBase> guides = new();

        /// <summary>
        /// All loaded guides, grouped by content type.
        /// </summary>
        private readonly Dictionary<ContentType, HashSet<GuideBase>> guidesByContentType = new();

        /// <summary>
        /// All loaded guides, grouped by inheritance.
        /// </summary>
        private readonly Dictionary<Type, HashSet<GuideBase>> guidesByInheritance = new();

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
            if (!this.disposedValue)
            {
                foreach (var guide in this.guides)
                {
                    guide.Dispose();
                }
                this.guides.Clear();
                this.guidesByContentType.Clear();

                this.disposedValue = true;
            }
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
        public HashSet<GuideBase> GetGuides(ContentType contentType)
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
        /// Gets all available guides that inherit from a given type.
        /// </summary>
        /// <typeparam name="T">The type to get guides for.</typeparam>
        /// <returns>A <see cref="HashSet{T}" /> of all loaded guides that inherit from the given type.</returns>
        public HashSet<T>? GetGuides<T>() where T : GuideBase
        {
            if (this.guidesByInheritance.TryGetValue(typeof(T), out _))
            {
                return this.guides.Where(guide => guide is T).Cast<T>().ToHashSet();
            }

            var guides = new HashSet<GuideBase>();
            foreach (var guide in this.guides)
            {
                if (guide is T)
                {
                    guides.Add(guide);
                }
            }
            this.guidesByInheritance.Add(typeof(T), guides);
            return guides.Cast<T>().ToHashSet();
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
