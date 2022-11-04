namespace KikoGuide.Managers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Dalamud.Logging;
    using KikoGuide.Base;
    using KikoGuide.Types;

    /// <summary>
    ///     The DutyManager works ontop of the Duty type to abstract critical tasks.
    /// </summary>
    public sealed class DutyManager : IDisposable
    {
        /// <summary>
        ///     The loaded duty cache, prevents re-reading the files every lookup.
        /// </summary>
        private List<Duty>? _loadedDutyCache;

        /// <summary>
        ///     Clears the loaded duty cache and forces a re-read of the files.
        /// </summary>
        public void ClearCache() => _loadedDutyCache = null;

        /// <summary>
        ///     Get the duty the player is currently inside of, if any.
        /// </summary>
        public Duty? GetPlayerDuty() => GetDuties().Find(duty => duty.TerritoryIDs.Contains(PluginService.ClientState.TerritoryType));

        public List<Duty> GetDuties()
        {
            if (this._loadedDutyCache != null) return this._loadedDutyCache;

            this._loadedDutyCache = this.LoadDutyData();
            return this._loadedDutyCache;
        }

        /// <summary>
        ///     Loads the duty data from the local files, this should be cached after usage.
        /// </summary>
        private List<Duty> LoadDutyData()
        {
            PluginLog.Information($"DutyManager(LoadDutyData): Loading duty data from files, this could cause a lag spike if your storage is slow.");

            // Try and get the language from the settings, or use fallback to default if not found.
            var language = PluginService.PluginInterface.UiLanguage;
            if (!Directory.Exists($"{PluginConstants.pluginlocalizationDir}\\Duty\\{language}")) language = PluginConstants.fallbackLanguage;

            // Start loading every duty file for the language and deserialize it into the Duty type.
            List<Duty> duties = Enumerable.Empty<Duty>().ToList();
            try
            {
                foreach (string file in Directory.GetFiles($"{PluginConstants.pluginlocalizationDir}\\Duty\\{language}", "*.json", SearchOption.AllDirectories))
                {
                    try
                    {
                        Duty? duty = JsonConvert.DeserializeObject<Duty>(File.ReadAllText(file));
                        if (duty != null) { duties.Add(duty); PluginLog.Verbose($"DutyManager(LoadDutyData): Loaded {duty.GetCanonicalName()}"); }
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
        public void Dispose()
        {
            _loadedDutyCache = null;
        }
    }
}