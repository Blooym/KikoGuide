using System;
using System.Collections.Generic;
using ImGuiNET;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.MechanicTable;

namespace KikoGuide.UI.ImGuiFullComponents.GuideSection
{
    /// <summary>
    ///     A component for displaying guide section information.
    /// </summary>
    internal static class GuideSectionComponent
    {
        /// <summary>
        ///     Draws the guide sections for the given guide.
        /// </summary>
        /// <param name="sections"></param>
        public static void Draw(List<Guide.Section> sections)
        {
            try
            {
                if (ImGui.BeginTabBar("##GuideSectionComponentTabs", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
                {
                    foreach (var section in sections)
                    {
                        if (ImGui.BeginTabItem(section.Name))
                        {
                            if (section.Phases == null || section.Phases.Count == 0)
                            {
                                Colours.TextWrappedColoured(Colours.Warning, "No phase data found for this section.");
                            }
                            else if (ImGui.BeginTabBar("##GuideSectionComponentPhaseTabs", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
                            {
                                foreach (var phase in section.Phases)
                                {
                                    if (ImGui.BeginTabItem($"Phase {phase.TitleOverride ?? (section.Phases.IndexOf(phase) + 1).ToString()}"))
                                    {
                                        ImGui.BeginChild($"##GuideSectionComponentPhaseTabsChild#{section.Name}{section.Phases.IndexOf(phase)}");

                                        Common.TextHeading(TGenerics.Strategy);
                                        if (
                                            phase.StrategyShort != null && phase.Strategy != string.Empty &&
                                            GuideSectionPresenter.Configuration.Accessiblity.ShortenGuideText
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
                                            Common.TextHeading(TGenerics.Mechanics);
                                            MechanicTableComponent.Draw(phase.Mechanics);
                                        }

                                        ImGui.EndChild();
                                        ImGui.EndTabItem();
                                    }
                                }
                                ImGui.EndTabBar();
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
