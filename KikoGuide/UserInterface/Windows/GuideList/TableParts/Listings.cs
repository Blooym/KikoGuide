using System;
using Dalamud.Interface.Colors;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.Enums;
using KikoGuide.GuideHandling;

namespace KikoGuide.UserInterface.Windows.GuideList.TableParts
{
    internal static class Listings
    {
        /// <summary>
        ///     Draw the guide listings.
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
        ///     Draw all guides that belong to the given content type.
        /// </summary>
        /// <param name="contentType"></param>
        private static void DrawContentTypeListings(GuideListLogic logic, ContentTypeModified contentType)
        {
            // If there are no guides for this ContentType, don't draw the tab
            var amount = GuideListLogic.GuidesForContentType(contentType);
            if (amount == 0)
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
                    foreach (var guide in guides)
                    {
                        DrawGuideSelectable(guide);
                    }
                    ImGui.EndChild();
                }
                ImGui.EndTabItem();
            }
            ImGui.EndDisabled();
        }

        /// <summary>
        ///     Draw a selectable guide.
        /// </summary>
        /// <param name="guide"></param>
        private static void DrawGuideSelectable(Guide guide)
        {
            if (Services.GuideManager.CurrentGuide == guide)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedGreen);

                if (ImGui.Selectable($"{guide.Name}##{guide.Id}"))
                {
                    guide.SetCurrent(true);
                }

                ImGui.PopStyleColor();
            }
            else if (ImGui.Selectable($"{guide.Name}##{guide.Id}"))
            {
                guide.SetCurrent(true);
            }
        }
    }
}