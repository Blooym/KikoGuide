using System;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using KikoGuide.Common;
using Sirensong;
using Sirensong.Game.State;

namespace KikoGuide.GuideSystem.FateGuide
{
    /// <summary>
    /// Manages fate guide auto-selection.
    /// </summary>
    internal sealed unsafe class FateConductorService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The fate configuration.
        /// </summary>
        private static readonly FateConfiguration Configuration = FateConfiguration.Instance;

        /// <summary>
        /// The fate state manager.
        /// </summary>
        private static readonly FateStateManager FateStateManager = SirenCore.GetOrCreateService<FateStateManager>();

        /// <summary>
        /// Creates a new <see cref="FateConductorService"/>.
        /// </summary>
        public FateConductorService()
        {
            FateStateManager.FateJoined += this.OnFateJoined;
            FateStateManager.FateLeft += this.OnFateLeft;
        }

        /// <summary>
        /// Disposes the <see cref="FateConductorService"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                FateStateManager.FateJoined -= this.OnFateJoined;
                FateStateManager.FateLeft -= this.OnFateLeft;
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Handles fate joining.
        /// </summary>
        /// <param name="fateContext"></param>
        private void OnFateJoined(FateContext* fateContext)
        {
            if (!Configuration.AutoOpen)
            {
                return;
            }

            // Get guides for the current fate.
            var fateGuides = Services.GuideManager.GetGuides<FateGuideBase>();
            if (fateGuides == null)
            {
                return;
            }

            // Select the first guide that matches the current fate.
            var fate = fateGuides.FirstOrDefault(f => f.Fate.RowId == fateContext->FateId);
            if (fate == null)
            {
                return;
            }

            // Handle opening the guide.
            switch (Configuration.AutoOpen)
            {
                case true:
                    Services.GuideManager.SelectedGuide = fate;
                    Services.WindowManager.SetGuideViewerWindowVis(true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles fate leaving.
        /// </summary>
        private void OnFateLeft()
        {
            if (!Configuration.AutoOpen)
            {
                return;
            }

            Services.WindowManager.SetGuideViewerWindowVis(false);
            Services.GuideManager.SelectedGuide = null;
        }
    }
}