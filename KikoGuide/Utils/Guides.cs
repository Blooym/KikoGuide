using System.Linq;
using KikoGuide.Base;
using KikoGuide.Types;

namespace KikoGuide.Utils
{
    /// <summary>
    ///     Common utilities for guides.
    /// </summary>
    internal static class GuideUtil
    {
        /// <summary>
        ///     Get the guide for the current territory.
        /// </summary>
        /// <returns> The guide for the current territory. </returns>
        internal static Guide? GetGuideForCurrentTerritory() => PluginService.GuideManager.GetAllGuides().Find(guide => guide.TerritoryIDs.Contains(PluginService.ClientState.TerritoryType));
    }
}
