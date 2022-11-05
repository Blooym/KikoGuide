using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Attributes;
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
                var disabledMechanics = MechanicTablePresenter.Configuration.Display.DisabledMechanics;
                var shortMode = MechanicTablePresenter.Configuration.Accessiblity.ShortenGuideText;

                if (!mechanics.All(x => disabledMechanics?.Contains((DutyMechanics)x.Type) ?? false))
                {

                    if (ImGui.BeginTable("##MechanicTableComponentMechTable", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable))
                    {
                        ImGui.TableSetupColumn(label: TStrings.Mechanic);
                        ImGui.TableSetupColumn(label: TStrings.Description);
                        ImGui.TableSetupColumn(TStrings.Type);
                        ImGui.TableHeadersRow();
                        foreach (var mechanic in mechanics)
                        {
                            if (disabledMechanics?.Contains((DutyMechanics)mechanic.Type) == true)
                            {
                                continue;
                            }

                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();
                            ImGui.Text(mechanic.Name);
                            ImGui.TableNextColumn();
                            ImGui.TextWrapped((shortMode && mechanic.ShortDesc != null) ? mechanic.ShortDesc : mechanic.Description);
                            ImGui.TableNextColumn();
                            ImGui.Text(AttributeExtensions.GetNameAttribute((DutyMechanics)mechanic.Type));
                            Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((DutyMechanics)mechanic.Type));
                        }

                        ImGui.EndTable();
                    }
                }
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }
    }
}
