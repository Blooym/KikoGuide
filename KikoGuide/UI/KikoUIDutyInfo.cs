namespace KikoGuide.UI;

using System;
using System.Numerics;
using System.Linq;

using ImGuiNET;
using CheapLoc;

using KikoGuide.Base;
using KikoGuide.Enums;

internal class KikoUIDutyInfo : IDisposable
{

    private protected Configuration? _configuration;

    // <summary>
    // Instantiates a new KikoUIFrame.
    // </summary>
    public KikoUIDutyInfo(Configuration configuration)
    {
        this._configuration = configuration;
    }


    // <summary>
    // Disposes of the KikoUIFrame and any resources it uses.
    // </summary>
    public void Dispose()
    { }


    // <summary>
    // Draws all UI elements.
    // </summary>
    public void Draw()
    {
        DrawFrameWindow();
    }


    // <summary>
    // Draws the frame window.
    // </summary>
    private void DrawFrameWindow()
    {

        if (KikoUIState.SelectedDuty == null || !KikoUIState.dutyInfoVisible) return;
        if (KikoUIState.SelectedDuty.Bosses == null || KikoUIState.SelectedDuty.UpdateRequired) return;

        var selectedDuty = KikoUIState.SelectedDuty;
        var disabledMechanics = this._configuration?.hiddenMechanics;
        var shortMode = this._configuration?.shortenStrategies;

        ImGui.SetNextWindowSizeConstraints(new Vector2(400, 350), new Vector2(800, 800));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.DutyInfo.Title", "{0} - Duty Information"), PStrings.PluginName), ref KikoUIState.dutyInfoVisible))
        {
            try
            {
                // Set the duty name to be just the duty name, or the duty name and difficulty if its not normal difficulty.
                var dutyName = selectedDuty.Name;
                if (selectedDuty.Difficulty != (int)DutyDifficulty.Normal) dutyName = $"{selectedDuty.Name} ({Enum.GetName(typeof(DutyDifficulty), selectedDuty.Difficulty)})";
                ImGui.TextWrapped(String.Format(Loc.Localize("UI.DutyInfo.DutyText", "Duty: {0}"), dutyName));
                ImGui.NewLine();

                // For each boss within this duty, create a collapsible header for it.
                foreach (var boss in selectedDuty.Bosses)
                {
                    if (ImGui.CollapsingHeader(boss.Name, ImGuiTreeNodeFlags.DefaultOpen))
                    {
                        // Add the core strategy for this boss.
                        if (shortMode == true && boss.TLDR != null) ImGui.TextWrapped(boss.TLDR);
                        else ImGui.TextWrapped(boss.Strategy);
                        ImGui.NewLine();

                        var keyMechanics = boss.KeyMechanics;
                        if (keyMechanics == null || keyMechanics.All(x => disabledMechanics?.Contains(x.Type) == true)) continue;

                        // Create a table for key mechanics of this boss that are enabled.
                        ImGui.BeginTable("Boss Mechanics", 3, ImGuiTableFlags.Sortable | ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable);
                        ImGui.TableSetupColumn(Loc.Localize("UI.DutyInfo.MechanicName", "Mechanic"), ImGuiTableColumnFlags.WidthStretch, 0.3f);
                        ImGui.TableSetupColumn(Loc.Localize("UI.DutyInfo.MechanicDescription", "Description"), ImGuiTableColumnFlags.WidthStretch, 0.6f);
                        ImGui.TableSetupColumn(Loc.Localize("UI.DutyInfo.MechanicType", "Type"), ImGuiTableColumnFlags.WidthStretch, 0.2f);
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
                            ImGui.Text(Enum.GetName(typeof(Mechanics), mechanic.Type));
                        }
                        ImGui.EndTable();
                        ImGui.NewLine();
                    }
                }
                ImGui.End();
            }
            catch (Exception e)
            {
                ImGui.TextWrapped(Loc.Localize("UI.DutyInfo.Error", "An error occurred while trying to display duty information."));
                ImGui.TextWrapped($"\nError: {e.Message}");
                ImGui.End();
            }
        }
    }
}
