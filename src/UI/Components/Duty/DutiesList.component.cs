namespace KikoGuide.UI.Components.Duty;

using ImGuiNET;
using KikoGuide.Base;
using KikoGuide.Types;
using KikoGuide.Managers;
using System;
using System.Linq;
using System.Collections.Generic;

public static class DutyListComponent
{
    public static void Draw(List<Duty> dutyPool, Action<Duty> onDutySelected, string filter = "", int? dutyType = null)
    {
        try
        {
            List<Duty> dutyList;
            if (dutyType != null) dutyList = dutyPool.Where(duty => duty.Type == dutyType).ToList();
            else dutyList = dutyPool;

            // If there are no duties found, display a message and return.
            if (dutyList.Count() == 0) { ImGui.TextDisabled(TStrings.DutyListNoneFound); return; }

            var playerDuty = DutyManager.GetPlayerDuty();

            // Create a table for each duty, containing its level and name.
            ImGui.BeginTable("DutyList", 2);
            ImGui.TableSetupColumn(TStrings.Level, ImGuiTableColumnFlags.WidthFixed, 45);
            ImGui.TableSetupColumn(TStrings.Duty);
            ImGui.TableHeadersRow();

            // Fetch all duties for this duty type and draw them.
            foreach (var duty in dutyList)
            {
                // Do not show the duty if it isn't unlocked or isn't part of the filter.
                if (!DutyManager.IsUnlocked(duty)) continue;
                if (!duty.Name.ToLower().Contains(filter.ToLower())) continue;

                // Ad the level and duty name to the list.
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text(duty.Level.ToString());
                ImGui.TableNextColumn();

                // If this duty does not have any data or is unsupported, draw it as such and move on.
                if (!_hasDutyData(duty)) { _noDataDuty(duty.GetCanonicalName()); continue; }
                if (!duty.IsSupported()) { _unsupportedDuty(duty.GetCanonicalName()); continue; }

                // Draw a selectable text for this duty and trigger the onDutySelected event when clicked.
                if (ImGui.Selectable(duty.GetCanonicalName(), false, ImGuiSelectableFlags.AllowDoubleClick)) onDutySelected(duty);

                // If the player is inside this duty, add some text next to it.
                if (duty == playerDuty) Badges.Custom(Colours.Green, TStrings.InDuty);
            }

            ImGui.EndTable();
        }
        catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
    }


    /// <summary> Draws an unsupported duty </summary>
    private static void _unsupportedDuty(string name)
    {
        ImGui.TextDisabled(name);
        Badges.Questionmark(TStrings.DutyListNeedsUpdate);
    }

    /// <summary> Draws a duty with no data. </summary>
    private static void _noDataDuty(string name)
    {
        ImGui.TextColored(Colours.Red, name);
        Badges.Questionmark(TStrings.DutyListNoGuide(name));
    }

    /// <summary>  Checks to see if the duty has data that can be displayed. </summary>
    private static bool _hasDutyData(Duty duty) => duty.Bosses != null && duty.Bosses.Count != 0;
}