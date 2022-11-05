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
    ///     The DutyManager works ontop of the Duty type to abstract critical tasks.
    /// </summary>
    public sealed class DutyManager : IDisposable
    {
        /// <summary>
        ///     The loaded duty cache, prevents re-reading the files every lookup.
        /// </summary>
        private List<Duty>? loadedDutyCache;

        /// <summary>
        ///     Clears the loaded duty cache and forces a re-read of the files.
        /// </summary>
        public void ClearCache() => this.loadedDutyCache = null;

        /// <summary>
        ///     Get the duty the player is currently inside of, if any.
        /// </summary>
        public Duty? GetPlayerDuty() => this.GetDuties().Find(duty => duty.TerritoryIDs.Contains(PluginService.ClientState.TerritoryType));

        public List<Duty> GetDuties()
        {
            if (this.loadedDutyCache != null)
            {
                return this.loadedDutyCache;
            }

            this.loadedDutyCache = LoadDutyData();
            return this.loadedDutyCache;
        }

        /// <summary>
        ///     Loads the duty data from the local files, this should be cached after usage.
        /// </summary>
        private static List<Duty> LoadDutyData()
        {
            PluginLog.Information($"DutyManager(LoadDutyData): Loading duty data from files, this could cause a lag spike if your storage is slow.");

            // Try and get the language from the settings, or use fallback to default if not found.
            var language = PluginService.PluginInterface.UiLanguage;
            if (!Directory.Exists($"{PluginConstants.PluginlocalizationDir}\\Duty\\{language}"))
            {
                language = PluginConstants.FallbackLanguage;
            }

            // Start loading every duty file for the language and deserialize it into the Duty type.
            var duties = Enumerable.Empty<Duty>().ToList();
            try
            {
                foreach (var file in Directory.GetFiles($"{PluginConstants.PluginlocalizationDir}\\Duty\\{language}", "*.json", SearchOption.AllDirectories))
                {
                    try
                    {
                        var duty = JsonConvert.DeserializeObject<Duty>(File.ReadAllText(file));
                        if (duty != null)
                        { duties.Add(duty); PluginLog.Verbose($"DutyManager(LoadDutyData): Loaded {duty.GetCanonicalName()}"); }
                    }
                    catch (Exception e)
                    {
                        PluginLog.Warning($"DutyManager(LoadDutyData): Failed to load duty from file {file}: {e.Message}. [Skipping]");
                    }
                }
            }
            catch
            {
                PluginLog.Error($"DutyManager(LoadDutyData): Failed to load duty data from files, you may need to reinstall the plugin or check your files for corruption.");
            }

            PluginLog.Information($"DutyManager(LoadDutyData): Loaded {duties.Count} duties for {language}");

            return duties;
        }

        /// <summary>
        ///     Disposes of the DutyManager.
        /// </summary>
        public void Dispose() => this.loadedDutyCache = null;
    }
}