using System;
using System.Globalization;
using System.Threading.Tasks;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using KikoGuide.Common;
using KikoGuide.DataModels;
using KikoGuide.Enums;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;
using Sirensong.Game.Extensions;
using Sirensong.Game.UI;
using Sirensong.Game.Utility;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    /// <summary>
    /// Represents a guide for duty/instance content.
    /// </summary>
    internal abstract class InstanceContentGuideBase : GuideBase
    {
        public InstanceContentGuideBase()
        {
            this.LinkedDuty = Duty.GetDuty(this.DutyId);
            this.UnlockQuest = Services.Data.GetExcelSheet<Quest>()?.GetRow(this.UnlockQuestId)!;

            // Throw if invalid data
            if (this.UnlockQuest == null || this.LinkedDuty == null)
            {
                throw new ArgumentException("Invalid duty or unlock quest ID.");
            }

            // Assign to properties
            this.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.LinkedDuty.CFCondition.Name.ToDalamudString().ToString());
            this.Description = this.LinkedDuty.CFConditionTransient.Description.ToDalamudString().ToString();
            this.ContentType = (ContentTypeModified?)this.LinkedDuty.CFCondition.GetContentType(true) ?? ContentTypeModified.Custom;
            this.Difficulty = this.LinkedDuty.CFCondition.GetContentDifficulty();
            this.Icon = this.LinkedDuty.CFCondition.ContentType.Value?.Icon ?? 21;
            this.Note = Note.CreateOrLoad(@$"{this.ContentType}_{this.Name}");

            // Sanity checks for invalid or missing data (can happen with some instance content for some reason)
            if (string.IsNullOrEmpty(this.Name))
            {
                this.Name = "An unnamed instance";
            }
            if (this.Icon == 0)
            {
                this.Icon = 21;
            }

            // Listen for territory changes
            Services.ClientState.TerritoryChanged += this.HandleTerritoryChange;
        }

        public override void Dispose() => Services.ClientState.TerritoryChanged -= this.HandleTerritoryChange;

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
        public abstract InstanceContentGuideFormat Content { get; }

        /// <inheritdoc/>
        protected override void DrawAction() => InstanceContentGuideLayout.Draw(this);

        /// <summary>
        /// Handles territory changes and updates the guide if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="territoryId"></param>
        private unsafe void HandleTerritoryChange(object? sender, ushort territoryId)
        {
            // TODO: Find a better way of hiding guides when no guide is found for the current territory.
            Services.GuideManager.ClearSelectedGuide();
            if (this.LinkedDuty.CFCondition.TerritoryType.Row != territoryId || Services.GuideManager.SelectedGuide == this)
            {
                return;
            }

            // run a task with a 1 second total allowed delay
            Task.Run(() =>
            {
                var retries = 3;
                while (EventFramework.Instance()->GetInstanceContentDirector() == null)
                {
                    if (retries-- == 0)
                    {
                        BetterLog.Warning("Failed to get instance content director.");
                        return;
                    }
                    Task.Delay(400).Wait();
                }

                // Handle content flags.
                switch (InstanceDirectorUtil.GetInstanceContentFlag())
                {
                    case ContentFlag.ExplorerMode:
                        BetterLog.Debug("Not loading guide, player is using explorer mode.");
                        return;
                    default:
                        break;
                }

                switch (this.AutoOpen)
                {
                    case true:
                        Services.GuideManager.SetSelectedGuide(this, true);
                        break;
                    case false:
                        GameChat.Print($"A guide for the duty {this.Name} is available. Use {Constants.Commands.GuideViewer} to open it.");
                        Services.GuideManager.SetSelectedGuide(this, false);
                        break;
                    default:
                }
            });
        }
    }
}