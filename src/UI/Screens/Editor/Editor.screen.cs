namespace KikoGuide.UI.Screens.Editor;

using System;
using System.Numerics;
using ImGuiNET;
using CheapLoc;
using Dalamud.Interface.Components;
using Dalamud.Interface;
using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.UI.Components.Duty;
using KikoGuide.UI.Components;

sealed class EditorScreen : IDisposable
{
    public EditorPresenter presenter = new EditorPresenter();

    /// <summary> Disposes of the Editor screen and any resources it uses. </summary>
    public void Dispose() => this.presenter.Dispose();

    /// <summary> Draws all UI elements associated with the Editor screen. </summary>
    public void Draw() => this.DrawEditorUI();

    /// <summary> The current editor input text. </summary>
    private string _inputText = "";

    /// <summary> Draws the Editor window and sub-components. </summary>
    private void DrawEditorUI()
    {
        if (!presenter.isVisible) return;

        // Draw the dialog manager as a sub-menu of the editor.
        presenter.dialogManager.Draw();

        // Begin the editor window.
        ImGui.SetWindowSize(new Vector2(600, 400), ImGuiCond.FirstUseEver);
        if (ImGui.Begin(String.Format(Loc.Localize("UI.Screens.Editor.Title", "{0} - Duty Editor"), PStrings.pluginName), ref presenter.isVisible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
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


    /// <summary> Draws the buttons at the top of the editor. </summary>
    private void DrawEditorButtons()
    {
        if (ImGuiComponents.IconButton(FontAwesomeIcon.FileImport))
        {
            presenter.dialogManager.OpenFileDialog(Loc.Localize("Generics.OpenFile", "Open File"), ".json", (success, file) => this._inputText = presenter.OnFileSelect(success, file, this._inputText));
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Screens.Editor.OpenFile.Tooltip", "Open a duty from a JSON file."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.Save))
        {
            presenter.dialogManager.SaveFileDialog(Loc.Localize("Generics.SaveFile", "Save File"), ".json", "", ".json", (success, file) => presenter.OnFileSave(success, file, this._inputText));
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Screens.Editor.SaveFile.Tooltip", "Save the current duty to a JSON file."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush))
        {
            this._inputText = presenter.OnFormat(this._inputText);
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Screens.Editor.Format.Tooltip", "Formats the current JSON text."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.Trash))
        {
            this._inputText = "";
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Screens.Editor.Clear.Tooltip", "Clears the text input."));
        ImGui.SameLine();

        if (ImGuiComponents.IconButton(FontAwesomeIcon.ExternalLinkAlt))
        {
            Utils.Common.OpenBrowser($"{PStrings.pluginRepository}blob/main/CONTRIBUTING.md#guide-contribution");
        }
        Tooltips.AddTooltip(Loc.Localize("UI.Screens.Editor.Contribute.Tooltip", "Opens the contribution guidelines."));
    }


    /// <summary> Draws the input zone for the editor. </summary>
    private void DrawEditorInput()
    {
        var parsedDuty = presenter.ParseDuty(this._inputText);
        var inputText = this._inputText;

        // Total lines & characters display
        ImGui.TextWrapped($"Lines: {inputText.Split('\n').Length.ToString()} | Characters: {inputText.Length.ToString()}/{presenter.characterLimit}");

        // Editor input
        if (ImGui.InputTextMultiline("##DutyInfoInput", ref inputText, presenter.characterLimit, new Vector2(-1, -70), ImGuiInputTextFlags.AllowTabInput))
        {
            this._inputText = inputText;
        }

        // Problems window.
        ImGui.TextWrapped(Loc.Localize("UI.Screens.Editor.Problems", "Problems:"));
        if (parsedDuty.Item2?.Message != null)
            Colours.TextWrappedColoured(Colours.Error, parsedDuty.Item2.Message);
        else if (parsedDuty?.Item1?.IsSupported() == false)
            Colours.TextWrappedColoured(Colours.Warning, Loc.Localize("UI.Screens.Editor.Unsupported", "Duty contains invalid IDs and/or is unsupported by this plugin version."));
        else
            ImGui.TextWrapped(Loc.Localize("UI.Screens.Editor.Problems.None", "No problems have been detected for this file."));
    }


    /// <summary> Draws the duty preview pane for the duty editor. </summary>
    private void DrawEditorPreview()
    {
        var duty = presenter.ParseDuty(this._inputText).Item1;

        // Create a tab for the parsed duty display.
        if (ImGui.BeginTabBar("##DutyInfoPreview", ImGuiTabBarFlags.Reorderable))
        {
            if (ImGui.BeginTabItem(Loc.Localize("UI.Screens.Editor.Preview", "Preview")))
            {
                if (duty != null)
                {
                    DutyHeadingComponent.Draw(duty);
                    if (duty.Bosses != null) DutyBossListComponent.Draw(duty.Bosses);
                }
                ImGui.EndTabItem();
            }
        }


        // Create a tab for parsed duty metadata.
        if (ImGui.BeginTabItem(Loc.Localize("UI.Screens.Editor.Metadata", "Metadata")))
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

            ImGui.EndTabItem();
        }


        // Create a tab for player information relating to duties.
        if (ImGui.BeginTabItem(Loc.Localize("UI.Screens.Editor.IDs", "IDs")))
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