using System;
using System.Text.Json.Serialization;
using KikoGuide.Common;
using KikoGuide.DataStructures;
using Lumina.Excel.GeneratedSheets;

#pragma warning disable IDE0051, IDE0044
namespace KikoGuide.DataModels
{
    /// <summary>
    ///     Represents an internal KikoGuide' guide.
    /// </summary>
    [Serializable]
    public sealed record class Guide
    {
        internal Guide() { }

        // Guide metadata
        [JsonIgnore]
        public const int FormatVersion = 0;
        public int Version { get; init; }
        public bool NoLoad { get; init; }
        public bool Hidden { get; init; }
        public TranslatableString GuideName { get; init; }

        // Linked duty
        public uint LinkedDutyID { get; init; }
        [JsonIgnore]
        private Duty? linkedDuty;
        [JsonIgnore]
        public Duty? LinkedDuty
            => this.linkedDuty ??= this.LinkedDutyID == 0 ? null : new Duty(this.LinkedDutyID);

        // Linked quest
        public uint UnlockQuestID { get; init; }
        [JsonIgnore]
        private Quest? unlockQuest;
        [JsonIgnore]
        public Quest? UnlockQuest
            => this.unlockQuest ??= this.UnlockQuestID == 0 ? null : Services.Data.GetExcelSheet<Quest>()?.GetRow(this.UnlockQuestID);

        // Linked note
        [JsonIgnore]
        private Note? note;
        [JsonIgnore]
        public Note Note
           => this.note ??= Note.CreateOrLoad(this.GuideName.EN);

        // Guide content
        public GuideContent? Content { get; init; }

        // Subclasses
        public sealed class GuideContent
        {
            // Section content
            public ContentSection[]? Sections { get; init; }

            // Subclasses
            public sealed class ContentSection
            {
                //  Content section content
                public TranslatableString Title { get; init; }
                public SubSection[]? SubSections { get; init; }

                // Subclasses
                public sealed class SubSection
                {
                    // Subsection content
                    public TranslatableString Content { get; init; }
                    public Mechanic[]? Mechanics { get; init; }
                    public Tip[]? Tips { get; init; }

                    // Subclasses
                    public sealed class Mechanic
                    {
                        // Mechanic content
                        public TranslatableString Name { get; init; }
                        public TranslatableString Description { get; init; }
                        public bool TypePlaceholder { get; init; }
                    }

                    public sealed class Tip
                    {
                        // Tip content
                        public TranslatableString Content { get; init; }
                    }
                }
            }
        }
    }
}