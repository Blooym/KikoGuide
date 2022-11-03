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
    ///     The current format version, incremented on breaking changes.
    ///     When this version does not match a duty, it cannot be loaded.
    /// </summary>
    private const int _formatVersion = 1;

    /// <summary>
    ///     The current duty version.
    /// </summary>
    public int Version;

    /// <summary>
    ///     The duty name.
    /// </summary>
    public string Name = TStrings.TypeDutyUnnamed;

    /// <summary>
    ///     The duty difficulty level.
    /// </summary>
    public int Difficulty = (int)DutyDifficulty.Normal;

    /// <summary>
    ///     The expansion the duty is from.
    /// </summary>
    public int Expansion = (int)DutyExpansion.ARealmReborn;

    /// <summary>
    ///     The duty type.
    /// </summary>
    public int Type = (int)DutyType.Dungeon;

    /// <summary>
    ///     The duty level.
    /// </summary>
    public int Level = 0;

    /// <summary>
    ///     The duty's unlock quest ID.
    /// </summary>
    public uint UnlockQuestID = 0;

    /// <summary>
    ///     The duty's TerritoryID(s).
    /// </summary>
    public List<uint> TerritoryIDs = new List<uint>();

    /// <summary>
    ///     The duty's section data.
    /// </summary>
    public List<Section>? Sections;

    /// <summary>
    ///     Represents a section of a duty.
    /// </summary>
    public class Section
    {
        /// <summary>
        ///     The type of section.
        /// </summary>
        public int Type = (int)DutySectionType.Boss;

        /// <summary>
        ///     The section's name.
        /// </summary>
        public string Name = "???";

        /// <summary>
        ///     The phases that belong to this section.
        /// </summary>
        public List<Phase>? Phases;

        /// <summary>
        ///     Represents a phase of a duty.
        /// </summary>
        public class Phase
        {
            /// <summary>
            ///     The title of the phase.
            /// </summary>
            public string? Title;

            /// <summary>
            ///     The strategy for the phase.
            /// </summary>
            public string Strategy = TStrings.TypeDutySectionStrategyNone;

            /// <summary>
            ///     The short strategy for the phase.
            /// </summary>
            public string? StrategyShort;

            /// <summary>
            ///     The phase's associated mechanics.
            /// </summary>
            public List<Mechanic>? Mechanics;

            /// <summary>
            ///     Represents a mechanic of a duty.
            /// </summary>
            public class Mechanic
            {
                /// <summary>
                ///     The mechanic's name.
                /// </summary>
                public string Name { get; set; } = "???";

                /// <summary>
                ///     The mechanic's description.
                /// </summary>
                public string Description { get; set; } = "???";

                /// <summary>
                ///     The mechanic's short description.
                /// </summary>
                public string? ShortDesc { get; set; }

                /// <summary>
                ///     The type of mechanic.
                /// </summary>
                public int Type { get; set; } = (int)DutyMechanics.Other;
            }
        }
    }

    /// <summary>
    ///     Boolean value indicating if this duty is not supported on the current plugin version.
    ///     Checks multiple things, such as the format version, invalid enums, etc.
    /// </summary>
    public bool IsSupported()
    {
        if (this.Version != _formatVersion) return false;
        if (!Enum.IsDefined(typeof(DutyExpansion), this.Expansion)) return false;
        if (!Enum.IsDefined(typeof(DutyType), this.Type)) return false;
        if (!Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)) return false;
        if (this.Sections?.Any(s => s.Phases?.Any(p => p.Mechanics?.Any(m => !Enum.IsDefined(typeof(DutyMechanics), m.Type)) == true) == true) == true) return false;

        return true;
    }

    /// <summary>
    ///     Gets all the given sections for this duty.
    /// </summary>
    public IEnumerable<Section> GetFilteredSections(DutySectionType filter) => this.Sections?.Where(s => s.Type == (int)filter) ?? Enumerable.Empty<Section>();

    /// <summary>
    ///     Get the canonical name for the duty
    /// </summary>
    public string GetCanonicalName()
    {
        if (this.Difficulty != (int)DutyDifficulty.Normal) return $"{this.Name} ({Enum.GetName(typeof(DutyDifficulty), this.Difficulty)})";
        else return this.Name;
    }
}

public enum DutyType
{
    Dungeon = 0,
    Trial = 1,
    AllianceRaid = 2
}

public enum DutyDifficulty
{
    Normal = 0,
    Hard = 1,
    Extreme = 2,
    Savage = 3,
    Ultimate = 4,
    Unreal = 5
}

public enum DutyExpansion
{
    ARealmReborn = 0,
    Heavensward = 1,
    Stormblood = 2,
    Shadowbringers = 3,
    Endwalker = 4
}

public enum DutySectionType
{
    Boss = 0,
    Trashpack = 1,
    Other = 2
}

public enum DutyDisplayType
{
    Display = 0,
    Hide = 1,
    Unavailable = 2
}

public enum DutyMechanics
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
