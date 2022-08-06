namespace KikoGuide.UI.Components.Duty;

using KikoGuide.UI.Components;
using System;
using ImGuiNET;
using CheapLoc;
using KikoGuide.Types;

static class DutyHeadingComponent
{
    public static void Draw(Duty duty)
    {
        try
        {
            var dutyName = duty.Name;
            if (duty.Difficulty != (int)DutyDifficulty.Normal) dutyName = $"{duty.Name} ({Enum.GetName(typeof(DutyDifficulty), duty.Difficulty)})";
            Common.TextHeading(String.Format(Loc.Localize("UI.Components.DutyHeading.Title", "Duty: {0}"), dutyName));
            ImGui.TextWrapped($"This duty has {duty.Bosses?.Count ?? 0} bosses and is apart of the {Enum.GetName(typeof(DutyExpansion), duty.Expansion)} expansion.");
        }
        catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
    }
}