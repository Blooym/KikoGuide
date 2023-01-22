using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CheapLoc;
using KikoGuide.Common;
using KikoGuide.DataModels;

namespace KikoGuide.Resources
{
    public class ResourceManager : IDisposable
    {
        /// <summary>
        ///     The singleton instance of <see cref="ResourceManager" />.
        /// </summary>
        private static ResourceManager? instance;

        /// <summary>
        ///     Gets the singleton instance of <see cref="ResourceManager" />.
        /// </summary>
        public static ResourceManager Instance => instance ??= new();

        /// <summary>
        ///     All currently loaded guides.
        /// </summary>
        private List<Guide>? loadedGuideCache;

        /// <summary>
        ///     Creates a new resource manager and sets up resources.
        /// </summary>
        private ResourceManager()
        {
            SetupLocalization(Services.PluginInterface.UiLanguage);
            Services.PluginInterface.LanguageChanged += this.OnLanguageChange;
        }

        /// <summary>
        ///     Disposes of the <see cref="ResourceManager" />
        /// </summary>
        public void Dispose()
        {
            Services.PluginInterface.LanguageChanged -= this.OnLanguageChange;
            instance = null!;
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Guide> GetAllGuides()
        {
            if (this.loadedGuideCache != null)
            {
                return this.loadedGuideCache;
            }
            this.loadedGuideCache = LoadAllGuides();
            if (this.loadedGuideCache == null)
            {
                throw new InvalidOperationException("Could not load guides.");
            }
            return this.loadedGuideCache;
        }

        /// <summary>
        ///     Loads all guides from the embedded resources.
        /// </summary>
        /// <returns>A list of all loaded guides.</returns>
        private static List<Guide>? LoadAllGuides()
        {
            try
            {
                var guides = new List<Guide>();
                var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                    .Where(x =>
                            x.StartsWith("KikoGuide.Resources.Guides", StringComparison.InvariantCultureIgnoreCase)
                            && x.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase));

                // Load all guides
                foreach (var resource in resources)
                {
                    try
                    {
                        BetterLog.Debug($"Loading guide from resource {resource}");
                        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
                        if (stream == null)
                        {
                            throw new FileNotFoundException($"Could not find resource file {resource}.");
                        }

                        using var reader = new StreamReader(stream);
                        var guide = Guide.FromJson(reader.ReadToEnd());
                        guides.Add(guide);
                    }
                    catch (Exception e)
                    {
                        BetterLog.Warning($"Could not load guide from resource {resource}, skipping. Error: {e.Message}");
                    }
                }
                return guides;
            }
            catch (Exception e)
            {
                BetterLog.Error($"Could not load guides from resources. Error: {e.Message}");
                return null;
            }
        }

        /// <summary>
        ///     Language change handler.
        /// </summary>
        /// <param name="newLanguage">The new language</param>
        private void OnLanguageChange(string newLanguage) => SetupLocalization(newLanguage);

        /// <summary>
        ///     Sets up localization for the given language, or uses fallbacks if not found.
        /// </summary>
        /// <param name="language">The language to use.</param>
        private static void SetupLocalization(string language)
        {
            try
            {
                using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"KikoGuide.Resources.Localization.{language}.json");

                if (resource == null)
                {
                    throw new FileNotFoundException($"Could not find resource file for language {language}.");
                }

                using var reader = new StreamReader(resource);
                Loc.Setup(reader.ReadToEnd());
                BetterLog.Debug($"Loaded localization for language {language}.");
            }
            catch (Exception)
            {
                BetterLog.Debug("Using fallback language for localization.");
                Loc.SetupWithFallbacks();
            }
        }
    }
}
