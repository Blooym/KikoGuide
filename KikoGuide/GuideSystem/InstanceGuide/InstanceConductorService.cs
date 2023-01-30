using System;
using System.Linq;
using System.Threading.Tasks;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.Game;
using Sirensong.Game.Enums;
using Sirensong.Game.Utility;

namespace KikoGuide.GuideSystem.InstanceGuide
{
    internal sealed class InstanceConductorService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Creates a new <see cref="InstanceConductorService"/>.
        /// </summary>
        public InstanceConductorService() => Services.ClientState.TerritoryChanged += this.TerritoryChanged;

        /// <summary>
        /// Disposes the <see cref="InstanceConductorService"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                Services.ClientState.TerritoryChanged -= this.TerritoryChanged;

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Called when the territory changes, handles auto opening of instance guides.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="territoryId"></param>
        private unsafe void TerritoryChanged(object? sender, ushort territoryId)
        {
            var duties = Services.GuideManager.GetGuides<InstanceGuideBase>();
            if (duties == null)
            {
                return;
            }
            var duty = duties.FirstOrDefault(d => d.LinkedDuty.CFCondition.TerritoryType.Row == territoryId);

            if (duty == null)
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
                if (InstanceContentDirectorUtil.HasFlag(ContentFlag.ExplorerMode))
                {
                    return;
                }

                Services.GuideManager.SelectedGuide = duty;
                switch (duty.Configuration.AutoOpen)
                {
                    case true:
                        Services.WindowManager.SetGuideViewerWindowVis(true);
                        break;
                    case false:
                        GameChat.Print(string.Format(Strings.Guide_InstanceContent_AvailableForDuty, duty.Name, Constants.Commands.GuideViewer));
                        break;
                    default:
                }
            });
        }
    }
}