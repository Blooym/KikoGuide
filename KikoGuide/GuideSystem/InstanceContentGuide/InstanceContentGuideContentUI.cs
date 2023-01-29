using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using KikoGuide.DataModels;
using KikoGuide.Resources.Localization;
using Sirensong.DataStructures;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    internal static class InstanceContentGuideContentUI
    {
        private static Vector2 GuideAreaSize => new(0, (ImGui.GetWindowContentRegionMax().Y * 0.75f) - ImGui.GetStyle().WindowPadding.Y);
        private static Vector2 NoteAreaSize => new(0, (ImGui.GetWindowSize().Y * 0.25f) - ImGui.GetStyle().WindowPadding.Y);
        private static Vector2 NoteContentSize => new(-1, ImGui.GetContentRegionAvail().Y * 0.8f);
        private static bool isEditingNote;

        /// <summary>
        /// Draws the guide.
        /// </summary>
        /// <param name="guide">The guide to draw.</param>
        public static void Draw(InstanceContentGuideBase guide)
        {
            // Window too small to display note
            if (ImGui.GetContentRegionAvail().Y < 380)
            {
                if (ImGui.BeginChild("Guide", Vector2.Zero, true))
                {
                    DrawSections(guide);
                    ImGui.EndChild();
                }
            }
            // Window large enough to display note
            else
            {
                if (ImGui.BeginChild("Guide", GuideAreaSize, true))
                {
                    DrawSections(guide);
                    ImGui.EndChild();
                }

                if (ImGui.BeginChild("Notes", NoteAreaSize, true, ImGuiWindowFlags.NoScrollbar))
                {
                    DrawNote(guide.Note);
                    ImGui.EndChild();
                }
            }
        }

        /// <summary>
        /// Draws the guide note.
        /// </summary>
        /// <param name="note">The note to draw.</param>
        private static void DrawNote(Note note)
        {
            SiGui.Heading(Strings.Guide_InstanceContent_Note_Heading);
            var noteContent = note.Content;

            // Not editing note
            if (!isEditingNote)
            {
                if (ImGui.BeginChild("NoteContent", NoteContentSize, false))
                {
                    SiGui.TextWrapped(noteContent);
                    ImGui.EndChild();
                }

                if (ImGui.Button(Strings.Guide_InstanceContent_Note_Edit))
                {
                    isEditingNote = true;
                }
            }

            // Editing note
            else
            {
                if (SiGui.InputTextMultiline("##Note", ref noteContent, 1000, NoteContentSize, true))
                {
                    note.SetContent(noteContent).Save();
                    isEditingNote = false;
                }

                if (ImGui.Button(Strings.Guide_InstanceContent_Note_Save))
                {
                    isEditingNote = false;
                }
            }
        }

        /// <summary>
        /// Draws the sections of the guide.
        /// </summary>
        /// <param name="guide">The guide to draw.</param>
        private static void DrawSections(InstanceContentGuideBase guide)
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
        private static void DrawSection(InstanceContentGuideContent.Section section)
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
        private static void DrawSubsection(InstanceContentGuideContent.Section.Subsection subsection)
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

                ImGui.EndChild();
            }
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
        private static void DrawMechanicsTable(InstanceContentGuideContent.Section.Subsection.MechanicsTableRow[] mechanics)
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
                SiGui.TextWrapped("- " + tip.UICurrent);
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