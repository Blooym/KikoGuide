using System;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Common;
using Sirensong.Game.Enums;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.GuideList.TableParts
{
    internal static class Sidebar
    {
        /// <summary>
        ///     Draw the sidebar.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(GuideListLogic logic)
        {
            DrawSearchbar(logic);
            ImGui.Dummy(new(0, 20));

            DrawDifficultyFilter(logic);
            ImGui.Dummy(new(0, 20));

            DrawContributing(logic);
        }

        /// <summary>
        ///     Draw the searchbar.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawSearchbar(GuideListLogic logic)
        {
            ImGui.TextDisabled("Guide Search");
            ImGui.Separator();
            ImGui.SetNextItemWidth(-1);
            SiGui.InputTextHint("##GuideSearch", "Search...", ref logic.SearchText, 100);
        }

        /// <summary>
        ///     Draw the difficulty filter.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawDifficultyFilter(GuideListLogic logic)
        {
            ImGui.TextDisabled("Difficulty Filter");
            ImGui.Separator();
            if (ImGui.Selectable("Any", logic.DifficultyFilter == null))
            {
                logic.DifficultyFilter = null;
            }
            foreach (var difficulty in Enum.GetValues<ContentDifficulty>())
            {
                if (ImGui.Selectable(difficulty.ToString(), logic.DifficultyFilter == difficulty))
                {
                    logic.DifficultyFilter = difficulty;
                }
            }
        }

        /// <summary>
        ///     Draw the contributing section.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawContributing(GuideListLogic _)
        {
            ImGui.TextDisabled("Contributing");
            ImGui.Separator();
            if (ImGui.Selectable("Submit a guide (GitHub)", false))
            {
                Util.OpenLink(Constants.Links.GitHub);
            }
            if (ImGui.Selectable("Donate (Ko-Fi)", false))
            {
                Util.OpenLink(Constants.Links.GitHub);
            }
        }
    }
}