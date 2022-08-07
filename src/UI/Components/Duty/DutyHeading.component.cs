namespace KikoGuide.UI.Components.Duty;

using KikoGuide.UI.Components;
using System;
using ImGuiNET;
using KikoGuide.Base;
using KikoGuide.Types;

static class DutyHeadingComponent
{
    public static void Draw(Duty duty)
    {
        try
        {
            var dutyName = duty.Name;
            if (duty.Difficulty != (int)DutyDifficulty.Normal) dutyName = $"{duty.Name} ({Enum.GetName(typeof(DutyDifficulty), duty.Difficulty)})";
            Common.TextHeading(TStrings.DutyHeadingTitle(dutyName));
        }
        catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
    }
}