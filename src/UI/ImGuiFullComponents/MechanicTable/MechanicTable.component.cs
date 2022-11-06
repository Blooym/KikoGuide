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
        public static void Draw(List<Guide.Section.Phase.Mechanic> mechanics)
        {
            try
            {
                var hiddenMechanics = MechanicTablePresenter.Configuration.Display.HiddenMechanics;
                var shortMode = MechanicTablePresenter.Configuration.Accessiblity.ShortenGuideText;

                if (!mechanics.All(x => hiddenMechanics?.Contains((GuideMechanics)x.Type) ?? false))
                {

                    if (ImGui.BeginTable("##MechanicTableComponentMechTable", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable))
                    {
                        ImGui.TableSetupColumn(label: TGenerics.Mechanic);
                        ImGui.TableSetupColumn(label: TGenerics.Description);
                        ImGui.TableSetupColumn(TGenerics.Type);
                        ImGui.TableHeadersRow();
                        foreach (var mechanic in mechanics)
                        {
                            if (hiddenMechanics?.Contains((GuideMechanics)mechanic.Type) == true)
                            {
                                continue;
                            }

                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();
                            ImGui.Text(mechanic.Name);
                            ImGui.TableNextColumn();
                            ImGui.TextWrapped((shortMode && mechanic.ShortDesc != null) ? mechanic.ShortDesc : mechanic.Description);
                            ImGui.TableNextColumn();
                            ImGui.Text(AttributeExtensions.GetNameAttribute((GuideMechanics)mechanic.Type));
                            Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((GuideMechanics)mechanic.Type));
                        }

                        ImGui.EndTable();
                    }
                }
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }
    }
}
