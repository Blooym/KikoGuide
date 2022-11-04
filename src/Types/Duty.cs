namespace KikoGuide.Types
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using KikoGuide.Localization;
    using Newtonsoft.Json;
    using FFXIVClientStructs.FFXIV.Client.Game;

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
        public int Version;

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
            if (this.Version != _formatVersion) return false;
            if (!Enum.IsDefined(typeof(DutyExpansion), this.Expansion)) return false;
            if (!Enum.IsDefined(typeof(DutyType), this.Type)) return false;
            if (!Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)) return false;
            if (this.Sections?.Any(s => !Enum.IsDefined(typeof(DutySectionType), s.Type)) ?? false) return false;
            if (this.Sections?.Any(s => s.Phases?.Any(p => p.Mechanics?.Any(m => !Enum.IsDefined(typeof(DutyMechanics), m.Type)) == true) == true) == true) return false;

            return true;
        }

        /// <summary>
        ///     Get the canonical name for the duty
        /// </summary>
        public string GetCanonicalName()
        {
            if (!Enum.IsDefined(typeof(DutyDifficulty), this.Difficulty)) return this.Name;
            else if (this.Difficulty != (int)DutyDifficulty.Normal) return $"{this.Name} ({(LoCExtensions.GetLocalizedName(this.Difficulty))})";
            else return this.Name;
        }

        /// <summary>
        ///     Get if the player has unlocked this duty.
        /// </summary>
        public bool IsUnlocked() => this.UnlockQuestID != 0 && QuestManager.IsQuestCurrent(this.UnlockQuestID) || QuestManager.IsQuestComplete(this.UnlockQuestID);
    }


    public enum DutyType
    {
        [LocalizableName("TypeDutyTypeDungeon", "Dungeons")]
        Dungeon = 0,

        [LocalizableName("TypeDutyTypeTrial", "Trials")]
        Trial = 1,

        [LocalizableName("TypeDutyTypeAllianceRaid", "Alliance Raids")]
        AllianceRaid = 2,

        [LocalizableName("TypeDutyTypeRaid", "Raids")]
        Raid = 3,
    }

    public enum DutyDifficulty
    {
        [LocalizableName("TypeDutyDifficultyNormal", "Normal")]
        Normal = 0,

        [LocalizableName("TypeDutyDifficultyHard", "Hard")]
        Hard = 1,

        [LocalizableName("TypeDutyDifficultyExtreme", "Extreme")]
        Extreme = 2,

        [LocalizableName("TypeDutyDifficultySavage", "Savage")]
        Savage = 3,

        [LocalizableName("TypeDutyDifficultyUltimate", "Ultimate")]
        Ultimate = 4,

        [LocalizableName("TypeDutyDifficultyUnreal", "Unreal")]
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
        [LocalizableName("TypeDutySectionTypeBoss", "Boss")]
        [LocalizableDescription("TypeDutySectionTypeBossDesc", "A boss fight.")]
        Boss = 0,

        [LocalizableName("TypeDutySectionTypeTrashpack", "Trashpack")]
        [LocalizableDescription("TypeDutySectionTypeTrashpackDesc", "A group of enemies that are not a boss.")]
        Trashpack = 1,

        [LocalizableName("TypeDutySectionTypeOther", "Other")]
        [LocalizableDescription("TypeDutySectionTypeOtherDesc", "A section that does not fit into the other categories.")]
        Other = 2
    }

    public enum DutyMechanics
    {
        [LocalizableName("TypeDutyMechanicsTankBuster", "Tank Buster")]
        [LocalizableDescription("TypeDutyMechanicsTankBusterDesc", "A mechanic that requires a tank to take the hit.")]
        Tankbuster = 0,

        [LocalizableName("TypeDutyMechanicsEnrage", "Enrage")]
        [LocalizableDescription("TypeDutyMechanicsEnrageDesc", "A mechanic that causes the boss to go into an enraged state, dealing more damage.")]
        Enrage = 1,

        [LocalizableName("TypeDutyMechanicsAOE", "AoE")]
        [LocalizableDescription("TypeDutyMechanicsAOEDesc", "A mechanic that requires the party to spread out.")]
        AOE = 2,

        [LocalizableName("TypeDutyMechanicsStackmarker", "Stackmarker")]
        [LocalizableDescription("TypeDutyMechanicsStackmarkerDesc", "A mechanic that requires the party to stack up.")]
        Stackmarker = 3,

        [LocalizableName("TypeDutyMechanicsRaidwide", "Raidwide")]
        [LocalizableDescription("TypeDutyMechanicsRaidwideDesc", "A mechanic that affects the entire raid.")]
        Raidwide = 4,

        [LocalizableName("TypeDutyMechanicsInvulnerability", "Invulnerability")]
        [LocalizableDescription("TypeDutyMechanicsInvulnerabilityDesc", "A mechanic that makes the boss invulnerable.")]
        Invulnerablity = 5,

        [LocalizableName("TypeDutyMechanicsTargetted", "Targetted")]
        [LocalizableDescription("TypeDutyMechanicsTargettedDesc", "A mechanic that targets a specific player.")]
        Targetted = 6,

        [LocalizableName("TypeDutyMechanicsAddspawn", "Addspawn")]
        [LocalizableDescription("TypeDutyMechanicsAddspawnDesc", "A mechanic that spawns additional enemies.")]
        AddSpawn = 7,

        [LocalizableName("TypeDutyMechanicsDPSCheck", "DPS Check")]
        [LocalizableDescription("TypeDutyMechanicsDPSCheckDesc", "A mechanic that requires the party to deal a certain amount of damage.")]
        DPSCheck = 8,

        [LocalizableName("TypeDutyMechanicsCleave", "Cleave")]
        [LocalizableDescription("TypeDutyMechanicsCleaveDesc", "A mechanic that deals damage in a non-telegraphed cone.")]
        Cleave = 9,

        [LocalizableName("TypeDutyMechanicsOther", "Other")]
        [LocalizableDescription("TypeDutyMechanicsOtherDesc", "A mechanic that does not fit into any other category.")]
        Other = 10,
    }
}