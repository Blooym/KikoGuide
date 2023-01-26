using System;
using System.Collections.Generic;
using System.Linq;
using KikoGuide.Common;
using KikoGuide.Enums;
using KikoGuide.Extensions;
using KikoGuide.GuideSystem;
using Sirensong.Game.Enums;

namespace KikoGuide.UserInterface.Windows.GuideList
{
    internal sealed class GuideListLogic
    {
        /// <summary>
        /// The search text to apply.
        /// </summary>
        public string SearchText = string.Empty;

        /// <summary>
        /// The difficulty filter to apply.
        /// </summary>
        public ContentDifficulty? DifficultyFilter;

        /// <summary>
        /// The total number of guides.
        /// </summary>
        public static int UnlockedGuides => Services.GuideManager.Guides.Where(g => !g.NoShow && g.IsUnlocked).Count();

        /// <summary>
        /// Get the number of guides for a given content type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GuidesForContentType(ContentTypeModified type) => Services.GuideManager.GetGuidesForType(type).Where(g => !g.NoShow && g.IsUnlocked).Count();

        /// <summary>
        /// Whether or not the player is logged in.
        /// </summary>
        public static bool IsLoggedIn => Services.ClientState.IsLoggedIn;

        public static GuideBase? CurrentGuide => Services.GuideManager.SelectedGuide;

        public static void SetCurrentGuide(GuideBase guide) => Services.GuideManager.SetSelectedGuide(guide, true);

        /// <summary>
        /// Fetch a filtered list of guides based on configuration, duty type, and search text.
        /// </summary>
        /// <param name="type">The type of duty to filter by.</param>
        /// <returns>A filtered list of guides.</returns>
        public HashSet<GuideBase> GetFilteredGuides(ContentTypeModified? type)
        {
            var filteredGuides = new HashSet<GuideBase>();
            var guides = type.HasValue ? Services.GuideManager.GetGuidesForType(type.Value) : Services.GuideManager.Guides;

            foreach (var guide in guides.Where(g => !g.NoShow))
            {
                if ((this.DifficultyFilter.HasValue && guide.Difficulty != this.DifficultyFilter.Value) || !guide.IsUnlocked)
                {
                    continue;
                }

                if (guide.Name.Contains(this.SearchText.TrimAndSquish(), StringComparison.OrdinalIgnoreCase))
                {
                    filteredGuides.Add(guide);
                }
            }
            return filteredGuides;
        }


        /// <summary>
        /// Opens the settings window.
        /// </summary>
        public static void OpenSettings() => Services.WindowManager.WindowingSystem.ToggleConfigWindow();
    }
}