using System;
using KikoGuide.DataModels;
using KikoGuide.Resources.Localization;
using Sirensong.Game.Enums;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

// TODO: Implement some kind of logic for handling guide auto-opening.
namespace KikoGuide.GuideSystem
{
    /// <summary>
    /// The base class that all guides inherit from, controls the base functionality of a guide.
    /// </summary>
    internal abstract class GuideBase : IDisposable
    {
        private bool disposedValue;

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
        /// The authors of the guide.
        /// </summary>
        public abstract string[] Authors { get; }

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
        public abstract ContentType ContentType { get; }

        /// <summary>
        /// The note associated with the guide.
        /// </summary>
        public abstract Note Note { get; }

        /// <summary>
        /// Whether or not the guide should be considered unlocked.
        /// </summary>
        public abstract bool IsUnlocked { get; }

        /// <summary>
        /// Stores the configuration of the overall "type" of the guide.
        /// </summary>
        public abstract GuideConfigurationBase Configuration { get; }

        /// <summary>
        /// Whether or not the guide should be hidden from the guide list, even if unlocked.
        /// </summary>
        public virtual bool NoShow { get; }

        /// <summary>
        /// The action to run to draw the content of the guide, this is wrapped by a try/catch and unlock check in the base class.
        /// </summary>
        protected abstract void DrawAction();

        /// <summary>
        /// Draws the guide content.
        /// </summary>
        public void Draw()
        {
            try
            {
                if (!this.IsUnlocked)
                {
                    SiGui.TextWrappedColoured(Colours.Warning, Strings.Guide_NotMetRequirements);
                    return;
                }

                this.DrawAction();
            }
            catch (Exception e)
            {
                SiGui.TextWrappedColoured(Colours.Error, string.Format(Strings.Errors_DrawFailed, e.GetType().Name, e.Message));
            }
        }

        /// <summary>
        /// Dispose of the guide.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the guide.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {

                }
            }

            this.disposedValue = true;
        }

        ~GuideBase()
        {
            this.Dispose(false);
        }
    }
}