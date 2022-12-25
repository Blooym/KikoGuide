using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.Types;

namespace KikoGuide.Managers
{
    /// <summary>
    ///     The GuideManager loads and manages guide data.
    /// </summary>
    internal sealed class GuideManager : IDisposable
    {
        /// <summary>
        ///     The guide version folder to load from.
        /// </summary>
        private const string GuideVersionFolder = "v1";

        /// <summary>
        ///     The loaded guide cache, prevents re-reading the files every lookup.
        /// </summary>
        private List<Guide>? loadedGuideCache;

        /// <summary>
        ///     The GuideManager constructor.
        /// </summary>
        internal GuideManager() => PluginService.ResourceManager.ResourcesUpdated += this.OnResourceUpdate;

        /// <summary>
        ///     Disposes of the GuideManager.
        /// </summary>
        public void Dispose() => PluginService.ResourceManager.ResourcesUpdated -= this.OnResourceUpdate;

        /// <summary>
        ///     Clears the loaded guide cache and forces a re-read of the files.
        /// </summary>
        private void OnResourceUpdate() => this.loadedGuideCache = null;

        /// <summary>
        ///     Gets the cached guide list or loads them from the file system if the cache is empty.
        /// </summary>
        internal List<Guide> GetAllGuides()
        {
            if (this.loadedGuideCache != null)
            {
                return this.loadedGuideCache;
            }

            this.loadedGuideCache = LoadGuideData();
            return this.loadedGuideCache;
        }

        /// <summary>e
        ///     Loads the guide data from the local files, this should be cached after usage.
        /// </summary>
        private static List<Guide> LoadGuideData()
        {
            PluginLog.Information("GuideManager(LoadGuideData): Loading guide data from files, this could cause a lag spike if your storage is slow");

            // Try and get the language from the settings, or use fallback to default if not found.
            var language = PluginService.PluginInterface.UiLanguage;
            var directory = $"{PluginConstants.PluginlocalizationDir}\\Guide\\{GuideVersionFolder}\\{language}";
            if (!Directory.Exists(directory))
            {
                directory = $"{PluginConstants.PluginlocalizationDir}\\Guide\\{GuideVersionFolder}\\{PluginConstants.FallbackLanguage}";
            }

            // Start loading every guide file for the language and deserialize it into the guide type.
            var guides = Enumerable.Empty<Guide>().ToList();
            try
            {
                foreach (var file in Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories))
                {
                    try
                    {
                        var guide = Guide.FromJson(File.ReadAllText(file));
                        var errorMessage = $"GuideManager(LoadGuideData): Failed to load guide file {file}:";

                        if (guide.Disabled)
                        {
                            PluginLog.Verbose($"GuideManager(LoadGuideData): {guide.GetCanonicalName()} ({guide.InternalName}) is disabled, skipping");
                            continue;
                        }

                        if (guide.InternalName == guides.Find(g => g.InternalName == guide.InternalName)?.InternalName)
                        {
                            PluginLog.Warning($"{errorMessage} Duplicate internal name ({guide.InternalName})");
                            continue;
                        }

                        guides.Add(guide);
                        PluginLog.Verbose($"GuideManager(LoadGuideData): Loaded {guide.GetCanonicalName()} ({guide.InternalName})");
                    }
                    catch (Exception e)
                    {
                        PluginLog.Warning($"GuideManager(LoadGuideData): Failed to load guide from file {file}: {e.Message}");
                    }
                }
            }
            catch
            {
                PluginLog.Error("GuideManager(LoadGuideData): Failed to load guide data from files, you may need to reinstall the plugin or check your files for corruption.");
            }

            PluginLog.Information($"GuideManager(LoadGuideData): Loaded {guides.Count} guides for {language}");
            return guides;
        }
    }
}
