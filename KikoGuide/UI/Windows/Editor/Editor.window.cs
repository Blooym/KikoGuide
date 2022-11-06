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
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.GuideSection;

namespace KikoGuide.UI.Windows.Editor
{
    public sealed class EditorWindow : Window, IDisposable
    {
        internal EditorPresenter Presenter = new();
        public EditorWindow() : base(TWindowNames.GuideEditor)
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
                // Guide Editor panes.
                if (ImGui.BeginTable("##GuideEditorTable", 2, ImGuiTableFlags.Resizable))
                {
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if (ImGui.BeginChild("##GuideEditorInput"))
                    {
                        this.DrawEditorInput();
                        ImGui.EndChild();
                    }

                    ImGui.TableNextColumn();
                    if (ImGui.BeginChild("##GuideEditorPreview"))
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
                this.Presenter.DialogManager.OpenFileDialog(TGenerics.OpenFile, ".json", (success, file) => this.inputText = this.Presenter.OnFileSelect(success, file, this.inputText));
            }
            Common.AddTooltip(TGenerics.OpenFile);
            ImGui.SameLine();

            // Save a file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Save))
            {
                this.Presenter.DialogManager.SaveFileDialog(TGenerics.SaveFile, ".json", "", ".json", (success, file) => EditorPresenter.OnFileSave(success, file, this.inputText));
            }
            Common.AddTooltip(TGenerics.SaveFile);
            ImGui.SameLine();

            // Format the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.PaintBrush))
            {
                this.inputText = EditorPresenter.OnFormat(this.inputText);
            }
            Common.AddTooltip(TEditor.Format);
            ImGui.SameLine();

            // Clear the file.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Trash))
            {
                this.inputText = "";
            }
            Common.AddTooltip(TEditor.Clear);
            ImGui.SameLine();

            // Open the contributing guide.
            if (ImGuiComponents.IconButton(FontAwesomeIcon.ExternalLinkAlt))
            {
                EditorPresenter.OpenContributingGuide();
            }
            Common.AddTooltip(TEditor.ContributingGuide);

            // Open the donation link.
            if (EditorPresenter.Configuration.Display.DonateButtonShown)
            {
                ImGui.SameLine();
                if (ImGuiComponents.IconButton(FontAwesomeIcon.Heart))
                {
                    Util.OpenLink(PluginConstants.DonateButtonUrl);
                }
                Common.AddTooltip(TGenerics.Donate);
            }
        }

        /// <summary>
        ///     Draws the input zone for the editor.
        ///  </summary>
        private void DrawEditorInput()
        {
            var parsedGuide = this.Presenter.ParseGuide(this.inputText);
            var inputText = this.inputText;

            // Total lines & characters display
            ImGui.TextWrapped($"Lines: {inputText.Split('\n').Length} | Characters: {inputText.Length}/{this.Presenter.CharacterLimit}");

            // Editor input
            if (ImGui.InputTextMultiline("##GuideEditorInfoInput", ref inputText, this.Presenter.CharacterLimit, new Vector2(-1, -70), ImGuiInputTextFlags.AllowTabInput))
            {
                this.inputText = inputText;
            }

            // Problems window.
            ImGui.TextWrapped(TEditor.Problems);
            if (parsedGuide.Item2?.Message != null)
            {
                Colours.TextWrappedColoured(Colours.Error, parsedGuide.Item2.Message);
            }
            else if (parsedGuide?.Item1?.IsSupported() == false)
            {
                Colours.TextWrappedColoured(Colours.Warning, TEditor.ProblemUnsupported);
            }
            else
            {
                ImGui.TextWrapped(TEditor.NoProblems);
            }
        }


        /// <summary>
        ///     Draws the guide preview pane for the guide editor.
        /// </summary>
        private void DrawEditorPreview()
        {
            var guide = this.Presenter.ParseGuide(this.inputText).Item1;

            if (ImGui.BeginTabBar("##GuideEditorTooling"))
            {
                // GuideViewer preview pane.
                if (ImGui.BeginTabItem(TEditor.Preview))
                {
                    if (guide != null && guide.Sections != null)
                    {
                        GuideSectionComponent.Draw(guide.Sections);
                    }
                    else
                    {
                        ImGui.TextWrapped("Unable to parse guide; preview unavailable.");
                    }

                    ImGui.EndTabItem();
                }

                // Metadata pane.
                if (ImGui.BeginTabItem(TEditor.Metadata))
                {
                    if (guide != null)
                    {
                        ImGui.TextWrapped($"Version: {guide.Version}");
                        ImGui.TextWrapped($"Name: {guide.Name}");
                        ImGui.TextWrapped($"Type: {AttributeExtensions.GetNameAttribute(guide.Type)}");
                        ImGui.TextWrapped($"Difficulty: {AttributeExtensions.GetNameAttribute(guide.Difficulty)}");
                        ImGui.TextWrapped($"Level: {guide.Level}");
                        ImGui.TextWrapped($"Expansion: {AttributeExtensions.GetNameAttribute(guide.Expansion)}");
                        ImGui.TextWrapped($"TerritoryIDs: {string.Join(", ", guide.TerritoryIDss)} (Current: {EditorPresenter.GetPlayerTerritory})");
                        ImGui.TextWrapped($"UnlockQuestID: {guide.UnlockQuestID}");
                    }
                    else
                    {
                        ImGui.TextWrapped(TEditor.CannotParseNoPreview);
                    }

                    ImGui.EndTabItem();
                }

                // Enum IDs pane.
                if (ImGui.BeginTabItem(TEditor.EnumList))
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
                                foreach (var mechanic in Enum.GetValues(typeof(GuideMechanics)))
                                {
                                    ImGui.TableNextColumn();
                                    ImGui.TextWrapped($"{AttributeExtensions.GetNameAttribute((GuideMechanics)mechanic)}");
                                    Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((GuideMechanics)mechanic));
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
                            foreach (var section in Enum.GetValues(typeof(GuideSectionType)))
                            {
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{AttributeExtensions.GetNameAttribute((GuideSectionType)section)}");
                                Common.AddTooltip(AttributeExtensions.GetDescriptionAttribute((GuideSectionType)section));
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
