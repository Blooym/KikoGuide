using System;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using KikoGuide.Common;
using KikoGuide.DataModels;
using Lumina.Excel.GeneratedSheets;
using Sirensong.DataStructures;

namespace KikoGuide.Guides
{
    /// <summary>
    ///     Represents a guide
    /// </summary>
    public abstract class Guide : IDisposable
    {
        // Constructors

        /// <summary>
        ///     Creates a new guide.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the unlock quest or linked duty is null.</exception>
        public Guide()
        {
            this.Duty = new(this.DutyId);
            this.UnlockQuest = Services.Data.GetExcelSheet<Quest>()?.GetRow(this.UnlockQuestId)!;
            this.Note = Note.CreateOrLoad(this.Name);

            if (this.UnlockQuest == null || this.Duty == null)
            {
                throw new ArgumentException(Constants.ExceptionMessages.InvalidDutyOrUnlockQuest);
            }
        }


        // Properties & Fields

        /// <summary>
        ///     The cached normalized name of the guide, prevents converting SeString -> string every time.
        /// </summary>
        private string? nameCached;

        /// <summary>
        ///     Whether or not this guide should be loaded by the <see cref="GuideManager"/>.
        /// </summary>
        public static bool NoLoad { get; }

        /// <summary>
        ///     Whether or not this guide should be hidden from any UI.
        /// </summary>
        public virtual bool Hide { get; }

        /// <summary>
        ///     The note associated with this guide.
        /// </summary>
        public Note Note { get; }

        /// <summary>
        ///     The duty this guide is linked to.
        /// </summary>
        public Duty Duty { get; }

        /// <summary>
        ///     The duty ID of the linked duty.
        /// </summary>
        public abstract uint DutyId { get; }

        /// <summary>
        ///     The difficulty of the linked duty.
        /// </summary>
        public abstract DutyDifficulty Difficulty { get; }

        /// <summary>
        ///     The quest that unlocks this guide.
        /// </summary>
        public Quest UnlockQuest { get; }

        /// <summary>
        ///     The quest ID of the unlock quest.
        /// </summary>
        public abstract uint UnlockQuestId { get; }

        /// <summary>
        ///     The content of this guide.
        /// </summary>
        public abstract GuideContent Content { get; protected set; }


        // Methods

        /// <summary>
        ///     The name of the guide, usually the name of the linked duty.
        /// </summary>
        public virtual string Name => this.nameCached ??= this.Duty.CFCondition.Name.ToDalamudString().ToString();

        /// <summary>
        ///     Whether or not the player has unlocked this guide.
        /// </summary>
        public virtual unsafe bool IsUnlocked => QuestManager.IsQuestComplete(this.UnlockQuestId) || QuestManager.Instance()->IsQuestAccepted(this.UnlockQuestId);

        /// <summary>
        ///     Whether or not the player is currently in the duty this guide is linked to.
        /// </summary>
        public virtual unsafe bool IsInDuty => Services.ClientState.TerritoryType == this.Duty.CFCondition.TerritoryType.Value?.RowId;

        /// <summary>
        ///     Disposes of the guide.
        /// </summary>
        public virtual void Dispose() => GC.SuppressFinalize(this);


        // Sub-records & enums

        /// <summary>
        ///     Represents the content of a guide.
        /// </summary>
        public sealed record GuideContent
        {
            public ContentSection[]? Sections { get; init; }

            /// <summary>
            ///     Represents a content section.
            /// </summary>
            public sealed record ContentSection
            {
                public TranslatableString Title { get; init; }
                public SubSection[]? SubSections { get; init; }

                /// <summary>
                ///     Represents a subsection.
                /// </summary>
                public sealed record SubSection
                {
                    public TranslatableString Content { get; init; }
                    public Mechanic[]? Mechanics { get; init; }
                    public Tip[]? Tips { get; init; }

                    /// <summary>
                    ///     Represents a game mechanic.
                    /// </summary>
                    public sealed record Mechanic
                    {
                        // Mechanic content
                        public TranslatableString Name { get; init; }
                        public TranslatableString Description { get; init; }
                        public string? Tooltip { get; init; }
                    }

                    /// <summary>
                    ///     Represents a tip.
                    /// </summary>
                    public sealed record Tip
                    {
                        public TranslatableString Content { get; init; }
                    }
                }
            }
        }

        /// <summary>
        ///     Represents the difficulty of a duty.
        /// </summary>
        public enum DutyDifficulty
        {
            Normal,
            Hard,
            Extreme,
            Savage,
            Ultimate,
            Unreal
        }
    }
}

