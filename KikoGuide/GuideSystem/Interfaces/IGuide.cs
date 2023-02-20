using System;
using KikoGuide.DataModels;
using Sirensong.Game.Enums;

namespace KikoGuide.GuideSystem.Interfaces
{
    /// <summary>
    ///     The interface for all guides.
    /// </summary>
    internal interface IGuide
    {
        /// <summary>
        ///     The runtime unique identifier of the guide.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     The name of the guide.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The description of the guide.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     The authors of the guide.
        /// </summary>
        string[] Authors { get; }

        /// <summary>
        ///     The icon of the guide.
        /// </summary>
        uint Icon { get; }

        /// <summary>
        ///     The difficulty type of the guide. Specify normal if non-applicable.
        /// </summary>
        ContentDifficulty Difficulty { get; }

        /// <summary>
        ///     The content type of the guide.
        /// </summary>
        ContentType ContentType { get; }

        /// <summary>
        ///     The note associated with the guide.
        /// </summary>
        Note Note { get; }

        /// <summary>
        ///     Whether or not the guide should be considered unlocked.
        /// </summary>
        bool IsUnlocked { get; }

        /// <summary>
        ///     Whether or not the guide should be considered hidden.
        /// </summary>
        IGuideConfiguration Configuration { get; }

        /// <summary>
        ///     Whether or not the guide should be considered hidden.
        /// </summary>
        bool NoShow { get; }

        /// <summary>
        ///     Whether or not the guide should be considered hidden.
        /// </summary>
        void Draw();
    }
}
