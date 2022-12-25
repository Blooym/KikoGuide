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
    /// <summary>
    ///     Represents a guide for an in-game duty.
    /// </summary>
    public class Guide
    {
        private Guide() { }

        /// <summary>
        ///     The current format version, incremented on breaking changes.
        ///     When this version does not match a guide, it cannot be loaded.
        /// </summary>
        [JsonIgnore]
        private const int FormatVersion = 1;

        /// <summary>
        ///     The current guide version.
        /// </summary>
        [JsonProperty]
        public int Version { get; private set; }

        /// <summary>
        ///     The guide's internal name, should be unique and not change after creation.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InternalName { get; private set; } = TGenerics.Unknown;

        /// <summary>
        ///     The duty/guide name.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; private set; } = TGenerics.Unknown;

        /// <summary>
        ///     Whether or not this duty is disabled and shouldn't be loaded.
        /// </summary>
        [JsonProperty]
        public bool Disabled { get; private set; }

        /// <summary>
        ///     Whether or not the guide is hidden from all forms of listing. Will still be accessible via auto-open.
        /// </summary>
        [JsonProperty]
        public bool Hidden { get; private set; }

        /// <summary>
        ///     The duty difficulty level.
        /// </summary>
        [JsonProperty]
        public DutyDifficulty Difficulty { get; private set; } = (int)DutyDifficulty.Normal;

        /// <summary>
        ///     The expansion the duty is from.
        /// </summary>
        [JsonProperty]
        public DutyExpansion Expansion { get; private set; } = (int)DutyExpansion.ARealmReborn;

        /// <summary>
        ///     The duty type.
        /// </summary>
        [JsonProperty]
        public DutyType Type { get; private set; } = (int)DutyType.Dungeon;

        /// <summary>
        ///     The duty level.
        /// </summary>
        [JsonProperty]
        public int Level { get; private set; }

        /// <summary>
        ///     The duty's unlock quest ID.
        /// </summary>
        [JsonProperty]
        public uint UnlockQuestID { get; private set; }

        /// <summary>
        ///     The duty's TerritoryIDs(s).
        /// </summary>
        [JsonProperty]
        public uint[] TerritoryIDs { get; private set; } = Array.Empty<uint>();

        /// <summary>
        ///     The lore for the duty.
        /// </summary>
        [JsonProperty]
        public string? Lore { get; private set; }

        /// <summary>
        ///     The guide's authors.
        /// </summary>
        [JsonProperty]
        public string[]? Authors { get; private set; } = Array.Empty<string>();

        /// <summary>
        ///     The guide section data.
        /// </summary>
        [JsonProperty]
        public List<Section>? Sections { get; private set; }

        /// <summary>
        ///     Represents a section of a guide.
        /// </summary>
        public class Section
        {
            /// <summary>
            ///     The type of section.
            /// </summary>
            [JsonProperty]
            public GuideSectionType Type { get; private set; } = (int)GuideSectionType.Boss;

            /// <summary>
            ///     The section's name.
            /// </summary>
            [JsonProperty]
            public string Name { get; private set; } = TGenerics.Unknown;

            /// <summary>
            ///     The phases that belong to this section.
            /// </summary>
            [JsonProperty]
            public List<Phase>? Phases { get; private set; }

            /// <summary>
            ///     Represents a phase of a section.
            /// </summary>
            public class Phase
            {
                /// <summary>
                ///     The overriden title of the phase, usually left blank.
                /// </summary>
                [JsonProperty]
                public string? TitleOverride { get; private set; }

                /// <summary>
                ///     The strategy for the phase.
                /// </summary>
                [JsonProperty]
                public string? Strategy { get; private set; }

                /// <summary>
                ///     The short strategy for the phase.
                /// </summary>
                [JsonProperty]
                public string? StrategyShort { get; private set; }

                /// <summary>
                ///     The phase's associated mechanics.
                /// </summary>
                [JsonProperty]
                public List<Mechanic>? Mechanics { get; private set; }

                /// <summary>
                ///     The phase's associated tips.
                /// </summary>
                [JsonProperty]
                public List<Tip>? Tips { get; private set; }

                /// <summary>
                ///     Represents a mechanic of a phase.
                /// </summary>
                public class Mechanic
                {
                    /// <summary>
                    ///     The mechanic's name.
                    /// </summary>
                    [JsonProperty]
                    public string Name { get; private set; } = TGenerics.Unknown;

                    /// <summary>
                    ///     The mechanic's description.
                    /// </summary>
                    [JsonProperty]
                    public string Description { get; private set; } = TGenerics.Unspecified;

                    /// <summary>
                    ///     The mechanic's short description.
                    /// </summary>
                    [JsonProperty]
                    public string? ShortDescription { get; private set; }

                    /// <summary>
                    ///     The type of mechanic.
                    /// </summary>
                    [JsonProperty]
                    public int Type { get; private set; } = (int)GuideMechanics.Other;
                }

                /// <summary>
                ///     Represents a tip of a phase.
                /// </summary>
                public class Tip
                {
                    /// <summary>
                    ///     The text of the tip.
                    /// </summary>
                    [JsonProperty]
                    public string? Text { get; private set; }

                    /// <summary>
                    ///     The short text of the tip.
                    /// </summary>
                    [JsonProperty]
                    public string? TextShort { get; private set; }
                }
            }
        }

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

            return Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)
                && this.Sections?.Any(s => !Enum.IsDefined(typeof(GuideSectionType), s.Type) || s.Phases?.Any(p => p.Mechanics?.Any(m => !Enum.IsDefined(typeof(GuideMechanics), m.Type)) == true) == true) != true;
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

        /// <summary>
        ///    Creates a new Guide object from a JSON input.
        /// </summary>
        /// <param name="json">The JSON input.</param>
        /// <returns>The Guide object.</returns>
        public static Guide FromJson(string json) => JsonConvert.DeserializeObject<Guide>(json) ?? throw new JsonException("Failed to deserialize guide JSON.");
    }

    public enum DutyType
    {
        [Name("Dungeon", "Dungeons")]
        Dungeon = 0,

        [Name("Trial", "Trials")]
        Trial = 1,

        [Name("Alliance Raid", "Alliance Raids")]
        AllianceRaid = 2,

        [Name("Raid", "Raids")]
        Raid = 3,

        [Name("FATE", "FATEs")]
        FATE = 4,

        [Name("Misc")]
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
        [Name("A Realm Reborn")]
        ARealmReborn = 0,

        [Name("Heavensward")]
        Heavensward = 1,

        [Name("Stormblood")]
        Stormblood = 2,

        [Name("Shadowbringers")]
        Shadowbringers = 3,

        [Name("Endwalker")]
        Endwalker = 4
    }

    public enum GuideSectionType
    {
        [Name("Boss", "Bosses")]
        [Description("A boss fight.")]
        Boss = 0,

        [Name("Trashpack", "Trashpacks")]
        [Description("A group of enemies that are not a boss.")]
        Trashpack = 1,

        [Name("Other", "Others")]
        [Description("A section that does not fit into the other categories.")]
        Other = 2
    }

    public enum GuideMechanics
    {
        [Name("Tankbuster", "Tankbusters")]
        [Description("A mechanic that requires a tank to take the hit.")]
        Tankbuster = 0,

        [Name("Enrage", "Enrages")]
        [Description("A mechanic that causes the boss to go into an enraged state, dealing more damage.")]
        Enrage = 1,

        [Name("AoE", "AoEs")]
        [Description("A mechanic that requires the party to spread out.")]
        AOE = 2,

        [Name("Stackmarker", "Stackmarkers")]
        [Description("A mechanic that requires the party to stack up.")]
        Stackmarker = 3,

        [Name("Raidwide", "Raidwides")]
        [Description("A mechanic that affects the entire raid.")]
        Raidwide = 4,

        [Name("Invulnerability", "Invulnerabilities")]
        [Description("A mechanic that makes the boss invulnerable.")]
        Invulnerablity = 5,

        [Name("Targetted", "Targetted")]
        [Description("A mechanic that targets a specific player.")]
        Targetted = 6,

        [Name("Add Spawn", "Add Spawns")]
        [Description("A mechanic that spawns additional enemies.")]
        AddSpawn = 7,

        [Name("DPS Check", "DPS Checks")]
        [Description("A mechanic that requires the party to deal a certain amount of damage.")]
        DPSCheck = 8,

        [Name("Cleave", "Cleaves")]
        [Description("A mechanic that deals damage in a non-telegraphed cone.")]
        Cleave = 9,

        [Name("Other", "Others")]
        [Description("A mechanic that does not fit into any other category.")]
        Other = 10,
    }
}
