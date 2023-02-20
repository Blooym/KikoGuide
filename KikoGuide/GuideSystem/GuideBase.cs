using System;
using KikoGuide.DataModels;
using KikoGuide.GuideSystem.Interfaces;
using KikoGuide.Resources.Localization;
using Sirensong.Game.Enums;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.GuideSystem
{
    /// <summary>
    ///     The base class that all guides inherit from, controls the base functionality of a guide.
    /// </summary>
    internal abstract class GuideBase : IDisposable, IGuide
    {
        private bool disposedValue;

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public abstract string[] Authors { get; }

        /// <inheritdoc />
        public abstract uint Icon { get; }

        /// <inheritdoc />
        public abstract ContentDifficulty Difficulty { get; }

        /// <inheritdoc />
        public abstract ContentType ContentType { get; }

        /// <inheritdoc />
        public abstract Note Note { get; }

        /// <inheritdoc />
        public abstract bool IsUnlocked { get; }

        /// <inheritdoc />
        public abstract IGuideConfiguration Configuration { get; }

        /// <inheritdoc />
        public virtual bool NoShow { get; }

        /// <inheritdoc />
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
        ///     The action to run to draw the content of the guide, this is wrapped by a try/catch and unlock check in the base
        ///     class.
        /// </summary>
        protected abstract void DrawAction();

        /// <summary>
        ///     Dispose of the guide.
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

        ~GuideBase() => this.Dispose(false);
    }
}
