using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Attributes;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;

namespace KikoGuide.UI.ImGuiFullComponents.MechanicHiderCombo
{
    public static class MechanicHiderComboComponent
    {
        private static string hiddenSectionFilter = string.Empty;
        public static void Draw()
        {
            var disabledMechanic = MechanicHiderComboPresenter.Configuration.Display.DisabledMechanics;
            if (ImGui.BeginCombo("##MechanicHiderCombo", $"Hidden Mechanic Types: {disabledMechanic.Count}"))
            {
                ImGui.SetNextItemWidth(-1);
                ImGui.InputTextWithHint("##MechanicHiderComboSearch", "Filter types...", ref hiddenSectionFilter, 100);
                ImGui.Separator();
                foreach (var mechanicType in Enum.GetValues(typeof(DutyMechanics)).Cast<DutyMechanics>())
                {
                    if (hiddenSectionFilter != string.Empty && !mechanicType.ToString().ToLower().Contains(hiddenSectionFilter.ToLower()))
                    {
                        continue;
                    }

                    if (ImGui.Selectable(AttributeExtensions.GetNameAttribute(mechanicType), disabledMechanic?.Contains(mechanicType) ?? false))
                    {
                        disabledMechanic = disabledMechanic?.Contains(mechanicType) ?? false
                            ? disabledMechanic.Where(t => t != mechanicType).ToList()
                            : disabledMechanic?.Append(mechanicType).ToList() ?? new List<DutyMechanics>() { mechanicType };
                        MechanicHiderComboPresenter.Configuration.Display.DisabledMechanics = disabledMechanic;
                        MechanicHiderComboPresenter.Configuration.Save();
                    }
                    Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute(mechanicType));
                }
                ImGui.EndCombo();
            }
        }
    }
}