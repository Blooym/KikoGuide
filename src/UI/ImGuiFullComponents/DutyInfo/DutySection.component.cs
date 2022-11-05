using System;
using System.Collections.Generic;
using ImGuiNET;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.MechanicTable;

namespace KikoGuide.UI.ImGuiFullComponents.DutyInfo
{
    /// <summary>
    ///     A component for displaying duty section information.
    /// </summary>
    internal static class DutyInfoComponent
    {
        /// <summary> 
        ///     Draws the duty sections for the given duty.
        /// </summary>
        /// <param name="section"> The sections to draw information for. </param>
        public static void Draw(List<Duty.Section> sections)
        {
            try
            {
                if (ImGui.BeginTabBar("##DutySectionComponentTabs", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
                {
                    foreach (var section in sections)
                    {
                        if (ImGui.BeginTabItem(section.Name))
                        {
                            if (section.Phases == null || section.Phases.Count == 0)
                            {
                                Colours.TextWrappedColoured(Colours.Warning, "No phase data found for this section.");
                            }
                            else
                            {
                                if (ImGui.BeginTabBar("##DutySectionComponentPhaseTabs", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
                                {
                                    foreach (var phase in section.Phases)
                                    {
                                        if (ImGui.BeginTabItem($"Phase {(phase.TitleOverride != null ? phase.TitleOverride : section.Phases.IndexOf(phase) + 1)}"))
                                        {

                                            if (
                                                phase.StrategyShort != null && phase.Strategy != string.Empty &&
                                                DutyInfoPresenter.Configuration.Accessiblity.ShortenGuideText
                                                )
                                            {
                                                ImGui.TextWrapped(phase.StrategyShort);
                                            }
                                            else
                                            {
                                                ImGui.TextWrapped(phase.Strategy);
                                            }

                                            if (phase.Mechanics != null)
                                            {
                                                MechanicTableComponent.Draw(phase.Mechanics);
                                            }

                                            ImGui.EndTabItem();
                                        }
                                    }
                                    ImGui.EndTabBar();
                                }
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