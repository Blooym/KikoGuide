using System;
using System.Globalization;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using KikoGuide.Common;
using KikoGuide.DataModels;
using KikoGuide.Enums;
using KikoGuide.UserInterface.Layouts;
using KikoGuide.UserInterface.Windows.GuideViewer;
using Lumina.Excel.GeneratedSheets;
using Sirensong.DataStructures;
using Sirensong.Game.Enums;
using Sirensong.Game.Extensions;
using Sirensong.Game.UI;

namespace KikoGuide.GuideHandling
{
    /// <summary>
    /// Represents a guide, which is a collection of information.
    /// </summary>
    /// <remarks>
    /// This class is abstract and should be inherited from to create a new guide.
    /// </remarks>
    internal abstract class GuideBase : IDisposable
    {
        // Constructors, Destructors & Dispose

        /// <summary>
        /// Creates a new guide.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the unlock quest or linked duty is null and the guide is not marked as unsafe.</exception>
        public GuideBase()
        {
            this.Duty = Duty.GetDutyOrNull(this.DutyId)!;
            this.UnlockQuest = Services.Data.GetExcelSheet<Quest>()?.GetRow(this.UnlockQuestId)!;
            this.Note = Note.CreateOrLoad(this.Name);

            if (!this.UseUnsafeNoGuideLink && (this.UnlockQuest == null || this.Duty == null))
            {
                throw new ArgumentException(Constants.ExceptionMessages.InvalidDutyOrUnlockQuest);
            }

            Services.ClientState.TerritoryChanged += this.HandleTerritoryChange;
        }

        /// <summary>
        /// Destructor for the guide.
        /// </summary>
        ~GuideBase()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Whether or not this guide has been disposed.
        /// </summary>
        protected bool Disposed { get; private set; }

        /// <summary>
        /// Disposes of the guide.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.Disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose of managed resources.
            }

            Services.ClientState.TerritoryChanged -= this.HandleTerritoryChange;

            this.nameCached = null;
            this.Disposed = true;
        }

        // Events & Handlers

        /// <summary>
        /// Handles territory changes for the guide, auto opening if <see cref="ShouldAutoOpen" /> & <see cref="IsInGuideTerritory" /> are true.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="territory">The territory the player has changed to.</param>
        protected virtual unsafe void HandleTerritoryChange(object? sender, ushort territory)
        {
            if (!this.IsInGuideTerritory || Services.GuideManager.CurrentGuide == this)
            {
                return;
            }

            Services.GuideManager.CurrentGuide = this;
            switch (this.ShouldAutoOpen)
            {
                case true:
                    this.SetCurrent(true);
                    break;
                case false:
                    this.SetCurrent(false);
                    GameChat.Print($"A guide is available for {this.Name}. Use /kiko to open it.");
                    break;
                default:
            }
        }


        // Properties & Fields

        /// <summary>
        /// The cached normalized name of the guide, prevents converting SeString -> string every time.
        /// </summary>
        private string? nameCached;

        /// <summary>
        /// The cached icon of the guide, prevents reading from a sheet every time.
        /// </summary>
        private uint? iconCached;

        /// <summary>
        /// The cached content type of the guide, prevents remapping every time.
        /// </summary>
        private ContentTypeModified? contentTypeCached;

        /// <summary>
        /// The cached content difficulty of the guide, prevents remapping every time.
        /// </summary>
        private ContentDifficulty? contentDifficultyCached;

        /// <summary>
        /// Whether or not this guide should be loaded by the <see cref="GuideManager"/>.
        /// </summary>
        public static bool NoLoad { get; }

        /// <summary>
        /// The guide's unique identifier, generated at runtime.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Whether or not this guide should be hidden from all UI/IPC.
        /// </summary>
        public virtual bool NoShow { get; }

        /// <summary>
        /// Disables validation checks for the guide. Only use for creating custom guides that don't link to duties and if you know what you're doing.
        /// </summary>
        protected virtual bool UseUnsafeNoGuideLink { get; }

        /// <summary>
        /// The note associated with this guide.
        /// </summary>
        public Note Note { get; }

        /// <summary>
        /// The duty this guide is linked to, created from the guide <see cref="DutyId"/>.
        /// </summary>
        public Duty Duty { get; }

        /// <summary>
        /// The duty ID of the linked duty.
        /// </summary>
        public abstract uint DutyId { get; }

        /// <summary>
        /// The quest that unlocks this guide, created from the guide <see cref="UnlockQuestId"/>.
        /// </summary>
        public Quest UnlockQuest { get; }

        /// <summary>
        /// The quest ID of the unlock quest.
        /// </summary>
        public abstract uint UnlockQuestId { get; }

        /// <summary>
        /// The content of this guide.
        /// </summary>
        public abstract GuideContent Content { get; protected set; }

        /// <summary>
        /// The author(s) of this guide.
        /// </summary>
        public abstract string[] Authors { get; }


        // Methods

        /// <summary>
        /// The name of the guide, usually the name of the linked duty.
        /// </summary>
        public virtual string Name => this.nameCached ??= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.Duty?.CFCondition.Name.ToDalamudString().ToString() ?? "An Unnamed Guide");

        /// <summary>
        /// The icon to show for this guide in all supported UIs.
        /// </summary>
        public virtual uint Icon => this.iconCached ??= (this.Duty.CFCondition.ContentType.Value?.Icon ?? 21);

        /// <summary>
        /// The linked difficulty of the guide, usually the same as the duty.
        /// </summary>
        public virtual ContentDifficulty Difficulty => this.contentDifficultyCached ??= this.Duty?.CFCondition.GetContentDifficulty() ?? ContentDifficulty.Normal;

        /// <summary>
        /// The content type of the guide, usually the same as the duty.
        /// </summary>
        public virtual ContentTypeModified Type => this.contentTypeCached ??= (ContentTypeModified?)this.Duty?.CFCondition.GetContentType() ?? ContentTypeModified.Custom;

        /// <summary>
        /// Whether or not the player has unlocked this guide.
        /// </summary>
        public virtual unsafe bool IsGuideUnlocked => this.UnlockQuest == null || QuestManager.IsQuestComplete(this.UnlockQuestId) || QuestManager.Instance()->IsQuestAccepted(this.UnlockQuestId);

        /// <summary>
        /// Whether or not the player is currently in the territory this guide is linked to.
        /// </summary>
        public virtual unsafe bool IsInGuideTerritory => this.Duty == null || Services.ClientState.TerritoryType == this.Duty?.CFCondition.TerritoryType.Value?.RowId;

        /// <summary>
        /// Whether or not the guide should be automatically opened when the player enters the territory.
        /// </summary>
        protected virtual bool ShouldAutoOpen => Services.Configuration.OpenGuideOnInstanceLoad;

        /// <summary>
        /// Whether or not the guide is currently set as active in the <see cref="GuideManager"/>.
        /// </summary>
        public bool IsActive => Services.GuideManager.CurrentGuide == this;

        /// <summary>
        /// Sets the current guide to this guide and opens the guide viewer window if <paramref name="openWindow"/> is true.
        /// </summary>
        /// <param name="openWindow">Whether or not to open the guide viewer window.</param>
        public virtual void SetCurrent(bool openWindow)
        {
            var window = Services.WindowManager.WindowingSystem.GetWindow<GuideViewerWindow>();
            if (window != null)
            {
                Services.GuideManager.CurrentGuide = this;
                if (openWindow)
                {
                    window.IsOpen = true;
                }
            }
        }

        /// <summary>
        /// Draws the UI for this guide.
        /// </summary>
        public virtual void Draw() => DutyGuideLayout.Draw(this);

        // Sub-records & enums

        /// <summary>
        /// Represents the content of a guide.
        /// </summary>
        public sealed record GuideContent
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

