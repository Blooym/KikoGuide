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
    internal static class GuideListTableComponent
    {
        /// <summary>
        ///     Draws the guide list with the given guide pools.
        /// </summary>
        /// <param name="guidePool"> The guide pool to draw information for. </param>
        /// <param name="onSelected"> The action to call when an item is selected. </param>
        /// <param name="searchFilter"> The search filter to use when drawing the guide list, compares against the guide name. </param>
        /// <param name="dutyType"> The duty type to show listings for, or null to show all. </param>
        internal static void Draw(List<Guide> guidePool, Action<Guide> onSelected, string searchFilter = "", DutyType? dutyType = null)
        {
            try
            {
                var guideList = dutyType != null ? guidePool.Where(duty => duty.Type == dutyType).ToList() : guidePool;

                // No guides found in the guideList, show a message and return.
                if (guideList.Count == 0)
                {
                    ImGui.TextDisabled(TGuideListTable.NoneFoundForType);
                    return;
                }

                // No guides unlocked for this duty type, show a message and return.
                if (!GuideListTablePresenter.HasAnyGuideUnlocked(guideList))
                {
                    ImGui.TextDisabled(TGuideListTable.NoGuidesUnlocked);
                    return;
                }

                // No guides found for the search filter, show a message and return.
                if (searchFilter != string.Empty && !GuideListTablePresenter.GuideExistsForSearch(guideList, searchFilter))
                {
                    ImGui.TextDisabled(TGuideListTable.NoGuidesFoundForSearch);
                    return;
                }

                // Create a table for the guides to be drawn in.
                if (ImGui.BeginTable("##GuideListTable", 2, ImGuiTableFlags.ScrollY))
                {
                    ImGui.TableSetupScrollFreeze(0, 1);
                    ImGui.TableSetupColumn(TGenerics.Level, ImGuiTableColumnFlags.WidthFixed, 45);
                    ImGui.TableSetupColumn(TGenerics.Guide);
                    ImGui.TableHeadersRow();

                    // Fetch all guides in the guideList and then sort them by level and apply the search filter.
                    foreach (var guide in guideList.OrderBy(d => d.Level).Where(g => g.Name.Contains(searchFilter, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Do not show the guide if it is set to be hidden.
                        if (guide.IsHidden())
                        {
                            continue;
                        }

                        // Do not show the guide if it isn't unlocked and the user has the setting enabled.
                        if (!guide.IsUnlocked() && GuideListTablePresenter.Configuration.Display.HideLockedGuides)
                        {
                            continue;
                        }

                        // Go to the next row in the table and add a the guide.
                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();
                        ImGui.Text(guide.Level.ToString());
                        ImGui.TableNextColumn();

                        // If this guide is unsupported by this plugin version, draw it as disabled.
                        if (!guide.IsSupported())
                        {
                            ImGui.TextDisabled(guide.GetCanonicalName());
                            Common.AddTooltip(TGuideListTable.UnsupportedGuide(guide.GetCanonicalName()));
                            continue;
                        }

                        // If this guide does not have any data, draw it as disabled.
                        if (!GuideListTablePresenter.HasGuideData(guide))
                        {
                            ImGui.TextDisabled(guide.GetCanonicalName());
                            Common.AddTooltip(TGuideListTable.NoGuideData(guide.GetCanonicalName()));
                            continue;
                        }

                        // Draw a selectable button that calls the onSelected action when clicked.
                        if (ImGui.Selectable(guide.GetCanonicalName(), false, ImGuiSelectableFlags.AllowDoubleClick))
                        {
                            onSelected(guide);
                        }

                        // If the player is inside the guide territory(s), add a tooltip.
                        if (guide == GuideListTablePresenter.GetGuideForPlayerTerritory())
                        {
                            Badges.Custom(Colours.Green, TGenerics.InTerritory);
                        }
                    }
                    ImGui.EndTable();
                }
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }
    }
}
