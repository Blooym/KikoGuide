using System;
using System.Linq;
using Dalamud.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using KikoGuide.Common;

namespace KikoGuide.GuideSystem.FateGuide
{
    /// <summary>
    /// Manages fate guide auto-selection.
    /// </summary>
    internal sealed class FateConductorService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The fate configuration.
        /// </summary>
        private static FateConfiguration Configuration => FateConfiguration.Instance;

        /// <summary>
        /// The currently selected fate Id.
        /// </summary>
        private uint currentFateId;

        /// <summary>
        /// Creates a new <see cref="FateConductorService"/>.
        /// </summary>
        public FateConductorService() => Services.Framework.Update += this.OnFrameworkUpdate;

        /// <summary>
        /// Disposes the <see cref="FateConductorService"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                Services.Framework.Update -= this.OnFrameworkUpdate;
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Called when the framework updates, handles auto opening of fate guides.
        /// </summary>
        /// <param name="framework"></param>
        private unsafe void OnFrameworkUpdate(Framework framework)
        {
            if (!Configuration.AutoOpen)
            {
                return;
            }

            // Get the fate manager.
            var fateMgr = FateManager.Instance();
            if (fateMgr == null)
            {
                return;
            }

            // Get the current fate.
            var currentFate = fateMgr->CurrentFate;
            if (currentFate == null)
            {
                if (this.currentFateId != 0)
                {
                    Services.GuideManager.SelectedGuide = null;
                    this.currentFateId = 0;
                }
                return;
            }

            // If the currnet fate is different from the last one, select it.
            if (currentFate->FateId != this.currentFateId)
            {
                this.currentFateId = currentFate->FateId;

                // Get guides for the current fate.
                var fateGuides = Services.GuideManager.GetGuides<FateGuideBase>();
                if (fateGuides == null)
                {
                    return;
                }

                // Select the first guide that matches the current fate.
                var fate = fateGuides.FirstOrDefault(f => f.Fate.RowId == this.currentFateId);
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
        }
    }
}