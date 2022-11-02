namespace KikoGuide.UI.ImGuiFullComponents.DutyInfo
{
    using System;
    using System.Linq;
    using ImGuiNET;
    using KikoGuide.Base;
    using KikoGuide.Types;
    using KikoGuide.UI.ImGuiBasicComponents;


    /// <summary>
    ///     A subcomponent for displaying a duty's boss.
    /// </summary>
    public static class DutyPhaseComponent
    {
        /// <summary> 
        ///     Draws information about a duty boss.
        /// </summary>
        /// <param name="boss"> The boss to draw information for. </param>
        public static void Draw(Duty.Section.Phase phase)
        {
            try
            {
                var disabledMechanics = PluginService.Configuration?.Display?.DisabledMechanics;
                var shortMode = PluginService.Configuration?.Accessiblity.ShortenGuideText ?? false;

                if (shortMode == true && phase.TLDR != null) ImGui.TextWrapped(phase.TLDR);
                else ImGui.TextWrapped(phase.Strategy);
                ImGui.NewLine();

                var keyMechanics = phase.Mechanics;
                if (keyMechanics == null || keyMechanics.All(x => disabledMechanics?.Contains((DutyMechanics)x.Type) ?? false)) return;

                ImGui.BeginTable("Boss Mechanics", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable);
                ImGui.TableSetupColumn(TStrings.Mechanic, ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn(TStrings.Description, ImGuiTableColumnFlags.WidthStretch, 0.6f);
                ImGui.TableSetupColumn(TStrings.Type, ImGuiTableColumnFlags.WidthStretch, 0.2f);
                ImGui.TableHeadersRow();

                foreach (var mechanic in keyMechanics)
                {
                    if (disabledMechanics?.Contains((DutyMechanics)mechanic.Type) == true) continue;
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text(mechanic.Name);
                    ImGui.TableNextColumn();
                    if (shortMode == true && mechanic.ShortDesc != null) ImGui.TextWrapped(mechanic.ShortDesc);
                    else ImGui.TextWrapped(mechanic.LongDesc);
                    ImGui.TableNextColumn();
                    ImGui.Text(Enum.GetName(typeof(DutyMechanics), mechanic.Type));
                }

                ImGui.EndTable();
                ImGui.NewLine();
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }
    }
}