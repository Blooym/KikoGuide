using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;

namespace KikoGuide.UI.ImGuiFullComponents.GuideListTable
{
    /// <summary>
    ///     A component for displaying a list of guides.
    /// </summary>
    public static class GuideListTableComponent
    {
        /// <summary>
        ///     Draws the guide list.
        /// </summary>
        /// <param name="guidePool"> The guide pool to draw information for. </param>
        /// <param name="onSelected"> The action to call when an item is selected. </param>
        /// <param name="filter"> The filter to use when drawing the guide list, empty to show all. </param>
        /// <param name="dutyType"> The duty type to show listings for, or null to show all. </param>
        public static void Draw(List<Guide> guidePool, Action<Guide> onSelected, string filter = "", DutyType? dutyType = null)
        {
            try
            {
                var guideList = dutyType != null ? guidePool.Where(duty => duty.Type == dutyType).ToList() : guidePool;

                // No guides found in the guideList.
                if (guideList.Count == 0)
                {
                    ImGui.TextDisabled(TGuideListTable.NoneFoundForType);
                    return;
                }

                // Has no guides unlocked in this guideList.
                if (!GuideListTablePresenter.HasAnyGuideUnlocked(guideList))
                {
                    ImGui.TextDisabled(TGuideListTable.NoGuidesUnlocked);
                    return;
                }

                // No guides found for this search in the guideList.
                if (filter != string.Empty && !GuideListTablePresenter.GuideExistsForSearch(guideList, filter))
                {
                    ImGui.TextDisabled(TGuideListTable.NoGuidesFoundForSearch);
                    return;
                }

                // Create a table and add each guide, containing its level and name.
                if (ImGui.BeginTable("##GuideListTable", 2, ImGuiTableFlags.ScrollY))
                {
                    ImGui.TableSetupScrollFreeze(0, 1);
                    ImGui.TableSetupColumn(TGenerics.Level, ImGuiTableColumnFlags.WidthFixed, 45);
                    ImGui.TableSetupColumn(TGenerics.Guide);
                    ImGui.TableHeadersRow();

                    // Fetch all guides for this duty type and draw them.
                    foreach (var guide in guideList.OrderBy(d => d.Level))
                    {
                        // Do not show the guide if it isn't unlocked or isn't part of the filter.
                        if (!guide.IsUnlocked() && GuideListTablePresenter.Configuration.Display.HideLockedGuides)
                        {
                            continue;
                        }

                        if (!guide.Name.ToLower().Contains(filter.ToLower()))
                        {
                            continue;
                        }

                        // Add the level and guide name to the list.
                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();
                        ImGui.Text(guide.Level.ToString());
                        ImGui.TableNextColumn();

                        // If this guide does not have any data or is unsupported, handle it.
                        if (!guide.IsSupported())
                        {
                            OutdatedGuide(guide.GetCanonicalName());
                            continue;
                        }

                        if (!GuideListTablePresenter.HasGuideData(guide))
                        {
                            NoDataGuide(guide.GetCanonicalName());
                            continue;
                        }
                        // Draw a selectable text for this guide and trigger the onSelected event when clicked.
                        if (ImGui.Selectable(guide.GetCanonicalName(), false, ImGuiSelectableFlags.AllowDoubleClick))
                        {
                            onSelected(guide);
                        }

                        // If the player is inside this duty, add some text next to it.
                        if (guide == GuideListTablePresenter.GetGuideForPlayerTerritory())
                        {
                            Badges.Custom(Colours.Green, TGenerics.InDuty);
                        }
                    }
                    ImGui.EndTable();
                }
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }

        /// <summary>
        ///     Draws an outdated guide in the list.
        /// </summary>
        private static void OutdatedGuide(string name)
        {
            ImGui.TextColored(Colours.GuideDisabled, name);
            Common.AddTooltip(TGuideListTable.UnsupportedGuide(name));
        }

        /// <summary>
        ///     Draws a guide with no data.
        /// </summary>
        private static void NoDataGuide(string name)
        {
            ImGui.TextDisabled(name);
            Common.AddTooltip(TGuideListTable.NoGuideData(name));
        }
    }
}