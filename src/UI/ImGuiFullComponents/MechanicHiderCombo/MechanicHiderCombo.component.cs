using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;

namespace KikoGuide.UI.ImGuiFullComponents.MechanicHiderCombo
{
    public static class MechanicHiderComboComponent
    {
        private static string _hiddenSectionFilter = string.Empty;
        public static void Draw()
        {
            List<DutyMechanics>? disabledMechanic = MechanicHiderComboPresenter.Configuration.Display.DisabledMechanics;
            if (ImGui.BeginCombo("##MechanicHiderCombo", $"Hidden Mechanic Types: {disabledMechanic.Count}"))
            {
                ImGui.SetNextItemWidth(-1);
                _ = ImGui.InputTextWithHint("##MechanicHiderComboSearch", "Filter types...", ref _hiddenSectionFilter, 100);
                ImGui.Separator();
                foreach (DutyMechanics mechanicType in Enum.GetValues(typeof(DutyMechanics)).Cast<DutyMechanics>())
                {
                    if (_hiddenSectionFilter != string.Empty && !mechanicType.ToString().ToLower().Contains(_hiddenSectionFilter.ToLower()))
                    {
                        continue;
                    }

                    if (ImGui.Selectable(LoCExtensions.GetLocalizedName(mechanicType), disabledMechanic?.Contains(mechanicType) ?? false))
                    {
                        disabledMechanic = disabledMechanic?.Contains(mechanicType) ?? false
                            ? disabledMechanic.Where(t => t != mechanicType).ToList()
                            : disabledMechanic?.Append(mechanicType).ToList() ?? new List<DutyMechanics>() { mechanicType };
                        MechanicHiderComboPresenter.Configuration.Display.DisabledMechanics = disabledMechanic;
                        MechanicHiderComboPresenter.Configuration.Save();
                    }
                    Common.AddTooltip(LoCExtensions.GetLocalizedDescription(mechanicType));
                }
                ImGui.EndCombo();
            }
        }
    }
}