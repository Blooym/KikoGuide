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
        /// The search text to apply to the guide list.
        /// </summary>
        public string SearchText = string.Empty;

        /// <summary>
        /// The difficulty filter to apply to the guide list.
        /// </summary>
        public ContentDifficulty? DifficultyFilter;

        /// <summary>
        /// Whether or not the player is logged in.
        /// </summary>
        public static bool IsLoggedIn => Services.ClientState.IsLoggedIn;

        /// <summary>
        /// The total number of guides.
        /// </summary>
        public static int UnlockedGuides => Services.GuideManager.GetGuides().Where(g => !g.NoShow && g.IsUnlocked).Count();

        /// <summary>
        /// Gets the currently selected guide.
        /// </summary>
        public static GuideBase? CurrentGuide => Services.GuideManager.SelectedGuide;

        /// <summary>
        /// Toggle the settings window.
        /// </summary>
        public static void ToggleSettingsWindow() => Services.WindowManager.ToggleSettingsWindow();

        /// <summary>
        /// Toggle the integrations window.
        /// </summary>
        public static void ToggleIntegrationsWindow() => Services.WindowManager.ToggleIntegrationSettingsWindow();

        /// <summary>
        /// Toggle the guide settings window.
        /// </summary>
        public static void ToggleGuideSettingsWindow() => Services.WindowManager.ToggleGuideConfigSettingsWindow();

        /// <summary>
        /// Get the number of guides for a given content type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GuidesForContentType(ContentTypeModified type) => Services.GuideManager.GetGuides(type).Where(g => !g.NoShow && g.IsUnlocked).Count();

        /// <summary>
        /// Opens the given guide in the guide viewer.
        /// </summary>
        /// <param name="guide">The guide to open.</param>
        public static void OpenGuide(GuideBase guide)
        {
            Services.GuideManager.SelectedGuide = guide;
            Services.WindowManager.SetGuideViewerWindowVis(true);
        }

        /// <summary>
        /// Fetch a filtered list of guides based on configuration, duty type, and search text.
        /// </summary>
        /// <param name="type">The type of duty to filter by.</param>
        /// <returns>A filtered list of guides.</returns>
        public HashSet<GuideBase> GetFilteredGuides(ContentTypeModified? type)
        {
            var filteredGuides = new HashSet<GuideBase>();
            var guides = type.HasValue ? Services.GuideManager.GetGuides(type.Value) : Services.GuideManager.GetGuides();

            foreach (var guide in guides)
            {
                if (guide.NoShow || !guide.IsUnlocked)
                {
                    continue;
                }

                if (this.DifficultyFilter.HasValue && guide.Difficulty != this.DifficultyFilter.Value)
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
    }
}