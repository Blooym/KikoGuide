using System;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Common;
using Sirensong.Extensions;
using Sirensong.Game.Enums;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.GuideList.TableParts
{
    internal static class Sidebar
    {
        /// <summary>
        /// Draw the sidebar.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(GuideListLogic logic)
        {
            DrawSearchbar(logic);
            ImGui.Dummy(new(0, 20));

            DrawDifficultyFilter(logic);
            ImGui.Dummy(new(0, 20));

            DrawConfiguration(logic);
            ImGui.Dummy(new(0, 20));

            DrawContributing(logic);
            ImGui.Dummy(new(0, 5));

            SiGui.TextFooter(Constants.PluginName.TrimWhitepace() + " v" + Constants.Version + " #" + Constants.GitCommitHash);
        }

        /// <summary>
        /// Draw the searchbar.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawSearchbar(GuideListLogic logic)
        {
            ImGui.TextDisabled("Guide Search");
            ImGui.Separator();
            ImGui.SetNextItemWidth(-1);
            SiGui.InputTextHint("##GuideSearch", "Search by name...", ref logic.SearchText, 50);
        }

        /// <summary>
        /// Draw the difficulty filter.
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
        /// Draw the contributing section.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawContributing(GuideListLogic _)
        {
            ImGui.TextDisabled("Contributing");
            ImGui.Separator();

            if (ImGui.Selectable("Create/Edit Guides"))
            {
                Util.OpenLink(Constants.Links.GitHub);
            }

            if (ImGui.Selectable("Help Translate"))
            {
                Util.OpenLink(Constants.Links.KoFi);
            }

            if (ImGui.Selectable("Support the Developer"))
            {
                Util.OpenLink(Constants.Links.KoFi);
            }
        }

        private static void DrawConfiguration(GuideListLogic _)
        {
            ImGui.TextDisabled("Configuration");
            ImGui.Separator();
            if (ImGui.Selectable("Plugin configuration"))
            {
                GuideListLogic.ToggleSettingsWindow();
            }

            ImGui.BeginDisabled();
            if (ImGui.Selectable("Integrations configuration"))
            {
                //
            }
            ImGui.EndDisabled();
        }
    }
}