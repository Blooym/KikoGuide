using System;
using System.Collections.Generic;
using System.Numerics;
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
        internal static void Draw(List<Guide.Section> sections)
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
                                ImGui.TextWrapped("No phase data found for this section.");
                            }

                            else if (ImGui.BeginTabBar("##GuideSectionComponentPhaseTabs", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton))
                            {
                                foreach (var phase in section.Phases)
                                {
                                    var hasContent = false;

                                    if (ImGui.BeginTabItem($"Phase {phase.TitleOverride ?? (section.Phases.IndexOf(phase) + 1).ToString()}"))
                                    {
                                        ImGui.BeginChild($"##GuideSectionComponentPhaseTabsChild#{section.Name}{section.Phases.IndexOf(phase)}");

                                        // Draw strategy.
                                        if (!string.IsNullOrEmpty(phase.Strategy?.Trim()) || !string.IsNullOrEmpty(phase.StrategyShort?.Trim()))
                                        {
                                            Common.TextHeading(TGenerics.Strategy);

                                            if (GuideSectionPresenter.Configuration.Accessiblity.ShortenGuideText && !string.IsNullOrEmpty(phase.StrategyShort?.Trim()))
                                            {
                                                Common.TextWrappedUnformatted(phase.StrategyShort);
                                            }
                                            else if (string.IsNullOrEmpty(phase.Strategy?.Trim()) && !string.IsNullOrEmpty(phase.StrategyShort?.Trim()))
                                            {
                                                Common.TextWrappedUnformatted(phase.StrategyShort);
                                            }
                                            else
                                            {
                                                Common.TextWrappedUnformatted(phase.Strategy ?? string.Empty);
                                            }
                                            hasContent = true;
                                        }

                                        // Draw mechanics
                                        if (phase.Mechanics != null)
                                        {
                                            ImGui.Dummy(new Vector2(0, 5));
                                            Common.TextHeading(TGenerics.Mechanics);
                                            MechanicTableComponent.Draw(phase.Mechanics);
                                            hasContent = true;
                                        }

                                        // Draw tips
                                        if (phase.Tips != null && phase.Tips.Count > 0)
                                        {
                                            ImGui.Dummy(new Vector2(0, 5));
                                            Common.TextHeading(TGenerics.Tips);
                                            foreach (var tip in phase.Tips)
                                            {
                                                if (string.IsNullOrEmpty(tip.Text?.Trim()) && string.IsNullOrEmpty(tip.TextShort?.Trim()))
                                                {
                                                    continue;
                                                }

                                                if (GuideSectionPresenter.Configuration.Accessiblity.ShortenGuideText && !string.IsNullOrEmpty(tip.TextShort?.Trim()))
                                                {
                                                    ImGui.TextWrapped($"- {tip.TextShort}");
                                                }
                                                else if (string.IsNullOrEmpty(tip.Text?.Trim()) && !string.IsNullOrEmpty(tip.TextShort?.Trim()))
                                                {
                                                    ImGui.TextWrapped($"- {tip.TextShort}");
                                                }
                                                else
                                                {
                                                    ImGui.TextWrapped($"- {tip.Text}");
                                                }
                                            }

                                            hasContent = true;
                                        }

                                        if (!hasContent)
                                        {
                                            ImGui.TextWrapped(TGuideViewer.NoPhaseInfoAvailable);
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
