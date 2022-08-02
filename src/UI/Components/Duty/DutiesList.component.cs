namespace KikoGuide.UI.Components.Duty;

using ImGuiNET;
using KikoGuide.Enums;
using KikoGuide.Managers;
using System;
using CheapLoc;
using System.Linq;

using System.Collections.Generic;

static class DutyListComponent
{
    public static void Draw(List<Duty> dutyPool, Action<Duty> onDutySelected, string filter = "", int? dutyType = null)
    {
        try
        {
            List<Duty> dutyList;
            if (dutyType != null) dutyList = dutyPool.Where(duty => duty.Type == dutyType).ToList();
            else dutyList = dutyPool;

            // If there are no duties found, display a message and return.
            if (dutyList.Count() == 0) { ImGui.TextDisabled(Loc.Localize("UI.Components.DutiesList.NoDutiesfound", "No duties were found.")); return; }

            // Create a table for each duty, containing its level and name.
            ImGui.BeginTable("DutyList", 2);
            ImGui.TableSetupColumn(Loc.Localize("Generics.Level", "Level"), ImGuiTableColumnFlags.WidthFixed, 45);
            ImGui.TableSetupColumn(Loc.Localize("Generics.Duty", "Duty"));
            ImGui.TableHeadersRow();

            // Fetch all duties for this duty type and draw them.
            foreach (var duty in dutyList)
            {
                // Do not show the duty if it isn't unlocked or isn't part of the filter.
                if (!duty.IsUnlocked()) continue;
                if (!duty.Name.ToLower().Contains(filter.ToLower())) continue;

                // Ad the level and duty name to the list.
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text(duty.Level.ToString());
                ImGui.TableNextColumn();

                // If this duty does not have any data or is unsupported, draw it as such and move on.
                var name = _generateDutyName(duty);
                if (!_hasDutyData(duty)) { _noDataDuty(name); continue; }
                if (!duty.IsSupported()) { _unsupportedDuty(name); continue; }

                // Draw a selectable text for this duty and trigger the onDutySelected event when clicked.
                if (ImGui.Selectable(name, false, ImGuiSelectableFlags.AllowDoubleClick)) onDutySelected(duty);

                // If the player is inside this duty, add some text next to it.
                if (duty == DutyManager.GetPlayerDuty()) Badges.Custom(Colours.Green, Loc.Localize("Generics.InDuty", "In Duty"));
            }

            ImGui.EndTable();
        }
        catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
    }


    /// <summary> Draws an unsupported duty </summary>
    private static void _unsupportedDuty(string name)
    {
        ImGui.TextDisabled(name);
        Badges.Questionmark(Loc.Localize("UI.Components.DutiesList.UpdateRequired", "Cannot display duty as it is not supported on this version."));
    }

    /// <summary> Draws a duty with no data. </summary>
    private static void _noDataDuty(string name)
    {
        ImGui.TextColored(Colours.Red, name);
        Badges.Questionmark(String.Format(Loc.Localize("UI.Components.DutiesList.NoBossData", "No guide available for {0}."), name));
    }

    /// <summary>  Checks to see if the duty has data that can be displayed. </summary>
    private static bool _hasDutyData(Duty duty) => duty.Bosses != null && duty.Bosses.Count != 0;

    /// <summary> Generates a name for the duty, including the difficulty if its not normal </summary>
    private static string _generateDutyName(Duty duty)
    {
        if (duty.Difficulty != (int)DutyDifficulty.Normal) return $"{duty.Name} ({Enum.GetName(typeof(DutyDifficulty), duty.Difficulty)})";
        else return duty.Name;
    }
}