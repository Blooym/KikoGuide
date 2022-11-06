using System.Collections.Generic;
using KikoGuide.Base;
using KikoGuide.Managers;
using KikoGuide.Types;

namespace KikoGuide.UI.ImGuiFullComponents.GuideListTable
{
    public sealed class GuideListTablePresenter
    {
        public static Guide? GetGuideForPlayerTerritory() => GuideManager.GetGuideForCurrentTerritory();

        public static bool HasGuideData(Guide guide) => guide.Sections != null && guide.Sections.Count > 0;

        public static bool HasAnyGuideUnlocked(List<Guide> guideList)
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

        public static bool GuideExistsForSearch(List<Guide> guideList, string search)
        {
            var guideExistsForSearch = false;
            foreach (var guide in guideList)
            {
                if (guide.Name.ToLower().Contains(search.ToLower()))
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
