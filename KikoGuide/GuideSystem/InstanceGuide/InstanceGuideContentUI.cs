using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Resources.Localization;
using Sirensong.DataStructures;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.GuideSystem.InstanceGuide
{
    internal static class InstanceGuideContentUI
    {
        /// <summary>
        /// Draws the guide.
        /// </summary>
        /// <param name="guide">The guide to draw.</param>
        public static void Draw(InstanceGuideBase guide)
        {
            DrawSections(guide);
            ImGui.EndChild();
        }

        /// <summary>
        /// Draws the sections of the guide.
        /// </summary>
        /// <param name="guide">The guide to draw.</param>
        private static void DrawSections(InstanceGuideBase guide)
        {
            if (ImGui.BeginTabBar("Sections", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
            {
                foreach (var section in guide.Content.Sections)
                {
                    DrawSection(section);
                }
                ImGui.EndTabBar();
            }
        }

        /// <summary>
        /// Draws a section.
        /// </summary>
        /// <param name="section">The section to draw.</param>
        private static void DrawSection(InstanceGuideContent.Section section)
        {
            if (ImGui.BeginTabItem(section.Title.UICurrent))
            {
                // Don't draw tab items for 1 subsection
                if (section.Subsections.Length == 1)
                {
                    DrawSubsection(section.Subsections[0]);
                }

                // Otherwise, draw subsections as tabs
                else if (ImGui.BeginTabBar("Subsections", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
                {
                    foreach (var subsection in section.Subsections)
                    {
                        if (ImGui.BeginTabItem(subsection.Title.UICurrent))
                        {
                            DrawSubsection(subsection);
                            ImGui.EndTabItem();
                        }
                    }
                    ImGui.EndTabBar();
                }
                ImGui.EndTabItem();
            }
        }

        /// <summary>
        /// Draws a subsection.
        /// </summary>
        /// <param name="subsection">The subsection to draw.</param>
        private static void DrawSubsection(InstanceGuideContent.Section.Subsection subsection)
        {
            if (ImGui.BeginChild("Subsection"))
            {
                DrawSubsectionContent(subsection.Content);
                ImGui.Dummy(Spacing.SectionSpacing);

                // Draw tips if set
                if (subsection.Tips != null)
                {
                    DrawTips(subsection.Tips);
                    ImGui.Dummy(Spacing.SectionSpacing);
                }

                // Draw mechanics table if set
                if (subsection.Mechanics != null)
                {
                    DrawMechanicsTable(subsection.Mechanics);
                    ImGui.Dummy(Spacing.SectionSpacing);
                }

                // Draw links if set
                if (subsection.Links != null)
                {
                    DrawLinks(subsection.Links);
                }

            }
            ImGui.EndChild();
        }

        /// <summary>
        /// Draws the subsection content.
        /// </summary>
        /// <param name="content">The content to draw.</param>
        private static void DrawSubsectionContent(TranslatableString content)
        {
            SiGui.Heading(Strings.Guide_InstanceContent_Content_Heading);
            SiGui.TextWrapped(content.UICurrent);
        }

        /// <summary>
        /// Draws the mechanics table.
        /// </summary>
        /// <param name="mechanics">The mechanics to draw.</param>
        private static void DrawMechanicsTable(InstanceGuideContent.Section.Subsection.MechanicsTableRow[] mechanics)
        {
            SiGui.Heading(Strings.Guide_InstanceContent_Mechanics_Heading);
            if (ImGui.BeginTable("Mechanics", 2, ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn(Strings.Guide_InstanceContent_Mechanics_Name);
                ImGui.TableSetupColumn(Strings.Guide_InstanceContent_Mechanics_Description);
                ImGui.TableHeadersRow();

                foreach (var mechanic in mechanics)
                {
                    ImGui.TableNextColumn();
                    SiGui.TextWrapped(mechanic.Name.UICurrent);
                    ImGui.TableNextColumn();
                    SiGui.TextWrapped(mechanic.Description.UICurrent);
                }

                ImGui.EndTable();
            }
        }

        /// <summary>
        /// Draws the tips.
        /// </summary>
        /// <param name="tips">The tips to draw.</param>
        private static void DrawTips(TranslatableString[] tips)
        {
            SiGui.Heading(Strings.Guide_InstanceContent_Tips_Heading);
            foreach (var tip in tips)
            {
                SiGui.TextWrapped($"• {tip.UICurrent}");
            }
        }

        /// <summary>
        /// The links to draw.
        /// </summary>
        /// <param name="links">The links to draw.</param>
        private static void DrawLinks(string[] links)
        {
            if (SiGui.CollapsingHeader(Strings.Guide_InstanceContent_Links_Heading))
            {
                foreach (var link in links)
                {
                    if (ImGui.Selectable(link))
                    {
                        Util.OpenLink(link);
                    }
                }
            }
        }
    }
}