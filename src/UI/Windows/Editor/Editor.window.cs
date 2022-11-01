namespace KikoGuide.UI.Windows.Editor
{
    using System;
    using System.Numerics;
    using ImGuiNET;
    using Dalamud.Interface.Components;
    using Dalamud.Interface;
    using Dalamud.Interface.Windowing;
    using KikoGuide.Base;
    using KikoGuide.Types;
    using KikoGuide.UI.Components.Duty;
    using KikoGuide.UI.Components;

    sealed public class EditorWindow : Window, IDisposable
    {
        public EditorPresenter presenter = new EditorPresenter();

        public EditorWindow() : base("Editor")
        {
            Flags |= ImGuiWindowFlags.NoScrollbar;
            Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            Size = new Vector2(600, 400);
            SizeCondition = ImGuiCond.FirstUseEver;
        }

        public void Dispose() => this.presenter.Dispose();

        /// <summary> 
        ///     The current editor input text.
        /// </summary>
        private string _inputText = "";

        /// <summary> 
        ///     Draws the Editor window and sub-components.
        /// </summary>
        public override void Draw()
        {
            // Draw the dialog manager as a sub-menu of the editor.
            presenter.dialogManager.Draw();

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


        /// <summary> 
        ///     Draws the buttons at the top of the editor.
        /// </summary>
        private void DrawEditorButtons()
        {
            if (ImGuiComponents.IconButton(FontAwesomeIcon.FileImport))
            {
                presenter.dialogManager.OpenFileDialog(TStrings.OpenFile, ".json", (success, file) => this._inputText = presenter.OnFileSelect(success, file, this._inputText));
            }
            Tooltips.AddTooltip(TStrings.OpenFile);
            ImGui.SameLine();

            if (ImGuiComponents.IconButton(FontAwesomeIcon.Save))
            {
                presenter.dialogManager.SaveFileDialog(TStrings.SaveFile, ".json", "", ".json", (success, file) => presenter.OnFileSave(success, file, this._inputText));
            }
            Tooltips.AddTooltip(TStrings.SaveFile);
            ImGui.SameLine();

            if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush))
            {
                this._inputText = presenter.OnFormat(this._inputText);
            }
            Tooltips.AddTooltip(TStrings.EditorFormat);
            ImGui.SameLine();

            if (ImGuiComponents.IconButton(FontAwesomeIcon.Trash))
            {
                this._inputText = "";
            }
            Tooltips.AddTooltip(TStrings.EditorClear);
            ImGui.SameLine();

            if (ImGuiComponents.IconButton(FontAwesomeIcon.ExternalLinkAlt))
            {
                Utils.Common.OpenBrowser($"{PStrings.repoUrl}blob/main/CONTRIBUTING.md#guide-contribution");
            }
            Tooltips.AddTooltip(TStrings.EditorContributingGuide);

            if (PluginService.Configuration.supportButtonShown)
            {
                ImGui.SameLine();
                if (ImGuiComponents.IconButton(FontAwesomeIcon.Heart))
                {
                    Utils.Common.OpenBrowser(PStrings.supportButtonUrl);
                }
                Tooltips.AddTooltip(TStrings.Support);
            }
        }


        /// <summary> 
        ///     Draws the input zone for the editor.
        ///  </summary>
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
            ImGui.TextWrapped(TStrings.EditorProblems);
            if (parsedDuty.Item2?.Message != null)
                Colours.TextWrappedColoured(Colours.Error, parsedDuty.Item2.Message);
            else if (parsedDuty?.Item1?.IsSupported() == false)
                Colours.TextWrappedColoured(Colours.Warning, TStrings.EditorProblemUnsupported);
            else
                ImGui.TextWrapped(TStrings.EditorNoProblems);
        }


        /// <summary>
        ///     Draws the duty preview pane for the duty editor.
        /// </summary>
        private void DrawEditorPreview()
        {
            var duty = presenter.ParseDuty(this._inputText).Item1;

            if (ImGui.BeginTabBar("##DutyInfoPreview", ImGuiTabBarFlags.Reorderable))
            {
                if (ImGui.BeginTabItem(TStrings.EditorPreview))
                {
                    if (duty != null) DutyInfoComponent.Draw(duty);
                    ImGui.EndTabItem();
                }



                // Create a tab for parsed duty metadata.
                if (ImGui.BeginTabItem(TStrings.EditorMetadata))
                {
                    if (duty != null)
                    {
                        ImGui.TextWrapped($"Version: {duty.Version}");
                        ImGui.TextWrapped($"Name: {duty.Name}");
                        ImGui.TextWrapped($"Type: {Enum.GetName(typeof(DutyType), duty.Type)}");
                        ImGui.TextWrapped($"Difficulty: {Enum.GetName(typeof(DutyDifficulty), duty.Difficulty)}");
                        ImGui.TextWrapped($"Level: {duty.Level}");
                        ImGui.TextWrapped($"Expansion: {Enum.GetName(typeof(DutyExpansion), duty.Expansion)}");
                        ImGui.TextWrapped($"TerritoryID: {duty.TerritoryID} (Current: {PluginService.ClientState.TerritoryType})");
                        ImGui.TextWrapped($"UnlockQuestID: {duty.UnlockQuestID}");
                    }

                    ImGui.EndTabItem();
                }


                // Create a tab for player information relating to duties.
                if (ImGui.BeginTabItem("IDs"))
                {
                    if (ImGui.CollapsingHeader("Mechanic IDs"))
                    {
                        foreach (var mechanic in Enum.GetNames(typeof(DutyMechanics)))
                        {
                            ImGui.TextWrapped($"{mechanic}: {(int)Enum.Parse(typeof(DutyMechanics), mechanic)}");
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
                        foreach (var expansion in Enum.GetNames(typeof(DutyExpansion)))
                        {
                            ImGui.TextWrapped($"{expansion}: {(int)Enum.Parse(typeof(DutyExpansion), expansion)}");
                        }
                    }
                }
                ImGui.EndTabItem();
            }
            ImGui.EndTabBar();
        }

    }
}