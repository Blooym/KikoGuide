using System;
using ImGuiNET;
using KikoGuide.Enums;
using KikoGuide.GuideSystem;

namespace KikoGuide.UserInterface.Windows.GuideList.TableParts
{
    internal static class Listings
    {
        /// <summary>
        /// Draw the guide listings.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(GuideListLogic logic)
        {
            if (ImGui.BeginTabBar("##GuideListTabs"))
            {
                foreach (var contentType in Enum.GetValues<ContentTypeModified>())
                {
                    DrawContentTypeListings(logic, contentType);
                }
                ImGui.EndTabBar();
            }
        }

        /// <summary>
        /// Draw all guides that belong to the given content type.
        /// </summary>
        /// <param name="contentType"></param>
        private static void DrawContentTypeListings(GuideListLogic logic, ContentTypeModified contentType)
        {
            // If there are no guides for this ContentType, don't draw  the tab
            if (GuideListLogic.GuidesForContentType(contentType) == 0)
            {
                return;
            }

            // Get guides for this duty type and display them
            var guides = logic.GetFilteredGuides(contentType);
            ImGui.BeginDisabled(guides.Count == 0);
            if (ImGui.BeginTabItem(contentType.ToString()))
            {
                if (ImGui.BeginChild($"ContentTypeTab_{contentType}"))
                {
                    // No guides found
                    if (guides.Count == 0)
                    {
                        ImGui.Text("No guides found for filter criteria.");
                    }
                    // Draw guides
                    else
                    {
                        if (ImGui.BeginTable($"ContentTypeTable_{contentType}", 1, ImGuiTableFlags.RowBg))
                        {
                            foreach (var guide in guides)
                            {
                                ImGui.TableNextColumn();
                                DrawGuideSelectable(logic, guide);
                            }
                            ImGui.EndTable();
                        }
                    }
                    ImGui.EndChild();
                }
                ImGui.EndTabItem();
            }
            ImGui.EndDisabled();
        }

        /// <summary>
        /// Draw a selectable guide.
        /// </summary>
        /// <param name="guide"></param>
        private static void DrawGuideSelectable(GuideListLogic _, GuideBase guide)
        {
            if (!guide.IsUnlocked)
            {
                return;
            }

            if (ImGui.Selectable($"{guide.Name}##{guide.Id}", GuideListLogic.CurrentGuide == guide))
            {
                GuideListLogic.SetCurrentGuide(guide);
            }
        }
    }
}