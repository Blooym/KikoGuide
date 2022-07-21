namespace KikoGuide.Managers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KikoGuide.Base;
using KikoGuide.Enums;
using FFXIVClientStructs.FFXIV.Client.Game;
using Dalamud.Logging;
using CheapLoc;


/// <summary>
///     The Duty class represents an in-game duty.
/// </summary>
sealed public class Duty
{
    /// <summary> Duty JSON format version, incremented on breaking changes. </summary>
    private readonly int _formatVersion = 0;

    public int Version { get; set; } = 0;
    public string Name { get; set; } = "Unnamed Duty";
    public int Difficulty { get; set; } = 0;
    public int Expansion { get; set; } = 0;
    public int Type { get; set; } = 0;
    public int Level { get; set; } = 0;
    public uint UnlockQuestID { get; set; } = 0;
    public uint TerritoryID { get; set; } = 0;
    public List<Boss>? Bosses { get; set; } = null;

    /// <summary> Boolean value indicating if this duty is not supported on the current plugin version. </summary>
    public bool IsSupported()
    {
        if (this.Version != _formatVersion) return false;
        if (!Enum.IsDefined(typeof(Expansion), this.Expansion)) return false;
        if (!Enum.IsDefined(typeof(DutyType), this.Type)) return false;
        if (!Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)) return false;
        if (this.Bosses?.Any(boss => boss.KeyMechanics != null &&
            boss.KeyMechanics.Any(keyMechanic => !Enum.IsDefined(typeof(Mechanics), keyMechanic.Type))) ?? false) return false;

        return true;
    }


    /// <summary> Boolean value indicating if the duty has been unlocked by the player. </summary>
    public bool IsUnlocked() => DutyManager.GetPlayerDuty() == this || QuestManager.IsQuestComplete(this.UnlockQuestID);
}


/// <summary>
///     The base Boss type, typically found within a <see cref="Duty" />.
/// </summary>
sealed public class Boss
{
    public string Name { get; set; } = Loc.Localize("Duty.Boss.Name.None", "Unnamed Boss");
    public string Strategy { get; set; } = Loc.Localize("Duty.Boss.Strategy.None", "No strategy available yet.");
    public string? TLDR { get; set; }
    public List<KeyMechanics>? KeyMechanics { get; set; }
}


/// <summary>
///      The base KeyMechanic type, typically found within a <see cref="Boss" />.
/// </summary>
sealed public class KeyMechanics
{
    public string Name { get; set; } = "???";
    public string Description { get; set; } = "???";
    public int Type { get; set; } = 10;
}


/// <summary> 
///     DutyManager handles the fetching and controling of the duty related data.
/// </summary>
public static class DutyManager
{
    /// <summary> All currently loaded duties </summary>
    private static List<Duty>? _loadedDuties = null;


    /// <summary> Handles updating duty data when resources are updated. </summary>
    public static void OnResourceUpdate() => _loadedDuties = null;


    /// <summary>
    ///     Desearializes duty data from the duty data folder, attempts to use client language.
    ///  </summary>
    public static List<Duty> LoadDutyData()
    {
        PluginLog.Debug($"DutyManager: Loading duty data");

        // Try and get the language from the settings, or use fallback to default if not found.
        var language = Service.PluginInterface.UiLanguage;
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
                    Duty? duty = Newtonsoft.Json.JsonConvert.DeserializeObject<Duty>(System.IO.File.ReadAllText(file));
                    if (duty != null) duties.Add(duty);
                }
                catch { } // If this fails, just skip the file and move on.
            }
        }
        catch { } // If this fails, we can continue on without duty files just fine .

        PluginLog.Debug($"DutyManager: Loaded {duties.Count} duties.");
        duties = duties.OrderBy(x => x.Level).ToList();

        _loadedDuties = duties;
        return duties;
    }


    /// <summary> 
    ///     Returns all currently loaded valid duty data.
    /// </summary>
    public static List<Duty> GetDuties()
    {
        if (_loadedDuties != null) return _loadedDuties;
        _loadedDuties = LoadDutyData();
        return _loadedDuties;
    }


    /// <summary>
    ///     Returns the duty the player is currently inside of. Returns null if the player is not inside of a known duty.
    /// </summary>
    public static Duty? GetPlayerDuty() => GetDuties().Find(x => Service.ClientState.TerritoryType == x.TerritoryID);
}