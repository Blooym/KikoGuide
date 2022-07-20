namespace KikoGuide.Managers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FFXIVClientStructs.FFXIV.Client.Game;
using Dalamud.Logging;

using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.Utils;

// <summary>
// Classto deserialize the duty data
// </summary>
public class Duty
{
    public int Version { get; set; } = 0;
    public string Name { get; set; } = "Unnamed Duty";
    public int Difficulty { get; set; } = 0;
    public int Expansion { get; set; } = 0;
    public int Type { get; set; } = 0;
    public int Level { get; set; } = 0;
    public uint UnlockQuestID { get; set; } = 0;
    public uint TerritoryID { get; set; } = 0;
    public bool UpdateRequired { get; set; } = false;
    public List<Boss>? Bosses { get; set; }
}


// <summary>
// Class to deserialize the boss data for a duty. 
// </summary>
public class Boss
{
    public string? Name { get; set; }
    public string? Strategy { get; set; }
    public string? TLDR { get; set; }
    public List<KeyMechanics>? KeyMechanics { get; set; }
}


// <summary>
// Class to deserialize the key mechanic data for a boss
// </summary>
public class KeyMechanics
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Type { get; set; } = 10;
}


// <summary>
// DutyManager handles the loading & controling of the duty data.
// </summary>
public static class DutyManager
{
    /* 
    DutyJSONVersion should be incremented whenever keys are renamed, removed or the structure of the JSON file changes.
    It does not need to be incremented when a new key is added to the duty, as it will just be ignored when deserializing.
    */
    private static int _dutyJSONVersion = 0;

#if !DEBUG
    private static bool _autoUpdateAttempted = false;
#endif

    private static CacheManager _dutyCacheMgr = new CacheManager(5000, new List<Duty>());
    private static List<string> _disabledDutyLoadPaths = new List<string>();


    // <summary>
    // Checks if the given duty supported by the current version of the plugin.
    // </summary>
    private static bool IsSupported(Duty duty)
    {
        if (duty.Version != _dutyJSONVersion) return false;
        if (!Enum.IsDefined(typeof(Expansion), duty.Expansion)) return false;
        if (!Enum.IsDefined(typeof(DutyType), duty.Type)) return false;
        if (!Enum.IsDefined(typeof(DutyDifficulty), duty.Difficulty)) return false;
        if (duty.Bosses?.Any(boss => boss.KeyMechanics != null && boss.KeyMechanics.Any(keyMechanic => !Enum.IsDefined(typeof(Mechanics), keyMechanic.Type))) ?? false) return false;

        return true;
    }


    // <summary>
    // Returns a list of all duty data available for the current language, sorted by ascending level.
    // </summary>
    public static List<Duty> GetDuties()
    {
        // if the duties list is empty or the cache has expired, reload the data.
        if (!_dutyCacheMgr.HasExpired() && _dutyCacheMgr.IsValid()) return (List<Duty>)_dutyCacheMgr.GetCacheItem();

#if !DEBUG
        // If an update of resource data has not been made yet, attempt to do once.
        if (!_autoUpdateAttempted)
        {
            UpdateManager.UpdateResources();
            _autoUpdateAttempted = true;
        }
#endif

        PluginLog.Debug($"DutyManager: Refreshing loaded duties...");
        var language = Service.PluginInterface.UiLanguage;

        // Fetch all duty data from the duty resources folder for the current language, or fallback on english
        List<Duty> duties = new List<Duty>();
        if (!Directory.Exists($"{FS.resourcePath}Localization\\Duty\\{language}")) language = "en";

        try
        {

            foreach (string file in Directory.GetFiles($"{FS.resourcePath}Localization\\Duty\\{language}", "*.json", SearchOption.AllDirectories))
            {
                try
                {
                    if (_disabledDutyLoadPaths?.Contains(file) == true) continue;
                    Duty? duty = Newtonsoft.Json.JsonConvert.DeserializeObject<Duty>(System.IO.File.ReadAllText(file));

                    if (duty == null) continue;
                    if (!IsSupported(duty)) duty.UpdateRequired = true;

                    duties.Add(duty);
                }

                catch (Exception e)
                {
                    _disabledDutyLoadPaths?.Add(file);
                    PluginLog.Error($"DutyManager: Disabling duty - Could not deserialize duty {file}: {e.Message}");
                }
            }
        }

        catch (Exception)
        {
            _dutyCacheMgr.SetCacheItem(duties);
            return duties;
        }

        PluginLog.Debug($"DutyManager: Loaded {duties.Count} duties.");

        // lower levels at the top of the list, higher levels at the bottom of the list.
        duties = duties.OrderBy(x => x.Level).ToList();

        _dutyCacheMgr.SetCacheItem(duties);
        return duties;
    }


    //<summary>
    // Returns a boolean value indicating if the duty has got enough data.
    //</summary>
    public static bool HasData(Duty duty) => duty != null && duty.Bosses?.Count > 0;


    // <summary>
    // Returns a boolean value indicating if the duty has been unlocked by the player.
    // </summary>
    public static bool IsDutyUnlocked(Duty duty) => GetPlayerDuty() == duty || QuestManager.IsQuestComplete(duty.UnlockQuestID);


    // <summary>
    // Returns the duty the player is currently inside of. Returns null if the player is not inside of a known duty.
    // </summary>
    public static Duty? GetPlayerDuty() => GetDuties().Find(x => Service.ClientState.TerritoryType == x.TerritoryID);
}