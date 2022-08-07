namespace KikoGuide.Managers;

using System.IO;
using System.Linq;
using System.Collections.Generic;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.Types;
using Newtonsoft.Json;
using FFXIVClientStructs.FFXIV.Client.Game;


/// <summary> The DutyManager works ontop of the Duty type to abstract critical tasks. </summary>
public static class DutyManager
{
    /// <summary> All currently loaded duties </summary>
    private static List<Duty>? _loadedDuties = LoadDutyData();


    /// <summary> Handles updating duty data when resources are updated. </summary>
    internal static void ClearCache() => _loadedDuties = null;


    /// <summary> All loaded duties from the DutyManager (Cached). </summary>
    public static List<Duty> GetDuties()
    {
        if (_loadedDuties != null) return _loadedDuties;
        return LoadDutyData();
    }


    /// <summary> Getthe duty the player is currently inside of, if any. </summary>
    public static Duty? GetPlayerDuty() => GetDuties().Find(x => PluginService.ClientState.TerritoryType == x.TerritoryID);


    /// <summary> Get if the player has unlocked the given duty or not. </summary>
    public static bool IsUnlocked(Duty duty) => QuestManager.IsQuestCurrent(duty.UnlockQuestID) || QuestManager.IsQuestComplete(duty.UnlockQuestID);


    /// <summary> Desearializes duties from the duty data folder into the Duty type. </summary>
    private static List<Duty> LoadDutyData()
    {
        PluginLog.Debug($"DutyManager: Loading duty data");

        // Try and get the language from the settings, or use fallback to default if not found.
        var language = PluginService.PluginInterface.UiLanguage;
        if (!Directory.Exists($"{PStrings.localizationPath}\\Duty\\{language}")) language = PStrings.fallbackLanguage;

        // Start loading duties, if this fails then the plugin will fallback to an empty duty list.
        List<Duty> duties = new List<Duty>();
        try
        {
            foreach (string file in Directory.GetFiles($"{PStrings.localizationPath}\\Duty\\{language}", "*.json", SearchOption.AllDirectories))
            {
                // Try and deserialize the duty data and add it to the list if its not null.
                try
                {
                    Duty? duty = JsonConvert.DeserializeObject<Duty>(File.ReadAllText(file));
                    if (duty != null) duties.Add(duty);
                }
                catch { } // If this fails, just skip the file and move on.
            }
        }
        catch { } // If this fails, we can continue on without duty files just fine.

        PluginLog.Debug($"DutyManager: Loaded {duties.Count} duties for {language}");
        duties = duties.OrderBy(x => x.Level).ToList();

        _loadedDuties = duties;

        return duties;
    }
}