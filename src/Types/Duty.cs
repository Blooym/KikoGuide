namespace KikoGuide.Types;

using System;
using System.Linq;
using System.Collections.Generic;
using CheapLoc;

enum DutyType
{
    Dungeon = 0,
    Trial = 1,
    AllianceRaid = 2
}

enum DutyDifficulty
{
    Normal = 0,
    Hard = 1,
    Extreme = 2,
    Savage = 3,
    Ultimate = 4,
    Unreal = 5
}

enum DutyExpansion
{
    ARealmReborn = 0,
    Heavensward = 1,
    Stormblood = 2,
    Shadowbringers = 3,
    Endwalker = 4
}

enum DutyMechanics
{
    Tankbuster = 0,
    Enrage = 1,
    AOE = 2,
    Stackmarker = 3,
    Raidwide = 4,
    Invulnerablity = 5,
    Targetted = 6,
    AddSpawn = 7,
    DPSCheck = 8,
    Cleave = 9,
    Other = 10,
}

/// <summary>
/// Represents an in-game duty as defined by the Plugin.
/// </summary>
public class Duty
{
    // Internal value representing the version of the duty class. (Used when parsing files)
    // The defualt behaviour is to assume the duty is on the oldest version.
    private static readonly int _formatVersion = 0;

    public int Version = 0;
    public string Name = Loc.Localize("Types.Duty.Name.None", "Unnamed Duty");
    public int Difficulty = (int)DutyDifficulty.Normal;
    public int Expansion = (int)DutyExpansion.ARealmReborn;
    public int Type = (int)DutyType.Dungeon;
    public int Level = 0;
    public uint UnlockQuestID = 0;
    public uint TerritoryID = 0;
    public List<Boss>? Bosses = null;

    /// <summary> The Boss class represents a boss in a duty. </summary>
    public class Boss
    {
        public string? Name = Loc.Localize("Types.Duty.Boss.Name.None", "Unnamed Boss");
        public string? Strategy = Loc.Localize("Types.Duty.Boss.Strategy.None", "No strategy available for this boss.");
        public string? TLDR;
        public List<KeyMechanic>? KeyMechanics;

        /// <summary> The KeyMechanic class represents a key mechanic in a boss. </summary>
        public class KeyMechanic
        {
            public string Name { get; set; } = "???";
            public string Description { get; set; } = "???";
            public string? TLDR { get; set; }
            public int Type { get; set; } = (int)DutyMechanics.Other;
        }
    }

    /// <summary> Boolean value indicating if this duty is not supported on the current plugin version. </summary>
    public bool IsSupported()
    {
        // Check a bunch of things to make sure the duty is supported, like enum values & format version.
        if (this.Version != _formatVersion) return false;
        if (!Enum.IsDefined(typeof(DutyExpansion), this.Expansion)) return false;
        if (!Enum.IsDefined(typeof(DutyType), this.Type)) return false;
        if (!Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)) return false;
        if (this.Bosses?.Any(boss => boss.KeyMechanics != null &&
            boss.KeyMechanics.Any(keyMechanic => !Enum.IsDefined(typeof(DutyMechanics), keyMechanic.Type))) ?? false) return false;

        // If nothing else returns false, then the duty is supported.
        return true;
    }


    /// <summary> Method to get the duties canonical name. </summary>
    public string GetCanonicalName()
    {
        if (this.Difficulty != (int)DutyDifficulty.Normal) return $"{this.Name} ({Enum.GetName(typeof(DutyDifficulty), this.Difficulty)})";
        else return this.Name;
    }
}
