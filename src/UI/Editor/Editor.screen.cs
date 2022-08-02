namespace KikoGuide.UI.Editor;

using System;
using System.Numerics;
using ImGuiNET;
using CheapLoc;
using Dalamud.Interface.Components;
using Dalamud.Interface;
using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.UI.Components;

internal class EditorScreen : IDisposable
{
    public EditorPresenter presenter = new EditorPresenter();


    /// <summary>
    ///     Disposes of the Editor screen and any resources it uses.
    /// </summary>
    public void Dispose()
    {
        this.presenter.Dispose();
    }


    /// <summary>
    ///     Draws all UI elements associated with the Editor screen.
    /// </summary>
    public void Draw() => this.DrawEditorUI();


    private string _inputText = "";

    /// <summary> 
    ///     Draws the Editor window and sub-components.
    /// </summary>
    private void DrawEditorUI()
    {
        if (!presenter.isVisible) return;

        // Draw the dialog manager as a sub-menu of the editor.
        presenter.dialogManager.Draw();

        // Begin the editor window.
        ImGui.SetWindowSize(new Vector2(600, 400), ImGuiCond.FirstUseEver);
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
            ImGui.TableNextColumn();
            ImGui.EndChild();
            ImGui.EndTable();
        }
    }


    /// <summary>
    ///     Draws the buttons at the top of the editor.
    /// </summary>
    private void DrawEditorButtons()
    {
        if (ImGuiComponents.IconButton(FontAwesomeIcon.FileImport))
        {
            presenter.dialogManager.OpenFileDialog(Loc.Localize("Generics.OpenFile", "Open File"), ".json", (success, file) => this._inputText = presenter.OnFileSelect(success, file, this._inputText));
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Editor.OpenFile.Tooltip", "Open a duty from a JSON file."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.Save))
        {
            presenter.dialogManager.SaveFileDialog(Loc.Localize("Generics.SaveFile", "Save File"), ".json", "", ".json", (success, file) => presenter.OnFileSave(success, file, this._inputText));
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Editor.SaveFile.Tooltip", "Save the current duty to a JSON file."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush))
        {
            this._inputText = presenter.OnFormat(this._inputText);
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Editor.Format.Tooltip", "Formats the current JSON text."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.Trash))
        {
            this._inputText = "";
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Editor.Clear.Tooltip", "Clears the text input."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.ExternalLinkAlt))
        {
            Utils.Common.OpenBrowser($"{PStrings.pluginRepository}blob/main/CONTRIBUTING.md#guide-contribution");
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Editor.Contribute.Tooltip", "Opens the contribution guidelines."));
    }


    /// <summary>
    ///     Draws the input zone for the editor.
    /// </summary>
    private void DrawEditorInput()
    {
        var txt = this._inputText;
        var parsedDuty = presenter.ParseDuty(this._inputText);

        // The text input box for the editor.
        if (ImGui.InputTextMultiline("##DutyInfoInput", ref txt, 10000, new Vector2(-1, -70), ImGuiInputTextFlags.AllowTabInput))
        {
            this._inputText = txt;
        }

        ImGui.TextWrapped(Loc.Localize("UI.Editor.DetectedIssues", "Detected Issues:"));
        if (parsedDuty.Item2?.Message != null) ImGui.TextWrapped(parsedDuty.Item2?.Message);
        else if (parsedDuty?.Item1?.IsSupported() == false) ImGui.TextWrapped(Loc.Localize("UI.Editor.Unsupported", "Duty contains invalid enum values or is unsupported by this plugin version."));
        else ImGui.TextWrapped(Loc.Localize("UI.Editor.NoIssues", "No issues detected."));
    }


    /// <summary>
    ///     Draws the duty preview pane for the duty editor.
    /// </summary>
    private void DrawEditorPreview()
    {
        var duty = presenter.ParseDuty(this._inputText).Item1;

        // Create a tab bar for the preview pane.
        if (ImGui.BeginTabBar("##DutyInfoPreview", ImGuiTabBarFlags.Reorderable))
        {
            // Create a tab for the formatted duty preview.
            if (ImGui.BeginTabItem(Loc.Localize("UI.Editor.Preview", "Preview")))
            {
                if (duty != null && duty.Bosses != null)
                {
                    foreach (var boss in duty.Bosses)
                    {
                        if (ImGui.CollapsingHeader(boss.Name, ImGuiTreeNodeFlags.DefaultOpen))
                        {
                            ImGui.TextWrapped(boss.Strategy);
                            ImGui.NewLine();

                            if (boss.KeyMechanics != null && boss.KeyMechanics.Count != 0)
                            {
                                // Create a table for key mechanics of this boss that are enabled.
                                ImGui.BeginTable("##Boss Mechanics", 3, ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable);
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
                }
                else ImGui.TextWrapped(Loc.Localize("UI.Editor.NothingToPreview", "Nothing to preview, start editing or resolve issues to see something here."));

                ImGui.EndTabItem();
            }
        }


        // Create a tab for parsed duty metadata.
        if (ImGui.BeginTabItem(Loc.Localize("UI.Editor.Metadata", "Metadata")))
        {
            if (duty != null)
            {
                ImGui.TextWrapped($"Version: {duty.Version}");
                ImGui.TextWrapped($"Name: {duty.Name}");
                ImGui.TextWrapped($"Type: {Enum.GetName(typeof(DutyType), duty.Type)}");
                ImGui.TextWrapped($"Difficulty: {Enum.GetName(typeof(DutyDifficulty), duty.Difficulty)}");
                ImGui.TextWrapped($"Level: {duty.Level}");
                ImGui.TextWrapped($"Expansion: {Enum.GetName(typeof(Expansion), duty.Expansion)}");
                ImGui.TextWrapped($"TerritoryID: {duty.TerritoryID} (Current: {Service.ClientState.TerritoryType})");
                ImGui.TextWrapped($"UnlockQuestID: {duty.UnlockQuestID}");
            }
            else ImGui.TextWrapped(Loc.Localize("UI.Editor.NothingToPreview", "Nothing to preview, start editing or resolve issues to see something here."));

            ImGui.EndTabItem();
        }


        // Create a tab for player information relating to duties.
        if (ImGui.BeginTabItem(Loc.Localize("UI.Editor.IDs", "IDs")))
        {
            if (ImGui.CollapsingHeader("Mechanic IDs"))
            {
                foreach (var mechanic in Enum.GetNames(typeof(Mechanics)))
                {
                    ImGui.TextWrapped($"{mechanic}: {(int)Enum.Parse(typeof(Mechanics), mechanic)}");
                }
            }

            if (ImGui.CollapsingHeader("Duty Type IDs"))
            {
                foreach (var dutyType in Enum.GetNames(typeof(DutyType)))
                {
                    ImGui.TextWrapped($"{dutyType}: {(int)Enum.Parse(typeof(DutyType), dutyType)}");
                }
            }

            if (ImGui.CollapsingHeader("Difficulty IDs"))
            {
                foreach (var dutyDifficulty in Enum.GetNames(typeof(DutyDifficulty)))
                {
                    ImGui.TextWrapped($"{dutyDifficulty}: {(int)Enum.Parse(typeof(DutyDifficulty), dutyDifficulty)}");
                }
            }

            if (ImGui.CollapsingHeader("Expansion IDs"))
            {
                foreach (var expansion in Enum.GetNames(typeof(Expansion)))
                {
                    ImGui.TextWrapped($"{expansion}: {(int)Enum.Parse(typeof(Expansion), expansion)}");
                }
            }

            ImGui.EndTabItem();
        }

        ImGui.EndTabBar();
    }
}