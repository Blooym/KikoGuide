using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;

namespace KikoGuide.UI.ImGuiFullComponents.MechanicTable
{
    public static class MechanicTableComponent
    {
        public static void Draw(List<Duty.Section.Phase.Mechanic> mechanics)
        {
            try
            {
                List<DutyMechanics>? disabledMechanics = MechanicTablePresenter.Configuration.Display.DisabledMechanics;
                bool shortMode = MechanicTablePresenter.Configuration.Accessiblity.ShortenGuideText;

                if (mechanics.All(x => disabledMechanics?.Contains((DutyMechanics)x.Type) ?? false))
                {
                    goto End;
                }

                _ = ImGui.BeginTable("##MechanicTableComponentMechTable", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable);
                ImGui.TableSetupColumn(TStrings.Mechanic, ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn(TStrings.Description, ImGuiTableColumnFlags.WidthStretch, 0.6f);
                ImGui.TableSetupColumn(TStrings.Type, ImGuiTableColumnFlags.WidthStretch, 0.2f);
                ImGui.TableHeadersRow();
                foreach (Duty.Section.Phase.Mechanic mechanic in mechanics)
                {
                    if (disabledMechanics?.Contains((DutyMechanics)mechanic.Type) == true)
                    {
                        continue;
                    }

                    ImGui.TableNextRow();
                    _ = ImGui.TableNextColumn();
                    ImGui.Text(mechanic.Name);
                    _ = ImGui.TableNextColumn();
                    if (shortMode && mechanic.ShortDesc != null)
                    {
                        ImGui.TextWrapped(mechanic.Description);
                    }
                    else
                    {
                        ImGui.TextWrapped(mechanic.ShortDesc);
                    }

                    _ = ImGui.TableNextColumn();
                    ImGui.Text(LoCExtensions.GetLocalizedName((DutyMechanics)mechanic.Type));
                    Common.AddTooltip(LoCExtensions.GetLocalizedDescription((DutyMechanics)mechanic.Type));
                }

                ImGui.EndTable();

            End:;
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }
    }
}
