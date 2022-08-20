namespace KikoGuide.UI.Components.Duty;

using KikoGuide.UI.Components;
using System;
using System.Linq;
using ImGuiNET;
using KikoGuide.Types;

static class DutyInfoComponent
{
    /// <summary> Draws the duty info window. </summary>
    /// <param name="duty"> The duty to draw information for. </param>
    public static void Draw(Duty duty)
    {
        try
        {
            DutyHeadingComponent.Draw(duty);

            if (ImGui.GetWindowSize().X > 380)
            {
                if (ImGui.BeginTabBar("#Bosses"))
                {
                    foreach (var boss in duty.Bosses ?? Enumerable.Empty<Duty.Boss>())
                    {
                        if (ImGui.BeginTabItem(boss.Name))
                        {
                            DutyBossComponent.Draw(boss);
                            ImGui.EndTabItem();
                        }
                    }
                }

            }
            else
            {
                foreach (var boss in duty.Bosses ?? Enumerable.Empty<Duty.Boss>())
                    if (ImGui.CollapsingHeader(boss.Name)) DutyBossComponent.Draw(boss);
            }

        }
        catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
    }
}