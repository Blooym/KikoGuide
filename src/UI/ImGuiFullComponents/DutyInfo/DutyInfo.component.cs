namespace KikoGuide.UI.ImGuiFullComponents.DutyInfo
{
    using System;
    using System.Linq;
    using ImGuiNET;
    using KikoGuide.Base;
    using KikoGuide.Types;
    using KikoGuide.UI.ImGuiBasicComponents;

    /// <summary>
    ///     A component for displaying duty information.
    /// </summary>
    static class DutyInfoComponent
    {
        private static DutyInfoPresenter _presenter = new DutyInfoPresenter();

        /// <summary> 
        ///     Draws the duty info window.
        /// </summary>
        /// <param name="duty"> The duty to draw information for. </param>
        public static void Draw(Duty duty)
        {
            try
            {
                Common.TextHeading(TStrings.DutyHeadingTitle(duty.GetCanonicalName()));

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