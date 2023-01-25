using System;
using System.Collections.Generic;
using System.Linq;
using KikoGuide.Common;
using KikoGuide.Enums;
using KikoGuide.GuideHandling;
using KikoGuide.UserInterface.Interfaces;
using Sirensong.Game.Enums;

namespace KikoGuide.UserInterface.Windows.GuideList
{
    internal sealed class GuideListLogic : IWindowLogic
    {
        /// <summary>
        ///     The search text to apply.
        /// </summary>
        public string SearchText = string.Empty;

        /// <summary>
        ///     The difficulty filter to apply.
        /// </summary>
        public ContentDifficulty? DifficultyFilter;

        /// <summary>
        ///     The total number of guides.
        /// </summary>
        public static int TotalGuides => Services.GuideManager.Guides.Count;

        /// <summary>
        ///     Fetch a filtered list of guides based on configuration, duty type, and search text.
        /// </summary>
        /// <param name="type">The type of duty to filter by.</param>
        /// <returns>A filtered list of guides.</returns>
        public HashSet<Guide> GetFilteredGuides(ContentTypeModified? type)
        {
            var filteredGuides = new HashSet<Guide>();
            var guides = type.HasValue ? Services.GuideManager.GetGuidesForType(type.Value) : Services.GuideManager.Guides;

            foreach (var guide in guides.Where(g => !g.NoShow))
            {
                if (this.DifficultyFilter.HasValue && guide.Difficulty != this.DifficultyFilter.Value)
                {
                    continue;
                }

                if (guide.Name.Contains(this.SearchText.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    filteredGuides.Add(guide);
                }
            }
            return filteredGuides;
        }

        /// <summary>
        ///    Get the number of guides for a given content type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GuidesForContentType(ContentTypeModified type) => Services.GuideManager.GetGuidesForType(type).Where(g => !g.NoShow).Count();
    }
}