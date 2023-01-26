using System;
using KikoGuide.Enums;
using Sirensong.Game.Enums;

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
        /// The authors of the guide.
        /// </summary>
        public abstract string[] Authors { get; }

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
        /// The method to draw the guide content.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Whether or not the guide should be considered unlocked.
        /// </summary>
        public abstract bool IsUnlocked { get; }

        /// <summary>
        /// Whether or not the guide should be hidden from the guide list, even if unlocked.
        /// </summary>
        public virtual bool NoShow { get; }

        /// <summary>
        /// The method to dispose of the guide.
        /// </summary>
        public abstract void Dispose();
    }
}