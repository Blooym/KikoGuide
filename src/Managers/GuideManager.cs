using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.Types;
using Newtonsoft.Json;

namespace KikoGuide.Managers
{
    /// <summary>
    ///     The GuideManager works ontop of the Guide type to abstract critical tasks.
    /// </summary>
    public static class GuideManager
    {
        /// <summary>
        ///     The loaded guide cache, prevents re-reading the files every lookup.
        /// </summary>
        private static List<Guide>? loadedGuideCache;

        /// <summary>
        ///     Clears the loaded guide cache and forces a re-read of the files.
        /// </summary>
        public static void ClearCache() => loadedGuideCache = null;

        /// <summary>
        ///     Get the guide for the territory the player is currently in.
        /// </summary>
        public static Guide? GetGuideForCurrentTerritory() => GetGuides().Find(guide => guide.TerritoryIDss.Contains(PluginService.ClientState.TerritoryType));

        public static List<Guide> GetGuides()
        {
            if (loadedGuideCache != null)
            {
                return loadedGuideCache;
            }

            loadedGuideCache = LoadGuideData();
            return loadedGuideCache;
        }

        /// <summary>
        ///     Loads the guide data from the local files, this should be cached after usage.
        /// </summary>
        private static List<Guide> LoadGuideData()
        {
            PluginLog.Information($"GuideManager(LoadGuideData): Loading guide data from files, this could cause a lag spike if your storage is slow");

            // Try and get the language from the settings, or use fallback to default if not found.
            var language = PluginService.PluginInterface.UiLanguage;
            if (!Directory.Exists($"{PluginConstants.PluginlocalizationDir}\\Guide\\{language}"))
            {
                language = PluginConstants.FallbackLanguage;
            }

            // Start loading every guide file for the language and deserialize it into the guide type.
            var guides = Enumerable.Empty<Guide>().ToList();
            try
            {
                foreach (var file in Directory.GetFiles($"{PluginConstants.PluginlocalizationDir}\\Guide\\{language}", "*.json", SearchOption.AllDirectories))
                {
                    try
                    {
                        var guide = JsonConvert.DeserializeObject<Guide>(File.ReadAllText(file));
                        if (guide != null)
                        { guides.Add(guide); PluginLog.Verbose($"GuideManager(LoadGuideData): Loaded {guide.GetCanonicalName()}"); }
                    }
                    catch (Exception e)
                    {
                        PluginLog.Warning($"GuideManager(LoadGuideData): Failed to load guide from file {file}: {e.Message}. [Skipping]");
                    }
                }
            }
            catch
            {
                PluginLog.Error($"GuideManager(LoadGuideData): Failed to load guide data from files, you may need to reinstall the plugin or check your files for corruption.");
            }

            PluginLog.Information($"GuideManager(LoadGuideData): Loaded {guides.Count} guides for {language}");

            return guides;
        }
    }
}