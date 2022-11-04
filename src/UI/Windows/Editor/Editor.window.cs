namespace KikoGuide.UI.Windows.Editor
{
    using System;
    using System.Numerics;
    using System.Collections.Generic;
    using ImGuiNET;
    using Dalamud.Utility;
    using Dalamud.Interface;
    using Dalamud.Interface.Windowing;
    using Dalamud.Interface.Components;
    using KikoGuide.Base;
    using KikoGuide.Localization;
    using KikoGuide.Types;
    using KikoGuide.Managers;
    using KikoGuide.UI.ImGuiBasicComponents;
    using KikoGuide.UI.ImGuiFullComponents.DutyInfo;
    using KikoGuide.UI.ImGuiFullComponents.DutyList;

    public sealed class EditorWindow : Window, IDisposable
    {
        public EditorPresenter _presenter = new EditorPresenter();
        public EditorWindow() : base(WindowManager.EditorWindowName)
        {
            Flags |= ImGuiWindowFlags.NoScrollbar;
            Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            Size = new Vector2(600, 400);
            SizeCondition = ImGuiCond.FirstUseEver;
        }
        public void Dispose() => this._presenter.Dispose();

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
            this._presenter.dialogManager.Draw();

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
            // Open a file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.FileImport))
            {
                this._presenter.dialogManager.OpenFileDialog(TStrings.OpenFile, ".json", (success, file) => this._inputText = this._presenter.OnFileSelect(success, file, this._inputText));
            }
            Common.AddTooltip(TStrings.OpenFile);
            ImGui.SameLine();

            // Save a file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Save))
            {
                this._presenter.dialogManager.SaveFileDialog(TStrings.SaveFile, ".json", "", ".json", (success, file) => this._presenter.OnFileSave(success, file, this._inputText));
            }
            Common.AddTooltip(TStrings.SaveFile);
            ImGui.SameLine();

            // Format the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush))
            {
                this._inputText = this._presenter.OnFormat(this._inputText);
            }
            Common.AddTooltip(TStrings.EditorFormat);
            ImGui.SameLine();

            // Clear the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Trash))
            {
                this._inputText = "";
            }
            Common.AddTooltip(TStrings.EditorClear);
            ImGui.SameLine();

            // Open the contributing guide.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.ExternalLinkAlt))
            {
                this._presenter.OpenContributingGuide();
            }
            Common.AddTooltip(TStrings.EditorContributingGuide);

            // Open the donation link.
            if (this._presenter.Configuration.Display.DonateButtonShown)
            {
                ImGui.SameLine();
                if (ImGuiComponents.IconButton(FontAwesomeIcon.Heart))
                {
                    Util.OpenLink(PluginConstants.donateButtonUrl);
                }
                Common.AddTooltip(TStrings.Donate);
            }
        }


        /// <summary> 
        ///     Draws the input zone for the editor.
        ///  </summary>
        private void DrawEditorInput()
        {
            var parsedDuty = this._presenter.ParseDuty(this._inputText);
            var inputText = this._inputText;

            // Total lines & characters display
            ImGui.TextWrapped($"Lines: {inputText.Split('\n').Length.ToString()} | Characters: {inputText.Length.ToString()}/{this._presenter.characterLimit}");

            // Editor input
            if (ImGui.InputTextMultiline("##DutyInfoInput", ref inputText, this._presenter.characterLimit, new Vector2(-1, -70), ImGuiInputTextFlags.AllowTabInput))
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
            var duty = this._presenter.ParseDuty(this._inputText).Item1;

            if (ImGui.BeginTabBar("##DutyEditorTooling"))
            {
                // DutyInfo preview pane.
                if (ImGui.BeginTabItem("DutyInfo Preview"))
                {
                    if (duty != null) DutyInfoComponent.Draw(duty);
                    ImGui.EndTabItem();
                }

                // DutyList preview pane.
                if (ImGui.BeginTabItem("DutyList Preview"))
                {
                    if (duty != null) DutyListComponent.Draw(new List<Duty>() { duty }, (Duty duty) => { });
                    ImGui.EndTabItem();
                }

                // Metadata pane.
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
                        ImGui.TextWrapped($"TerritoryIDs: {string.Join(", ", duty.TerritoryIDs)} (Current: {this._presenter.GetPlayerTerritory})");
                        ImGui.TextWrapped($"UnlockQuestID: {duty.UnlockQuestID}");
                    }

                    ImGui.EndTabItem();
                }

                // Enum IDs pane.
                if (ImGui.BeginTabItem("Enum IDs"))
                {
                    ImGui.BeginChild("##EnumIDs");
                    if (ImGui.CollapsingHeader("Mechanic IDs"))
                    {
                        if (ImGui.BeginTable("##MechanicIDs", 2, ImGuiTableFlags.Borders))
                        {
                            ImGui.TableSetupColumn("Name");
                            ImGui.TableSetupColumn("ID");
                            ImGui.TableHeadersRow();
                            foreach (var mechanic in Enum.GetValues(typeof(DutyMechanics)))
                            {
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyMechanics)mechanic)}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{(int)mechanic}");
                            }
                            ImGui.EndTable();
                        }
                    }

                    if (ImGui.CollapsingHeader("Duty Type IDs"))
                    {
                        if (ImGui.BeginTable("##DutyTypeIDs", 2, ImGuiTableFlags.Borders))
                        {
                            ImGui.TableSetupColumn("Name");
                            ImGui.TableSetupColumn("ID");
                            ImGui.TableHeadersRow();
                            foreach (var type in Enum.GetValues(typeof(DutyType)))
                            {
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyType)type)}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{(int)type}");
                            }
                            ImGui.EndTable();
                        }
                    }

                    if (ImGui.CollapsingHeader("Difficulty IDs"))
                    {
                        if (ImGui.BeginTable("##DifficultyIDs", 2, ImGuiTableFlags.Borders))
                        {
                            ImGui.TableSetupColumn("Name");
                            ImGui.TableSetupColumn("ID");
                            ImGui.TableHeadersRow();
                            foreach (var difficulty in Enum.GetValues(typeof(DutyDifficulty)))
                            {
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyDifficulty)difficulty)}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{(int)difficulty}");
                            }
                            ImGui.EndTable();
                        }
                    }

                    if (ImGui.CollapsingHeader("Duty Section IDs"))
                    {
                        if (ImGui.BeginTable("##DutySectionIDs", 2, ImGuiTableFlags.Borders))
                        {
                            ImGui.TableSetupColumn("Name");
                            ImGui.TableSetupColumn("ID");
                            ImGui.TableHeadersRow();
                            foreach (var section in Enum.GetValues(typeof(DutySectionType)))
                            {
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutySectionType)section)}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{(int)section}");
                            }
                            ImGui.EndTable();
                        }
                    }

                    if (ImGui.CollapsingHeader("Expansion IDs"))
                    {
                        if (ImGui.BeginTable("##ExpansionIDs", 2, ImGuiTableFlags.Borders))
                        {
                            ImGui.TableSetupColumn("Name");
                            ImGui.TableSetupColumn("ID");
                            ImGui.TableHeadersRow();
                            foreach (var expansion in Enum.GetValues(typeof(DutyExpansion)))
                            {
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyExpansion)expansion)}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{(int)expansion}");
                            }
                            ImGui.EndTable();
                        }
                    }
                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }

    }
}