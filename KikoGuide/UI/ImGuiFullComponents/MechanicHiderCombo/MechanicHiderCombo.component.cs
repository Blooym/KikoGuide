using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Attributes;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;

namespace KikoGuide.UI.ImGuiFullComponents.MechanicHiderCombo
{
    internal static class MechanicHiderComboComponent
    {
        private static string hiddenSectionFilter = string.Empty;
        internal static void Draw()
        {
            var disabledMechanic = MechanicHiderComboPresenter.Configuration.Display.HiddenMechanics;
            if (ImGui.BeginCombo("##MechanicHiderCombo", $"Hidden Mechanic Types: {disabledMechanic.Count}"))
            {
                ImGui.SetNextItemWidth(-1);
                ImGui.InputTextWithHint("##MechanicHiderComboSearch", TGenerics.Search, ref hiddenSectionFilter, 100);
                ImGui.Separator();
                foreach (var mechanicType in Enum.GetValues(typeof(GuideMechanics)).Cast<GuideMechanics>())
                {
                    if (hiddenSectionFilter != string.Empty && !mechanicType.ToString().Contains(hiddenSectionFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (ImGui.Selectable(mechanicType.GetNameAttribute(), disabledMechanic?.Contains(mechanicType) ?? false, ImGuiSelectableFlags.DontClosePopups))
                    {
                        disabledMechanic = disabledMechanic?.Contains(mechanicType) ?? false
                            ? disabledMechanic.Where(t => t != mechanicType).ToList()
                            : disabledMechanic?.Append(mechanicType).ToList() ?? new List<GuideMechanics>() { mechanicType };
                        MechanicHiderComboPresenter.Configuration.Display.HiddenMechanics = disabledMechanic;
                        MechanicHiderComboPresenter.Configuration.Save();
                    }
                    Common.AddTooltip(mechanicType.GetDescriptionAttribute());
                }
                ImGui.EndCombo();
            }
        }
    }
}
