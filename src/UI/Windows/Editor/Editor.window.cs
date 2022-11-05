using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Attributes;
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
        internal EditorPresenter Presenter = new();
        public EditorWindow() : base(WindowManager.EditorWindowName)
        {
            this.Flags |= ImGuiWindowFlags.NoScrollbar;
            this.Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            this.Size = new Vector2(600, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }
        public void Dispose() => this.Presenter.Dispose();

        /// <summary> 
        ///     The current editor input text.
        /// </summary>
        private string inputText = "";

        /// <summary> 
        ///     Draws the Editor window and sub-components.
        /// </summary>
        public override void Draw()
        {
            // Draw the dialog manager as a sub-menu of the editor.
            this.Presenter.DialogManager.Draw();

            // Draw some buttons at the top of the editor.
            this.DrawEditorButtons();

            try
            {
                // Duty Editor panes.
                if (ImGui.BeginTable("##DutyInfoTable", 2, ImGuiTableFlags.Resizable))
                {
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if (ImGui.BeginChild("##EditorInput"))
                    {
                        this.DrawEditorInput();
                        ImGui.EndChild();
                    }

                    ImGui.TableNextColumn();
                    if (ImGui.BeginChild("##EditorPreview"))
                    {
                        this.DrawEditorPreview();
                        ImGui.EndChild();
                    }
                    ImGui.EndTable();
                }
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
                this.Presenter.DialogManager.OpenFileDialog(TStrings.OpenFile, ".json", (success, file) => this.inputText = this.Presenter.OnFileSelect(success, file, this.inputText));
            }
            Common.AddTooltip(TStrings.OpenFile);
            ImGui.SameLine();

            // Save a file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Save))
            {
                this.Presenter.DialogManager.SaveFileDialog(TStrings.SaveFile, ".json", "", ".json", (success, file) => EditorPresenter.OnFileSave(success, file, this.inputText));
            }
            Common.AddTooltip(TStrings.SaveFile);
            ImGui.SameLine();

            // Format the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush))
            {
                this.inputText = EditorPresenter.OnFormat(this.inputText);
            }
            Common.AddTooltip(TStrings.EditorFormat);
            ImGui.SameLine();

            // Clear the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Trash))
            {
                this.inputText = "";
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
                    Util.OpenLink(PluginConstants.DonateButtonUrl);
                }
                Common.AddTooltip(TStrings.Donate);
            }
        }


        /// <summary> 
        ///     Draws the input zone for the editor.
        ///  </summary>
        private void DrawEditorInput()
        {
            var parsedDuty = this.Presenter.ParseDuty(this.inputText);
            var inputText = this.inputText;

            // Total lines & characters display
            ImGui.TextWrapped($"Lines: {inputText.Split('\n').Length} | Characters: {inputText.Length}/{this.Presenter.CharacterLimit}");

            // Editor input
            if (ImGui.InputTextMultiline("##DutyInfoInput", ref inputText, this.Presenter.CharacterLimit, new Vector2(-1, -70), ImGuiInputTextFlags.AllowTabInput))
            {
                this.inputText = inputText;
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
            var duty = this.Presenter.ParseDuty(this.inputText).Item1;

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
                        ImGui.TextWrapped($"Type: {AttributeExtensions.GetNameAttribute(duty.Type)}");
                        ImGui.TextWrapped($"Difficulty: {AttributeExtensions.GetNameAttribute(duty.Difficulty)}");
                        ImGui.TextWrapped($"Level: {duty.Level}");
                        ImGui.TextWrapped($"Expansion: {AttributeExtensions.GetNameAttribute(duty.Expansion)}");
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
                    if (ImGui.BeginChild("##EnumIDs"))
                    {
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
                                    ImGui.TextWrapped($"{AttributeExtensions.GetNameAttribute((DutyMechanics)mechanic)}");
                                    Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((DutyMechanics)mechanic));
                                    ImGui.TableNextColumn();
                                    ImGui.TextWrapped($"{(int)mechanic}");
                                }
                                ImGui.EndTable();
                            }
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
                                ImGui.TextWrapped($"{AttributeExtensions.GetNameAttribute((DutyType)type)}");
                                Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((DutyType)type));
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
                                ImGui.TextWrapped($"{AttributeExtensions.GetNameAttribute((DutyDifficulty)difficulty)}");
                                Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((DutyDifficulty)difficulty));
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
                                ImGui.TextWrapped($"{AttributeExtensions.GetNameAttribute((DutySectionType)section)}");
                                Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((DutySectionType)section));
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
                                ImGui.TextWrapped($"{AttributeExtensions.GetNameAttribute((DutyExpansion)expansion)}");
                                Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((DutyExpansion)expansion));
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