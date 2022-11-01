namespace KikoGuide.UI.Components.Duty
{
    using KikoGuide.UI.Components;
    using System;
    using System.Linq;
    using ImGuiNET;
    using KikoGuide.Types;

    /// <summary>
    ///     A component for displaying duty information.
    /// </summary>
    static class DutyInfoComponent
    {
        /// <summary> 
        ///     Draws the duty info window.
        /// </summary>
        /// <param name="duty"> The duty to draw information for. </param>
        public static void Draw(Duty duty)
        {
            try
            {
                DutyHeadingComponent.Draw(duty);

                if (ImGui.BeginTabBar("#Bosses", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton | ImGuiTabBarFlags.NoTabListScrollingButtons))
                {
                    foreach (var sect in duty.Sections ?? Enumerable.Empty<Duty.Section>())
                    {
                        if (ImGui.BeginTabItem(sect.Name))
                        {
                            foreach (var phase in sect.Phases ?? Enumerable.Empty<Duty.Section.Phase>())
                            {
                                DutyPhaseComponent.Draw(phase);
                            }
                            ImGui.EndTabItem();
                        }
                    }
                    ImGui.EndTabBar();
                }
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }
    }
}