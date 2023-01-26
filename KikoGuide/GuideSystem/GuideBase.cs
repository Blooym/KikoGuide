using System;
using Dalamud.Interface.Colors;
using KikoGuide.Common;
using KikoGuide.Enums;
using Sirensong.Game.Enums;
using Sirensong.UserInterface;

// TODO: Implement some kind of logic for handling guide auto-opening.
namespace KikoGuide.GuideSystem
{
    /// <summary>
    /// The base class that all guides inherit from, controls the basic properties of a guide that are non-specific to the type of guide.
    /// </summary>
    internal abstract class GuideBase : IDisposable
    {
        /// <summary>
        /// The runtime unique identifier of the guide.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// The name of the guide.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The description of the guide.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The icon of the guide.
        /// </summary>
        public abstract uint Icon { get; }

        /// <summary>
        /// The difficulty type of the guide. Specify normal if non-applicable.
        /// </summary>
        public abstract ContentDifficulty Difficulty { get; }

        /// <summary>
        /// The content type of the guide.
        /// </summary>
        public abstract ContentTypeModified ContentType { get; }

        /// <summary>
        /// Whether or not the guide should allow itself to open automatically.
        /// </summary>
        public virtual bool AutoOpen => Services.Configuration.AutoOpenGuides;

        /// <summary>
        /// Whether or not the guide should be considered unlocked.
        /// </summary>
        public abstract bool IsUnlocked { get; }

        /// <summary>
        /// Whether or not the guide should be hidden from the guide list, even if unlocked.
        /// </summary>
        public virtual bool NoShow { get; }

        /// <summary>
        /// The content to draw in the guide viewer, called by the base class.
        /// </summary>
        protected abstract void DrawAction();

        /// <summary>
        /// Draws the guide content.
        /// </summary>
        public void Draw()
        {
            try
            {
                this.DrawAction();
            }
            catch (Exception e)
            {
                SiGui.TextWrappedColoured(ImGuiColors.DalamudRed, $"Draw failed to due to an error! [{e.GetType().Name}] {e.Message}");
            }
        }

        /// <summary>
        /// The method to dispose of the guide.
        /// </summary>
        public abstract void Dispose();
    }
}