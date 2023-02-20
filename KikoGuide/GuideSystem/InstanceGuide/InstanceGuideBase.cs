using System;
using System.Globalization;
using Dalamud;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using KikoGuide.Common;
using KikoGuide.DataModels;
using KikoGuide.Resources.Localization;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;
using Sirensong.Game.Extensions;
using ContentType = Sirensong.Game.Enums.ContentType;

namespace KikoGuide.GuideSystem.InstanceGuide
{
    /// <summary>
    ///     The base class for instance content guides.
    /// </summary>
    internal abstract class InstanceGuideBase : GuideBase
    {
        private bool disposedValue;

        /// <inheritdoc />
        public InstanceGuideBase()
        {
            // Get duty and quest data and throw if they're invalid
            this.LinkedDuty = Duty.GetDutyOrNull(this.DutyId)!;
            this.UnlockQuest = Services.QuestCache.GetRow(this.UnlockQuestId)!;
            if (this.UnlockQuest == null || this.LinkedDuty == null)
            {
                throw new InvalidOperationException("Invalid duty or quest data.");
            }

            // Assign to properties
            this.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.LinkedDuty.CFCondition.Name.ToDalamudString().ToString());
            this.Difficulty = Services.ContentFinderConditionCache.OfLanguage(ClientLanguage.English).GetRow(this.DutyId)!.GetContentDifficulty();
            this.Description = this.LinkedDuty.CFConditionTransient.Description.ToDalamudString().ToString();
            this.ContentType = this.LinkedDuty.CFCondition.GetContentType(true) ?? ContentType.Unknown;
            this.Icon = this.LinkedDuty.CFCondition.ContentType.Value?.Icon ?? 21;
            this.Note = Note.CreateOrLoad(@$"{this.ContentType}_{this.Name}");

            // Do sanity checks for some properties that can be invalid
            if (string.IsNullOrEmpty(this.Name))
            {
                this.Name = Strings.Guide_InstanceContent_UnnamedInstance;
            }
            if (this.Icon == 0)
            {
                this.Icon = 21;
            }

            // Register conductor service if it's not already registered
            Services.Container.GetOrCreateService<InstanceConductorService>();
        }

        /// <inheritdoc />
        public override InstanceGuideConfiguration Configuration { get; } = InstanceGuideConfiguration.Instance;

        /// <summary>
        ///     The duty associated with this guide.
        /// </summary>
        public Duty LinkedDuty { get; }

        /// <summary>
        ///     The quest that unlocks this guide.
        /// </summary>
        public Quest UnlockQuest { get; }

        /// <summary>
        ///     The sheet row of the duty associated with this guide.
        /// </summary>
        protected abstract uint DutyId { get; }

        /// <summary>
        ///     The sheet row of the quest that unlocks this guide.
        /// </summary>
        protected abstract uint UnlockQuestId { get; }

        /// <summary>
        ///     The structured content of the guide, used for rendering.
        /// </summary>
        public abstract InstanceGuideContent Content { get; }

        /// <summary>
        ///     The note associated with this guide.
        /// </summary>
        public override Note Note { get; }

        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        public override string Description { get; }

        /// <inheritdoc />
        public override uint Icon { get; }

        /// <inheritdoc />
        public override ContentDifficulty Difficulty { get; }

        /// <inheritdoc />
        public override ContentType ContentType { get; }

        /// <inheritdoc />
        public override unsafe bool IsUnlocked => QuestManager.IsQuestComplete(this.UnlockQuestId) || QuestManager.Instance()->IsQuestAccepted(this.UnlockQuestId);

        /// <inheritdoc />
        protected override void DrawAction() => InstanceGuideContentUI.Draw(this);

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    Services.Container.RemoveService<InstanceConductorService>();
                }

                this.disposedValue = true;

                base.Dispose(disposing);
            }
        }
    }
}
