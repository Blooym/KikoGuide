using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Enums;
using KikoGuide.GuideSystem;

namespace KikoGuide.UserInterface.Windows.GuideList.TableParts
{
    internal static class Listings
    {
        private static readonly unsafe ImGuiListClipperPtr Clipper = new(ImGuiNative.ImGuiListClipper_ImGuiListClipper());

        /// <summary>
        /// Draw the guide listings.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(GuideListLogic logic)
        {
            if (ImGui.BeginTabBar("##GuideListTabs", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
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
                if (ImGui.BeginChild("ContentTypeTab_" + contentType))
                {
                    if (guides.Count == 0)
                    {
                        ImGui.Text("No guides found for filter criteria.");
                    }
                    else
                    {
                        DrawGuideTable(logic, guides);
                    }
                    ImGui.EndChild();
                }
                ImGui.EndTabItem();
            }
            ImGui.EndDisabled();
        }

        /// <summary>
        /// Draw a table of guides and clips it for performance.
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="guides"></param>
        private static void DrawGuideTable(GuideListLogic logic, HashSet<GuideBase> guides)
        {
            Clipper.Begin(guides.Count, ImGui.GetFontSize() + ImGui.GetStyle().FramePadding.Y);

            if (ImGui.BeginTable("GuideTable", 1, ImGuiTableFlags.RowBg))
            {
                while (Clipper.Step())
                {
                    for (var i = Clipper.DisplayStart; i < Clipper.DisplayEnd; i++)
                    {
                        var guide = guides.ElementAt(i);
                        ImGui.TableNextColumn();
                        DrawGuideSelectable(logic, guide);
                    }
                }
                ImGui.EndTable();
            }

            Clipper.End();
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

            if (ImGui.Selectable(guide.Name + "##" + guide.Id, GuideListLogic.CurrentGuide == guide))
            {
                GuideListLogic.OpenGuide(guide);
            }
        }
    }
}