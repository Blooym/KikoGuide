using System;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.Extensions;
using Sirensong.Game.Enums;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.UserInterface.Windows.GuideList.TableParts
{
    internal static class GuideListSidebar
    {
        /// <summary>
        /// Draw the sidebar.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(GuideListLogic logic)
        {
            DrawSearchbar(logic);
            ImGui.Dummy(Spacing.SidebarElementSpacing);

            DrawDifficultyFilter(logic);
            ImGui.Dummy(Spacing.SidebarElementSpacing);

            DrawConfiguration(logic);
            ImGui.Dummy(Spacing.SidebarElementSpacing);

            DrawContributing(logic);
            ImGui.Dummy(Spacing.SidebarElementSpacing);

            SiGui.Footer($"{Constants.PluginName.TrimWhitepace()} v{Constants.Build.VersionInformational} #{Constants.Build.GitCommitHash}");
        }

        /// <summary>
        /// Draw the searchbar.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawSearchbar(GuideListLogic logic)
        {
            SiGui.TextDisabled(Strings.UserInterface_GuideList_Search_Heading);
            ImGui.Separator();
            ImGui.SetNextItemWidth(-1);
            SiGui.InputTextHint("##GuideSearch", Strings.UserInterface_GuideList_SearchHint, ref logic.SearchText, 50);
        }

        /// <summary>
        /// Draw the difficulty filter.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawDifficultyFilter(GuideListLogic logic)
        {
            SiGui.TextDisabled(Strings.UserInterface_GuideList_DifficultyFilter_Heading);
            ImGui.Separator();
            if (ImGui.Selectable(Strings.UserInterface_GuideList_DifficultyFilter_Any, logic.DifficultyFilter == null))
            {
                logic.DifficultyFilter = null;
            }

            foreach (var difficulty in Enum.GetValues<ContentDifficulty>())
            {
                if (ImGui.Selectable(difficulty.GetLocalizedName(), logic.DifficultyFilter == difficulty))
                {
                    logic.DifficultyFilter = difficulty;
                }
            }
        }

        /// <summary>
        /// Draw the contributing section.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawContributing(GuideListLogic _)
        {
            SiGui.TextDisabled(Strings.UserInterface_GuideList_Contributing_Heading);
            ImGui.Separator();

            if (ImGui.Selectable(Strings.UserInterface_GuideList_Contributing_Guides))
            {
                Util.OpenLink(Constants.Links.GitHub);
            }

            if (ImGui.Selectable(Strings.UserInterface_GuideList_Contributing_Translate))
            {
                Util.OpenLink(Constants.Links.Crowdin);
            }

            if (ImGui.Selectable(Strings.UserInterface_GuideList_Contributing_Donate))
            {
                Util.OpenLink(Constants.Links.KoFi);
            }
        }

        /// <summary>
        /// Draw the configuration section.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawConfiguration(GuideListLogic _)
        {
            SiGui.TextDisabled(Strings.UserInterface_GuideList_Configuration_Heading);
            ImGui.Separator();
            if (ImGui.Selectable(Strings.UserInterface_GuideList_Configuration_Plugin))
            {
                GuideListLogic.ToggleSettingsWindow();
            }

            if (ImGui.Selectable(Strings.UserInterface_GuideList_Configuration_Guide))
            {
                GuideListLogic.ToggleGuideSettingsWindow();
            }

            if (ImGui.Selectable(Strings.UserInterface_GuideList_Configuration_Integrations))
            {
                GuideListLogic.ToggleIntegrationsWindow();
            }
        }
    }
}
