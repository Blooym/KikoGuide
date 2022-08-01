namespace KikoGuide.UI.Editor;

using System;
using System.Numerics;
using ImGuiNET;
using CheapLoc;
using Dalamud.Interface.Components;
using Dalamud.Interface;
using KikoGuide.Base;
using KikoGuide.Managers;
using KikoGuide.Enums;

internal class EditorScreen : IDisposable
{
    public EditorPresenter presenter = new EditorPresenter();


    /// <summary>
    ///     Disposes of the Editor screen and any resources it uses.
    /// </summary>
    public void Dispose() { }


    /// <summary>
    ///     Draws all UI elements associated with the Editor screen.
    /// </summary>
    public void Draw() => this.DrawEditorUI();


    /// <summary> 
    ///     Draws the Editor window and sub-components.
    /// </summary>
    private void DrawEditorUI()
    {
        if (!presenter.isVisible) return;

        // Draw the dialog manager as a sub-menu of the editor.
        presenter.dialogManager.Draw();

        // Begin the editor window.
        if (ImGui.Begin(String.Format(Loc.Localize("UI.Editor.Title", "{0} - Duty Editor"), PStrings.pluginName), ref presenter.isVisible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
        {
            // Draw some buttons at the top of the editor.
            this.DrawEditorButtons();

            // Duty Editor panes.
            ImGui.BeginTable("##DutyInfoTable", 2, ImGuiTableFlags.Resizable);
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.BeginChild("##EditorInput");
            this.DrawEditorInput();
            ImGui.EndChild();
            ImGui.TableNextColumn();
            ImGui.BeginChild("##EditorPreview");
            this.DrawEditorPreview();
            ImGui.EndChild();
            ImGui.EndTable();
        }
    }


    /// <summary>
    ///     Draws the buttons at the top of the editor.
    /// </summary>
    private void DrawEditorButtons()
    {
        if (ImGuiComponents.IconButton(FontAwesomeIcon.Save)) presenter.dialogManager.SaveFileDialog(Loc.Localize("Generics.SaveFile", "Save File"), ".json", "", ".json", presenter.OnFileSave);
        ImGui.SameLine();
        if (ImGuiComponents.IconButton(FontAwesomeIcon.FileImport)) presenter.dialogManager.OpenFileDialog(Loc.Localize("Generics.OpenFile", "Open File"), ".json", presenter.OnFileSelect);
        ImGui.SameLine();
        if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush)) presenter.OnFormat();
    }


    /// <summary>
    ///     Draws the text input for the editor.
    /// </summary>
    private void DrawEditorInput()
    {
        var previewText = presenter.previewDutyText;
        if (ImGui.InputTextMultiline("##DutyInfoInput", ref previewText, 10000, new Vector2(-1, -1), ImGuiInputTextFlags.AllowTabInput))
        {
            presenter.previewDutyText = previewText;
        }
    }


    /// <summary>
    ///     Draws the duty preview pane for the duty editor.
    /// </summary>
    private void DrawEditorPreview()
    {
        // Attempt to parse the text from the editor pane into a duty object.
        Duty? parsedDuty = null;
        try { parsedDuty = Newtonsoft.Json.JsonConvert.DeserializeObject<Duty>(presenter.previewDutyText); }
        catch (Exception e) { ImGui.TextWrapped(e.Message); }

        if (parsedDuty == null || parsedDuty.Bosses == null) return;

        // Create a tab bar for the preview pane.
        if (ImGui.BeginTabBar("##DutyInfoPreview"))
        {
            // Create a tab for the formatted duty preview.
            if (ImGui.BeginTabItem(Loc.Localize("UI.Editor.DutyPreview", "Duty Preview")))
            {
                foreach (var boss in parsedDuty.Bosses)
                {
                    if (ImGui.CollapsingHeader(boss.Name, ImGuiTreeNodeFlags.DefaultOpen))
                    {
                        ImGui.TextWrapped(boss.Strategy);
                        ImGui.NewLine();

                        if (boss.KeyMechanics != null && boss.KeyMechanics.Count != 0)
                        {
                            // Create a table for key mechanics of this boss that are enabled.
                            ImGui.BeginTable("Boss Mechanics", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable);
                            ImGui.TableSetupColumn(Loc.Localize("UI.DutyInfo.MechanicName", "Mechanic"), ImGuiTableColumnFlags.WidthStretch, 0.3f);
                            ImGui.TableSetupColumn(Loc.Localize("UI.DutyInfo.MechanicDescription", "Description"), ImGuiTableColumnFlags.WidthStretch, 0.6f);
                            ImGui.TableSetupColumn(Loc.Localize("UI.DutyInfo.MechanicType", "Type"), ImGuiTableColumnFlags.WidthStretch, 0.2f);
                            ImGui.TableHeadersRow();


                            foreach (var mechanic in boss.KeyMechanics)
                            {
                                ImGui.TableNextRow();
                                ImGui.TableNextColumn();
                                ImGui.Text(mechanic.Name);
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped(mechanic.Description);
                                ImGui.TableNextColumn();
                                if (Enum.GetName(typeof(Mechanics), mechanic.Type) == null) ImGui.Text("Unknown");
                                else ImGui.Text(Enum.GetName(typeof(Mechanics), mechanic.Type));

                            }
                            ImGui.EndTable();
                            ImGui.NewLine();
                        }
                    }
                }
                ImGui.EndTabItem();
            }
        }



        // Create a tab for parsed duty metadata.
        if (ImGui.BeginTabItem(Loc.Localize("UI.Editor.DutyMetadata", "Duty Metadata")))
        {
            ImGui.TextWrapped($"Version: {parsedDuty.Version}");
            ImGui.TextWrapped($"Name: {parsedDuty.Name}");
            ImGui.TextWrapped($"Type: {Enum.GetName(typeof(DutyType), parsedDuty.Type)}");
            ImGui.TextWrapped($"Difficulty: {Enum.GetName(typeof(DutyDifficulty), parsedDuty.Difficulty)}");
            ImGui.TextWrapped($"Level: {parsedDuty.Level}");
            ImGui.TextWrapped($"Expansion: {Enum.GetName(typeof(Expansion), parsedDuty.Expansion)}");
            ImGui.TextWrapped($"TerritoryID: {parsedDuty.TerritoryID}");
            ImGui.TextWrapped($"UnlockQuestID: {parsedDuty.UnlockQuestID}");
            ImGui.TextWrapped($"Compatiable: {parsedDuty.IsSupported()}");
            ImGui.EndTabItem();
        }



        // Create a tab for player information relating to duties.
        if (ImGui.BeginTabItem(Loc.Localize("UI.Editor.EnumValues", "Enum Values")))
        {
            Components.Common.TextHeading("Mechanics");
            foreach (var mechanic in Enum.GetNames(typeof(Mechanics)))
            {
                ImGui.TextWrapped($"{mechanic}: {(int)Enum.Parse(typeof(Mechanics), mechanic)}");
            }

            Components.Common.TextHeading("Duty Types");
            foreach (var dutyType in Enum.GetNames(typeof(DutyType)))
            {
                ImGui.TextWrapped($"{dutyType}: {(int)Enum.Parse(typeof(DutyType), dutyType)}");
            }

            Components.Common.TextHeading("Duty Difficulties");
            foreach (var dutyDifficulty in Enum.GetNames(typeof(DutyDifficulty)))
            {
                ImGui.TextWrapped($"{dutyDifficulty}: {(int)Enum.Parse(typeof(DutyDifficulty), dutyDifficulty)}");
            }

            Components.Common.TextHeading("Expansions");
            foreach (var expansion in Enum.GetNames(typeof(Expansion)))
            {
                ImGui.TextWrapped($"{expansion}: {(int)Enum.Parse(typeof(Expansion), expansion)}");
            }

            ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem(Loc.Localize("UI.Editor.PlayerInfo", "Player Info")))
        {
            ImGui.TextWrapped($"Current Territory: {Service.ClientState.TerritoryType}");
            ImGui.TextWrapped($"Current Language: {Service.ClientState.ClientLanguage}");
        }

        ImGui.EndTabBar();
    }
}