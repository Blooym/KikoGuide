using System;
using System.Globalization;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using KikoGuide.Common;
using KikoGuide.DataModels;
using KikoGuide.Enums;
using Lumina.Excel.GeneratedSheets;
using Sirensong.DataStructures;
using Sirensong.Game.Enums;
using Sirensong.Game.Extensions;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    /// <summary>
    /// Represents a guide for duty/instance content.
    /// </summary>
    internal abstract class InstanceContentGuide : GuideBase
    {
        public InstanceContentGuide()
        {
            this.LinkedDuty = Duty.GetDuty(this.DutyId);
            this.UnlockQuest = Services.Data.GetExcelSheet<Quest>()?.GetRow(this.UnlockQuestId)!;

            if (this.UnlockQuest == null || this.LinkedDuty == null)
            {
                throw new ArgumentException("Invalid duty or unlock quest ID.");
            }

            this.Icon = this.LinkedDuty.CFCondition.Icon;
            this.ContentType = (ContentTypeModified?)this.LinkedDuty.CFCondition.GetContentType() ?? throw new ArgumentException("Cannot determine content type.");
            this.Difficulty = this.LinkedDuty.CFCondition.GetContentDifficulty();
            this.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.LinkedDuty.CFCondition.Name.ToDalamudString().ToString());
            this.Description = this.LinkedDuty.CFConditionTransient.Description.ToDalamudString().ToString();
            this.Note = Note.CreateOrLoad(this.Name);
        }

        public override void Dispose()
        {

        }

        /// <summary>
        /// The duty associated with this guide.
        /// </summary>
        public Duty LinkedDuty { get; }

        /// <summary>
        /// The quest that unlocks this guide.
        /// </summary>
        public Quest UnlockQuest { get; }

        /// <inheritdoc/>
        public override string Name { get; }

        /// <inheritdoc/>
        public override string Description { get; }

        /// <inheritdoc/>
        public override uint Icon { get; }

        /// <summary>
        /// The note associated with this guide.
        /// </summary>
        public Note Note { get; }

        /// <inheritdoc/>
        public override unsafe bool IsUnlocked => QuestManager.IsQuestComplete(this.UnlockQuestId) || QuestManager.Instance()->IsQuestAccepted(this.UnlockQuestId);

        /// <inheritdoc/>
        public override ContentDifficulty Difficulty { get; }

        /// <inheritdoc/>
        public override ContentTypeModified ContentType { get; }

        /// <summary>
        /// The sheet row of the duty associated with this guide.
        /// </summary>
        public abstract uint DutyId { get; }

        /// <summary>
        /// The sheet row of the quest that unlocks this guide.
        /// </summary>
        public abstract uint UnlockQuestId { get; }

        /// <summary>
        /// The structured content of the guide, used for rendering.
        /// </summary>
        public abstract DutyGuideContent Content { get; }

        /// <inheritdoc/>
        public override void Draw() => InstanceContentGuideLayout.Draw(this);

        /// <summary>
        /// Represents the content of a guide.
        /// </summary>
        public sealed record DutyGuideContent
        {
            public ContentSection[]? Sections { get; init; }

            /// <summary>
            /// Represents a content section.
            /// </summary>
            public sealed record ContentSection
            {
                public TranslatableString Title { get; init; }
                public SubSection[]? SubSections { get; init; }

                /// <summary>
                /// Represents a subsection.
                /// </summary>
                public sealed record SubSection
                {
                    public TranslatableString Content { get; init; }
                    public TableRow[]? Mechanics { get; init; }
                    public Bulletpoint[]? Tips { get; init; }

                    /// <summary>
                    /// Represents a table row.
                    /// </summary>
                    public sealed record TableRow
                    {
                        // Mechanic content
                        public TranslatableString Name { get; init; }
                        public TranslatableString Description { get; init; }
                        public string? Tooltip { get; init; }
                    }

                    /// <summary>
                    /// Represents a bulletpoint.
                    /// </summary>
                    public sealed record Bulletpoint
                    {
                        public TranslatableString Content { get; init; }
                    }
                }
            }
        }
    }
}