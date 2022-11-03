namespace KikoGuide.Managers
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Dalamud.Logging;
    using KikoGuide.Base;
    using KikoGuide.Types;
    using FFXIVClientStructs.FFXIV.Client.Game;

    /// <summary>
    ///     The DutyManager works ontop of the Duty type to abstract critical tasks.
    /// </summary>
    public static class DutyManager
    {
        /// <summary>
        ///     The loaded duties cache, prevents re-reading the files every lookup.
        /// </summary>
        private static List<Duty>? _loadedDuties;

        /// <summary> 
        ///    Clears the loaded duties cache and forces a re-read of the files.
        /// </summary>
        public static void ClearCache() => _loadedDuties = null;

        /// <summary>
        ///     Fetches all duties from the cache or from the files if the cache is empty.
        /// </summary>
        public static List<Duty> GetDuties()
        {
            if (_loadedDuties != null) return _loadedDuties;
            return LoadDutyData();
        }

        /// <summary>
        ///     Get the duty the player is currently inside of, if any.
        /// </summary>
        public static Duty? GetPlayerDuty() => GetDuties().Find(duty => duty.TerritoryIDs.Contains(PluginService.ClientState.TerritoryType));

        /// <summary>
        ///     Get if the player has unlocked the given duty or not.
        /// </summary>
        /// <param name="duty">The duty to check </param>
        public static bool IsUnlocked(Duty duty) => duty.UnlockQuestID != 0 && QuestManager.IsQuestCurrent(duty.UnlockQuestID) || QuestManager.IsQuestComplete(duty.UnlockQuestID);

        /// <summary> 
        ///     Desearializes duties from the duty data folder into the Duty type.
        /// </summary>
        private static List<Duty> LoadDutyData()
        {
            PluginLog.Debug($"DutyManager(LoadDutyData): Loading duty data");

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
                    catch { /* File is invalid, skip it */ }
                }
            }
            catch { /* No duties files found for the language, just return an empty enumerable */ }

            PluginLog.Debug($"DutyManager(LoadDutyData): Loaded {duties.Count} duties for {language}");

            duties = duties.OrderBy(x => x.Level).ToList();
            _loadedDuties = duties;
            return duties;
        }
    }
}