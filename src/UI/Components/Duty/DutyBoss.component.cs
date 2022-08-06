namespace KikoGuide.UI.Components.Duty;

using ImGuiNET;
using CheapLoc;
using System.Linq;
using KikoGuide.Base;
using KikoGuide.Types;
using System;


public static class DutyBossListComponent
{
    public static void Draw(Duty.Boss boss)
    {
        try
        {
            var disabledMechanics = PluginService.Configuration?.hiddenMechanics;
            var shortMode = PluginService.Configuration?.shortenStrategies;

            if (ImGui.CollapsingHeader(boss.Name, ImGuiTreeNodeFlags.DefaultOpen))
            {
                if (shortMode == true && boss.TLDR != null) ImGui.TextWrapped(boss.TLDR);
                else ImGui.TextWrapped(boss.Strategy);
                ImGui.NewLine();

                var keyMechanics = boss.KeyMechanics;
                if (keyMechanics == null || keyMechanics.All(x => disabledMechanics?.Contains(x.Type) == true)) return;

                ImGui.BeginTable("Boss Mechanics", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable);
                ImGui.TableSetupColumn(Loc.Localize("Generics.Mechanic", "Mechanic"), ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn(Loc.Localize("Generics.Description", "Description"), ImGuiTableColumnFlags.WidthStretch, 0.6f);
                ImGui.TableSetupColumn(Loc.Localize("Generics.BossList.Type", "Type"), ImGuiTableColumnFlags.WidthStretch, 0.2f);
                ImGui.TableHeadersRow();

                foreach (var mechanic in keyMechanics)
                {
                    if (disabledMechanics?.Contains(mechanic.Type) == true) continue;
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text(mechanic.Name);
                    ImGui.TableNextColumn();
                    ImGui.TextWrapped(mechanic.Description);
                    ImGui.TableNextColumn();
                    ImGui.Text(Enum.GetName(typeof(DutyMechanics), mechanic.Type));
                }

                ImGui.EndTable();
                ImGui.NewLine();
            }
        }
        catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
    }
}


