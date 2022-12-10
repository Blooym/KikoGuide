using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game;
using KikoGuide.Attributes;
using KikoGuide.Localization;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;

namespace KikoGuide.Types
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    /// <summary>
    ///     Represents a guide for an in-game duty.
    /// </summary>
    public class Guide
    {
        /// <summary>
        ///     The current format version, incremented on breaking changes.
        ///     When this version does not match a guide, it cannot be loaded.
        /// </summary>
        [JsonIgnore]
        private const int FormatVersion = 1;

        /// <summary>
        ///     The current guide version.
        /// </summary>
        public int Version = 1;

        /// <summary>
        ///     The guide's internal name, should be unique and not change after creation.
        /// </summary>
        public string InternalName;

        /// <summary>
        ///     The constructor for a guide.
        /// </summary>
        public Guide(string internalName)
        {
            this.InternalName = internalName;

            if (this.InternalName == null)
            {
                throw new ArgumentNullException(nameof(internalName));
            }
        }

        /// <summary>
        ///     The duty/guide name.
        /// </summary>
        public string Name = "???";

        /// <summary>
        ///     Whether or not this duty is disabled and shouldn't be loaded.
        /// </summary>
        public bool Disabled;

        /// <summary>
        ///     Whether or not the guide is hidden from all forms of listing. Will still be accessible via auto-open.
        /// </summary>
        public bool Hidden;

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
        ///     The duty's TerritoryIDs(s).
        /// </summary>
        public uint[] TerritoryIDs = Array.Empty<uint>();

        /// <summary>
        ///     The lore for the duty.
        /// </summary>
        public string? Lore;

        /// <summary>
        ///     The guide's authors.
        /// </summary>
        public string[]? Authors = Array.Empty<string>();

        /// <summary>
        ///     The guide section data.
        /// </summary>
        public List<Section>? Sections;

        /// <summary>
        ///     Represents a section of a guide.
        /// </summary>
        public class Section
        {
            /// <summary>
            ///     The type of section.
            /// </summary>
            public GuideSectionType Type = (int)GuideSectionType.Boss;

            /// <summary>
            ///     The section's name.
            /// </summary>
            public string Name = "???";

            /// <summary>
            ///     The phases that belong to this section.
            /// </summary>
            public List<Phase>? Phases;

            /// <summary>
            ///     Represents a phase of a section.
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
                public string? Strategy;

                /// <summary>
                ///     The short strategy for the phase.
                /// </summary>
                public string? StrategyShort;

                /// <summary>
                ///     The phase's associated mechanics.
                /// </summary>
                public List<Mechanic>? Mechanics;

                /// <summary>
                ///     The phase's associated tips.
                /// </summary>
                public List<Tip>? Tips;

                /// <summary>
                ///     Represents a mechanic of a phase.
                /// </summary>
                public class Mechanic
                {
                    /// <summary>
                    ///     The mechanic's name.
                    /// </summary>
                    public string Name { get; set; } = TGenerics.Unknown;

                    /// <summary>
                    ///     The mechanic's description.
                    /// </summary>
                    public string Description { get; set; } = TGenerics.Unspecified;

                    /// <summary>
                    ///     The mechanic's short description.
                    /// </summary>
                    public string? ShortDescription { get; set; }

                    /// <summary>
                    ///     The type of mechanic.
                    /// </summary>
                    public int Type { get; set; } = (int)GuideMechanics.Other;
                }

                /// <summary>
                ///     Represents a tip of a phase.
                /// </summary>
                public class Tip
                {
                    public string? Text { get; set; }
                    public string? TextShort { get; set; }
                }
            }
        }
#pragma warning restore CA1051 // Do not declare visible instance fields

        /// <summary>
        ///     Boolean value indicating if this duty is not supported on the current plugin version.
        ///     Checks multiple things, such as the format version, invalid enums, etc.
        /// </summary>
        public bool IsSupported()
        {
            if (this.Version != FormatVersion)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(DutyExpansion), this.Expansion))
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(DutyType), this.Type))
            {
                return false;
            }

#pragma warning disable IDE0075 // Simplify conditional expression
            return Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)
                && this.Sections?.Any(s => !Enum.IsDefined(typeof(GuideSectionType), s.Type) || s.Phases?.Any(p => p.Mechanics?.Any(m => !Enum.IsDefined(typeof(GuideMechanics), m.Type)) == true) == true) != true;
#pragma warning restore IDE0075 // Simplify conditional expression
        }

        /// <summary>
        ///     Get the canonical name for the duty/guide.
        /// </summary>
        public string GetCanonicalName()
        {
            {
                if (!Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty))
                {
                    return this.Name;
                }
                else if (this.Difficulty != (int)DutyDifficulty.Normal)
                {
                    return $"{this.Name} ({this.Difficulty.GetNameAttribute()})";
                }
                else
                {
                    return this.Name;
                }
            }
        }

        /// <summary>
        ///     Get if the player has unlocked this duty/guide.
        /// </summary>
        public bool IsUnlocked() => (this.UnlockQuestID != 0 && QuestManager.IsQuestCurrent(this.UnlockQuestID)) || QuestManager.IsQuestComplete(this.UnlockQuestID);

        /// <summary>
        ///     Whether or not this guide is hidden and should not be listed.
        /// </summary>
        public bool IsHidden() => this.Hidden || this.Disabled;
    }

    public enum DutyType
    {
        [Name("Dungeon"), PluralName("Dungeons")]
        Dungeon = 0,

        [Name("Trial"), PluralName("Trials")]
        Trial = 1,

        [Name("Alliance Raid"), PluralName("Alliance Raids")]
        AllianceRaid = 2,

        [Name("Raid"), PluralName("Raids")]
        Raid = 3,

        [Name("FATE"), PluralName("FATEs")]
        FATE = 4,

        [Name("Misc"), PluralName("Misc")]
        Misc = 5,
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

    public enum GuideSectionType
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

    public enum GuideMechanics
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

        [Name("Add Spawn")]
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
