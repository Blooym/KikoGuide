using System;
using System.Collections.Generic;
using System.Linq;
using KikoGuide.Base;
using KikoGuide.Types;
using KikoGuide.Utils;

namespace KikoGuide.UI.ImGuiFullComponents.GuideListTable
{
    internal sealed class GuideListTablePresenter
    {
        internal static Guide? GetGuideForPlayerTerritory() => GuideUtil.GetGuideForCurrentTerritory();

        internal static bool HasGuideData(Guide guide) => guide.Sections?.Count > 0;

        internal static bool HasAnyGuideUnlocked(List<Guide> guideList)
        {
            var hasGuideUnlocked = false;
            foreach (var guide in guideList)
            {
                if (guide.IsUnlocked() || !Configuration.Display.HideLockedGuides)
                {
                    hasGuideUnlocked = true;
                    break;
                }
            }
            return hasGuideUnlocked;
        }

        internal static bool GuideExistsForSearch(List<Guide> guideList, string search)
        {
            var guideExistsForSearch = false;
            foreach (var guide in guideList.Where(g => g.Name.Contains(search, StringComparison.OrdinalIgnoreCase)))
            {
                if (guide.IsUnlocked() || !Configuration.Display.HideLockedGuides)
                {
                    guideExistsForSearch = true;
                    break;
                }
            }
            return guideExistsForSearch;
        }

        internal static Configuration Configuration => PluginService.Configuration;
    }
}
