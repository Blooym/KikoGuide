namespace KikoGuide.UI.Components.Duty;

using ImGuiNET;
using System.Linq;
using KikoGuide.Base;
using KikoGuide.Types;
using System;


/// <summary>
///     A subcomponent for displaying a duty's boss.
/// </summary>
public static class DutySectionComponents
{
    /// <summary> 
    ///     Draws information about a duty boss.
    /// </summary>
    /// <param name="boss"> The boss to draw information for. </param>
    public static void Draw(Duty.Section sect)
    {
        try
        {
            var disabledMechanics = PluginService.Configuration?.hiddenMechanics;
            var shortMode = PluginService.Configuration?.shortenStrategies;

            if (shortMode == true && sect.TLDR != null) ImGui.TextWrapped(sect.TLDR);
            else ImGui.TextWrapped(sect.Strategy);
            ImGui.NewLine();

            var keyMechanics = boss.KeyMechanics;
            if (keyMechanics == null || keyMechanics.All(x => disabledMechanics?.Contains(x.Type) == true)) return;

            ImGui.BeginTable("Boss Mechanics", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable);
            ImGui.TableSetupColumn(TStrings.Mechanic, ImGuiTableColumnFlags.WidthStretch, 0.3f);
            ImGui.TableSetupColumn(TStrings.Description, ImGuiTableColumnFlags.WidthStretch, 0.6f);
            ImGui.TableSetupColumn(TStrings.Type, ImGuiTableColumnFlags.WidthStretch, 0.2f);
            ImGui.TableHeadersRow();

            foreach (var mechanic in keyMechanics)
            {
                if (disabledMechanics?.Contains(mechanic.Type) == true) continue;
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text(mechanic.Name);
                ImGui.TableNextColumn();
                if (shortMode == true && mechanic.TLDR != null) ImGui.TextWrapped(mechanic.TLDR);
                else ImGui.TextWrapped(mechanic.Description);
                ImGui.TableNextColumn();
                ImGui.Text(Enum.GetName(typeof(DutyMechanics), mechanic.Type));
            }

            ImGui.EndTable();
            ImGui.NewLine();
        }
        catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
    }
}


