namespace KikoGuide.Types;

using System;
using System.Linq;
using System.Collections.Generic;
using KikoGuide.Base;


/// <summary>
///     Represents an in-game duty.
/// </summary>
public class Duty
{
    /// <summary>
    ///      Internal value representing the version of the duty class, incremented on breaking format changes.
    /// </summary>
    private static readonly int _formatVersion = 1;

    public int Version;
    public string Name = TStrings.TypeDutyUnnamed;
    public string CanconicalName => GetCanonicalName();
    public int Difficulty = (int)DutyDifficulty.Normal;
    public int Expansion = (int)DutyExpansion.ARealmReborn;
    public int Type = (int)DutyType.Dungeon;
    public int Level = 0;
    public uint UnlockQuestID = 0;
    public uint TerritoryID = 0;
    public List<Section>? Sections;
    public class Section
    {
        public int Type = (int)DutySectionType.Boss;
        public string Name = "???";
        public List<Phase>? Phases;
        public class Phase
        {
            public string Strategy = TStrings.TypeDutySectionStrategyNone;
            public string? TLDR;
            public List<KeyMechanic>? Mechanics;
            public class KeyMechanic
            {
                public string Name { get; set; } = "???";
                public string LongDesc { get; set; } = "???";
                public string ShortDesc { get; set; } = "???";
                public string? Images { get; set; }
                public int Type { get; set; } = (int)DutyMechanics.Other;
            }
        }
    }

    /// <summary>
    ///     Boolean value indicating if this duty is not supported on the current plugin version.
    /// </summary>
    public bool IsSupported()
    {
        // Check a bunch of things to make sure the duty is supported, like enum values & format version.
        if (this.Version != _formatVersion) return false;
        if (!Enum.IsDefined(typeof(DutyExpansion), this.Expansion)) return false;
        if (!Enum.IsDefined(typeof(DutyType), this.Type)) return false;
        if (!Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)) return false;
        if (this.Sections?.Any(s => s.Phases?.Any(p => p.Mechanics?.Any(m => !Enum.IsDefined(typeof(DutyMechanics), m.Type)) == true) == true) == true) return false;

        // If nothing else returns false, then the duty is supported.
        return true;
    }


    /// <summary>
    ///     Get the canonical name for the duty
    /// </summary>
    public string GetCanonicalName()
    {
        if (this.Difficulty != (int)DutyDifficulty.Normal) return $"{this.Name} ({Enum.GetName(typeof(DutyDifficulty), this.Difficulty)})";
        else return this.Name;
    }
}

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

enum DutySectionType
{
    Boss = 0,
    Trashpack = 1,
    Other = 2
}

enum DutyDisplayType
{
    Display = 0,
    Hide = 1,
    Unavailable = 2
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