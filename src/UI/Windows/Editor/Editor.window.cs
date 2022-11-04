using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.DutyInfo;

namespace KikoGuide.UI.Windows.Editor
{
    public sealed class EditorWindow : Window, IDisposable
    {
        public EditorPresenter _presenter = new();
        public EditorWindow() : base(WindowManager.EditorWindowName)
        {
            Flags |= ImGuiWindowFlags.NoScrollbar;
            Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            Size = new Vector2(600, 400);
            SizeCondition = ImGuiCond.FirstUseEver;
        }
        public void Dispose()
        {
            _presenter.Dispose();
        }

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
            _presenter.dialogManager.Draw();

            // Draw some buttons at the top of the editor.
            DrawEditorButtons();

            try
            {
                // Duty Editor panes.
                _ = ImGui.BeginTable("##DutyInfoTable", 2, ImGuiTableFlags.Resizable);
                ImGui.TableNextRow();
                _ = ImGui.TableNextColumn();
                _ = ImGui.BeginChild("##EditorInput");
                DrawEditorInput();
                ImGui.EndChild();

                _ = ImGui.TableNextColumn();
                _ = ImGui.BeginChild("##EditorPreview");
                DrawEditorPreview();
                ImGui.EndChild();
                ImGui.EndTable();
            }
            catch (Exception e)
            {
                ImGui.Text(e.ToString());
            }
        }


        /// <summary> 
        ///     Draws the buttons at the top of the editor.
        /// </summary>
        private void DrawEditorButtons()
        {
            // Open a file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.FileImport))
            {
                _presenter.dialogManager.OpenFileDialog(TStrings.OpenFile, ".json", (success, file) => _inputText = _presenter.OnFileSelect(success, file, _inputText));
            }
            Common.AddTooltip(TStrings.OpenFile);
            ImGui.SameLine();

            // Save a file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Save))
            {
                _presenter.dialogManager.SaveFileDialog(TStrings.SaveFile, ".json", "", ".json", (success, file) => EditorPresenter.OnFileSave(success, file, _inputText));
            }
            Common.AddTooltip(TStrings.SaveFile);
            ImGui.SameLine();

            // Format the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush))
            {
                _inputText = EditorPresenter.OnFormat(_inputText);
            }
            Common.AddTooltip(TStrings.EditorFormat);
            ImGui.SameLine();

            // Clear the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Trash))
            {
                _inputText = "";
            }
            Common.AddTooltip(TStrings.EditorClear);
            ImGui.SameLine();

            // Open the contributing guide.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.ExternalLinkAlt))
            {
                EditorPresenter.OpenContributingGuide();
            }
            Common.AddTooltip(TStrings.EditorContributingGuide);

            // Open the donation link.
            if (EditorPresenter.Configuration.Display.DonateButtonShown)
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
            Tuple<Duty?, Exception?> parsedDuty = _presenter.ParseDuty(_inputText);
            string inputText = _inputText;

            // Total lines & characters display
            ImGui.TextWrapped($"Lines: {inputText.Split('\n').Length} | Characters: {inputText.Length}/{_presenter.characterLimit}");

            // Editor input
            if (ImGui.InputTextMultiline("##DutyInfoInput", ref inputText, _presenter.characterLimit, new Vector2(-1, -70), ImGuiInputTextFlags.AllowTabInput))
            {
                _inputText = inputText;
            }

            // Problems window.
            ImGui.TextWrapped(TStrings.EditorProblems);
            if (parsedDuty.Item2?.Message != null)
            {
                Colours.TextWrappedColoured(Colours.Error, parsedDuty.Item2.Message);
            }
            else if (parsedDuty?.Item1?.IsSupported() == false)
            {
                Colours.TextWrappedColoured(Colours.Warning, TStrings.EditorProblemUnsupported);
            }
            else
            {
                ImGui.TextWrapped(TStrings.EditorNoProblems);
            }
        }


        /// <summary>
        ///     Draws the duty preview pane for the duty editor.
        /// </summary>
        private void DrawEditorPreview()
        {
            Duty? duty = _presenter.ParseDuty(_inputText).Item1;

            if (ImGui.BeginTabBar("##DutyEditorTooling"))
            {
                // DutyInfo preview pane.
                if (ImGui.BeginTabItem("DutyInfo Preview"))
                {
                    if (duty != null && duty.Sections != null)
                    {
                        DutyInfoComponent.Draw(duty.Sections);
                    }
                    else
                    {
                        ImGui.TextWrapped("Unable to parse duty; preview unavailable.");
                    }

                    ImGui.EndTabItem();
                }

                // Metadata pane.
                if (ImGui.BeginTabItem(TStrings.EditorMetadata))
                {
                    if (duty != null)
                    {
                        ImGui.TextWrapped($"Version: {duty.Version}");
                        ImGui.TextWrapped($"Name: {duty.Name}");
                        ImGui.TextWrapped($"Type: {LoCExtensions.GetLocalizedName(duty.Type)}");
                        ImGui.TextWrapped($"Difficulty: {LoCExtensions.GetLocalizedName(duty.Difficulty)}");
                        ImGui.TextWrapped($"Level: {duty.Level}");
                        ImGui.TextWrapped($"Expansion: {LoCExtensions.GetLocalizedName(duty.Expansion)}");
                        ImGui.TextWrapped($"TerritoryIDs: {string.Join(", ", duty.TerritoryIDs)} (Current: {EditorPresenter.GetPlayerTerritory})");
                        ImGui.TextWrapped($"UnlockQuestID: {duty.UnlockQuestID}");
                    }
                    else
                    {
                        ImGui.TextWrapped("Unable to parse duty; cannot display metadata.");
                    }

                    ImGui.EndTabItem();
                }

                // Enum IDs pane.
                if (ImGui.BeginTabItem("Enum IDs"))
                {
                    _ = ImGui.BeginChild("##EnumIDs");
                    if (ImGui.CollapsingHeader("Mechanic IDs"))
                    {
                        if (ImGui.BeginTable("##MechanicIDs", 2, ImGuiTableFlags.Borders))
                        {
                            ImGui.TableSetupColumn("Name");
                            ImGui.TableSetupColumn("ID");
                            ImGui.TableHeadersRow();
                            foreach (object? mechanic in Enum.GetValues(typeof(DutyMechanics)))
                            {
                                _ = ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyMechanics)mechanic)}");
                                Common.AddTooltip(LoCExtensions.GetLocalizedDescription((DutyMechanics)mechanic));
                                _ = ImGui.TableNextColumn();
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
                            foreach (object? type in Enum.GetValues(typeof(DutyType)))
                            {
                                _ = ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyType)type)}");
                                Common.AddTooltip(LoCExtensions.GetLocalizedDescription((DutyType)type));
                                _ = ImGui.TableNextColumn();
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
                            foreach (object? difficulty in Enum.GetValues(typeof(DutyDifficulty)))
                            {
                                _ = ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyDifficulty)difficulty)}");
                                Common.AddTooltip(LoCExtensions.GetLocalizedDescription((DutyDifficulty)difficulty));
                                _ = ImGui.TableNextColumn();
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
                            foreach (object? section in Enum.GetValues(typeof(DutySectionType)))
                            {
                                _ = ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutySectionType)section)}");
                                Common.AddTooltip(LoCExtensions.GetLocalizedDescription((DutySectionType)section));
                                _ = ImGui.TableNextColumn();
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
                            foreach (object? expansion in Enum.GetValues(typeof(DutyExpansion)))
                            {
                                _ = ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{LoCExtensions.GetLocalizedName((DutyExpansion)expansion)}");
                                Common.AddTooltip(LoCExtensions.GetLocalizedDescription((DutyExpansion)expansion));
                                _ = ImGui.TableNextColumn();
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