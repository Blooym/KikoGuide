using System;
using System.Globalization;
using System.Threading.Tasks;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using KikoGuide.Common;
using KikoGuide.DataModels;
using KikoGuide.Resources.Localization;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;
using Sirensong.Game.Extensions;
using Sirensong.Game.State;
using Sirensong.Game.UI;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    /// <summary>
    /// The base class for instance content guides.
    /// </summary>
    internal abstract class InstanceContentGuideBase : GuideBase
    {
        private bool disposedValue;

        /// <inheritdoc/>
        public InstanceContentGuideBase()
        {
            // Get duty and quest data and throw if they're invalid
            this.LinkedDuty = Duty.GetDutyOrNull(this.DutyId)!;
            this.UnlockQuest = Services.QuestCache.GetRow(this.UnlockQuestId)!;
            if (this.UnlockQuest == null || this.LinkedDuty == null)
            {
                throw new ArgumentException("Invalid duty or unlock quest ID.");
            }

            // Assign to properties
            this.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.LinkedDuty.CFCondition.Name.ToDalamudString().ToString());
            this.Description = this.LinkedDuty.CFConditionTransient.Description.ToDalamudString().ToString();
            this.ContentType = this.LinkedDuty.CFCondition.GetContentType(true) ?? Sirensong.Game.Enums.ContentType.Unknown;
            this.Difficulty = this.LinkedDuty.CFCondition.GetContentDifficulty();
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

            // Subscribe to territory change events
            Services.ClientState.TerritoryChanged += this.HandleTerritoryChange;
        }

        /// <inheritdoc/>
        public override InstanceContentGuideConfiguration Configuration { get; } = InstanceContentGuideConfiguration.Instance;

        /// <summary>
        /// The duty associated with this guide.
        /// </summary>
        public Duty LinkedDuty { get; }

        /// <summary>
        /// The quest that unlocks this guide.
        /// </summary>
        public Quest UnlockQuest { get; }

        /// <summary>
        /// The sheet row of the duty associated with this guide.
        /// </summary>
        protected abstract uint DutyId { get; }

        /// <summary>
        /// The sheet row of the quest that unlocks this guide.
        /// </summary>
        protected abstract uint UnlockQuestId { get; }

        /// <summary>
        /// The structured content of the guide, used for rendering.
        /// </summary>
        public abstract InstanceContentGuideContent Content { get; }

        /// <summary>
        /// The note associated with this guide.
        /// </summary>
        public Note Note { get; }

        /// <inheritdoc/>
        public override string Name { get; }

        /// <inheritdoc/>
        public override string Description { get; }

        /// <inheritdoc/>
        public override uint Icon { get; }

        /// <inheritdoc/>
        public override ContentDifficulty Difficulty { get; }

        /// <inheritdoc/>
        public override Sirensong.Game.Enums.ContentType ContentType { get; }

        /// <inheritdoc/>
        public override unsafe bool IsUnlocked => QuestManager.IsQuestComplete(this.UnlockQuestId) || QuestManager.Instance()->IsQuestAccepted(this.UnlockQuestId);

        /// <inheritdoc/>
        protected override void DrawAction() => InstanceContentGuideContentUI.Draw(this);

        /// <summary>
        /// Handles territory changes and updates the guide if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="territoryId"></param>
        // TODO: improve how this works
        private unsafe void HandleTerritoryChange(object? sender, ushort territoryId)
        {
            if (this.LinkedDuty.CFCondition.TerritoryType.Row != territoryId || Services.GuideManager.SelectedGuide != null)
            {
                Services.GuideManager.SelectedGuide = null;
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
                if (InstanceContentDirector.HasFlag(ContentFlag.ExplorerMode))
                {
                    BetterLog.Debug("Not loading guide, player is using explorer mode.");
                    return;
                }

                BetterLog.Information($"Config: {this.Configuration.AutoOpen}");
                switch (this.Configuration.AutoOpen)
                {
                    case true:
                        Services.GuideManager.SelectedGuide = this;
                        Services.WindowManager.SetGuideViewerWindowVis(true);
                        break;
                    case false:
                        GameChat.Print(string.Format(Strings.Guide_InstanceContent_AvailableForDuty, this.Name, Constants.Commands.GuideViewer));
                        Services.GuideManager.SelectedGuide = this;
                        break;
                    default:
                }
            });
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    Services.ClientState.TerritoryChanged -= this.HandleTerritoryChange;
                }

                this.disposedValue = true;

                base.Dispose(disposing);
            }
        }
    }
}
