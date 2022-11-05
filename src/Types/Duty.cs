using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game;
using KikoGuide.Attributes;
using KikoGuide.Localization;
using Newtonsoft.Json;

namespace KikoGuide.Types
{
    /// <summary>
    ///     Represents an in-game duty.
    /// </summary>
    public class Duty
    {
        /// <summary>
        ///     The current format version, incremented on breaking changes.
        ///     When this version does not match a duty, it cannot be loaded.
        /// </summary>
        [JsonIgnore]
        private const int _formatVersion = 1;

        /// <summary>
        ///     The current duty version.
        /// </summary>
        public int Version = 1;

        /// <summary>
        ///     The duty name.
        /// </summary>
        public string Name = TStrings.TypeDutyUnnamed;

        /// <summary>
        ///     The duty difficulty level.
        /// </summary>
        public DutyDifficulty Difficulty = (int)DutyDifficulty.Normal;

        /// <summary>
        ///     The expansion the duty is from.
        /// </summary>
        public DutyExpansion Expansion = (int)DutyExpansion.ARealmReborn;

        /// <summary>
        ///     The duty type.
        /// </summary>
        public DutyType Type = (int)DutyType.Dungeon;

        /// <summary>
        ///     The duty level.
        /// </summary>
        public int Level;

        /// <summary>
        ///     The duty's unlock quest ID.
        /// </summary>
        public uint UnlockQuestID;

        /// <summary>
        ///     The duty's TerritoryID(s).
        /// </summary>
        public List<uint> TerritoryIDs = new();

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
            public DutySectionType Type = (int)DutySectionType.Boss;

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
                ///     The overriden title of the phase, usually left blank.
                /// </summary>
                public string? TitleOverride;

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
            if (Version != _formatVersion)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(DutyExpansion), Expansion))
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(DutyType), Type))
            {
                return false;
            }

#pragma warning disable IDE0075 // Simplify conditional expression
            return !Enum.IsDefined(typeof(DutyDifficulty), Difficulty)
                ? false
                : Sections?.Any(s => !Enum.IsDefined(typeof(DutySectionType), s.Type) || s.Phases?.Any(p => p.Mechanics?.Any(m => !Enum.IsDefined(typeof(DutyMechanics), m.Type)) == true) == true) != true;
#pragma warning restore IDE0075 // Simplify conditional expression
        }

        /// <summary>
        ///     Get the canonical name for the duty
        /// </summary>
        public string GetCanonicalName()
        {
            return !Enum.IsDefined(typeof(DutyDifficulty), Difficulty)
                ? Name
                : Difficulty != (int)DutyDifficulty.Normal
                ? $"{Name} ({AttributeExtensions.GetNameAttribute(Difficulty)})"
                : Name;
        }

        /// <summary>
        ///     Get if the player has unlocked this duty.
        /// </summary>
        public bool IsUnlocked()
        {
            return (UnlockQuestID != 0 && QuestManager.IsQuestCurrent(UnlockQuestID)) || QuestManager.IsQuestComplete(UnlockQuestID);
        }
    }


    public enum DutyType
    {
        [Name("Dungeon")]
        Dungeon = 0,

        [Name("Trial")]
        Trial = 1,

        [Name("Alliance Raid")]
        AllianceRaid = 2,

        [Name("Raid")]
        NormalRaid = 3,
    }

    public enum DutyDifficulty
    {
        [Name("Normal")]
        Normal = 0,

        [Name("Hard")]
        Hard = 1,

        [Name("Extreme")]
        Extreme = 2,

        [Name("Savage")]
        Savage = 3,

        [Name("Ultimate")]
        Ultimate = 4,

        [Name("Unreal")]
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
        [Name("Boss")]
        [Description("A boss fight.")]
        Boss = 0,

        [Name("Trashpack")]
        [Description("A group of enemies that are not a boss.")]
        Trashpack = 1,

        [Name("Other")]
        [Description("A section that does not fit into the other categories.")]
        Other = 2
    }

    public enum DutyMechanics
    {
        [Name("Tankbuster")]
        [Description("A mechanic that requires a tank to take the hit.")]
        Tankbuster = 0,

        [Name("Enrage")]
        [Description("A mechanic that causes the boss to go into an enraged state, dealing more damage.")]
        Enrage = 1,

        [Name("AoE")]
        [Description("A mechanic that requires the party to spread out.")]
        AOE = 2,

        [Name("Stackmarker")]
        [Description("A mechanic that requires the party to stack up.")]
        Stackmarker = 3,

        [Name("Raidwide")]
        [Description("A mechanic that affects the entire raid.")]
        Raidwide = 4,

        [Name("Invulnerability")]
        [Description("A mechanic that makes the boss invulnerable.")]
        Invulnerablity = 5,

        [Name("Targetted")]
        [Description("A mechanic that targets a specific player.")]
        Targetted = 6,

        [Name("Addspawn")]
        [Description("A mechanic that spawns additional enemies.")]
        AddSpawn = 7,

        [Name("DPS Check")]
        [Description("A mechanic that requires the party to deal a certain amount of damage.")]
        DPSCheck = 8,

        [Name("Cleave")]
        [Description("A mechanic that deals damage in a non-telegraphed cone.")]
        Cleave = 9,

        [Name("Other")]
        [Description("A mechanic that does not fit into any other category.")]
        Other = 10,
    }
}