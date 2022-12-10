using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.GuideSection;

namespace KikoGuide.UI.Windows.GuideViewer
{
    internal sealed class GuideViewerWindow : Window, IDisposable
    {
        internal GuideViewerPresenter Presenter = new();

        internal GuideViewerWindow() : base(TWindowNames.GuideViewer)
        {
            this.Flags |= ImGuiWindowFlags.NoScrollbar;

            this.Size = new Vector2(380, 420);
            this.SizeCondition = ImGuiCond.FirstUseEver;

            if (GuideViewerPresenter.Configuration.Display.LockGuideViewerWindow)
            {
                this.Flags |= ImGuiWindowFlags.NoMove;
                this.Flags |= ImGuiWindowFlags.NoResize;
                this.RespectCloseHotkey = false;
            }
        }

        public void Dispose() => this.Presenter.Dispose();

        private static bool CanShowHeaderButtons(string title) => ImGui.GetWindowWidth() - ImGui.CalcTextSize(TGuideViewer.GuideHeading(title)).X > 220;

        private bool currentEditingState;

        /// <summary>
        ///     Draws the guide viewer window.
        /// </summary>
        public override void Draw()
        {
            var guide = this.Presenter.SelectedGuide;

            // No guide selected, show non-selected message.
            if (guide == null)
            {
                ImGui.TextWrapped(TGuideViewer.NoGuideSelected);
                return;
            }

            // Player does not have the guide unlocked and have the setting enabled to hide locked guides.
            if (!guide.IsUnlocked() && GuideViewerPresenter.Configuration.Display.HideLockedGuides)
            {
                ImGui.TextWrapped(TGuideViewer.GuideNotUnlocked);
                return;
            }



            ///////////////////////////////////////
            /////// GUIDE CONTENTS DRAWING ////////
            ///////////////////////////////////////

            // Guide title and header buttons if the window is wide enough.
            Colours.TextWrappedColoured(Colours.Grey, TGuideViewer.GuideHeading(guide.GetCanonicalName()));
            if (CanShowHeaderButtons(guide.GetCanonicalName()))
            {
                ImGui.SameLine();
                this.DrawHeaderButtons();
            }

            // Everything from here will scroll from the child so the header buttons stay visible.
            ImGui.BeginChild("##GuideContents", new Vector2(0, 0), false);


            // First, let's draw the guides sections and primary guide content. this gets 70% of the window length, so lets calculate that.
            var size = (this.Presenter.LinkedNote?.GetContents() != string.Empty || this.currentEditingState) ? ImGui.GetWindowHeight() * 0.75f : ImGui.GetWindowHeight();
            ImGui.BeginChild("GuideSectionContent", size: new Vector2(0, size), true, ImGuiWindowFlags.NoScrollbar);
            if (guide.Sections == null || guide.Sections.Count == 0 || !guide.IsSupported())
            {
                ImGui.TextWrapped(TGuideViewer.NoGuideInfoAvailable);
                return;
            }
            ImGui.TextDisabled("Guide");
            GuideSectionComponent.Draw(guide.Sections);
            ImGui.EndChild();

            // Now, let's draw the guide's notes. This gets 30% of the window length, so lets calculate that.
            if (this.Presenter.LinkedNote != null)
            {
                this.DrawNoteEditor(this.Presenter.LinkedNote);
            }
        }

        private void DrawNoteEditor(Note note)
        {

            var noteText = note.GetContents();


            if (this.currentEditingState)
            {
                ImGui.BeginChild("GuideNotes", new Vector2(0, ImGui.GetWindowHeight() * 0.20f), true, ImGuiWindowFlags.NoScrollbar);
                ImGui.TextDisabled(TGenerics.Note);
                if (ImGui.InputTextMultiline("##GuideNote", ref noteText, 10000, new Vector2(-1, ImGui.GetWindowHeight() / 2)))
                {
                    note.SetContents(noteText);
                }
                // align the buttons to the right
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 60);

                if (ImGui.Button("Save"))
                {
                    this.currentEditingState = false;
                    note.Save();
                }
                ImGui.EndChild();
            }
            else if (!string.IsNullOrEmpty(noteText))
            {
                ImGui.BeginChild("GuideNotes", new Vector2(0, ImGui.GetWindowHeight() * 0.20f), true, ImGuiWindowFlags.NoScrollbar);
                ImGui.TextDisabled(TGenerics.Note);

                ImGui.BeginChild("##GuideNote");
                Common.TextWrappedUnformatted(noteText);
                ImGui.EndChild();
                ImGui.EndChild();
            }
        }

        private void DrawHeaderButtons()
        {
            var guideWindowNoMove = GuideViewerPresenter.Configuration.Display.LockGuideViewerWindow;
            var guide = this.Presenter.SelectedGuide;

            // Report issue button.
            ImGui.SameLine();
            if (guide != null && guide.Sections != null && guide.Sections.Count > 0)
            {
                var issueReportUrl = $"{PluginConstants.RepoUrl}/issues/new?labels=type+|+guide-problem&template=guide_issue_report.yaml&title=Guide+Issue+Report%3A+{guide.Name} (PluginVer. {GuideViewerPresenter.PluginVersion})";
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 140);
                if (ImGuiComponents.IconButton(FontAwesomeIcon.Flag))
                {
                    Util.OpenLink(issueReportUrl);
                }
                Common.AddTooltip($"{TGuideViewer.ReportIssueWithGuide}");
                ImGui.SameLine();
            }
            else
            {
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 100);
            }

            // Lock button.
            ImGui.SameLine();
            if (ImGuiComponents.IconButton(guideWindowNoMove ? FontAwesomeIcon.Lock : FontAwesomeIcon.Unlock))
            {
                this.Flags ^= ImGuiWindowFlags.NoMove;
                this.Flags ^= ImGuiWindowFlags.NoResize;
                this.RespectCloseHotkey = !this.RespectCloseHotkey;
                GuideViewerPresenter.Configuration.Display.LockGuideViewerWindow ^= true;
                GuideViewerPresenter.Configuration.Save();
            }
            Common.AddTooltip(guideWindowNoMove ? TGuideViewer.UnlockWindowMovement : TGuideViewer.LockWindowMovement);

            // Note edit button.
            ImGui.SameLine();
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Pen))
            {
                this.currentEditingState ^= true;
            }

            // Settings button.
            ImGui.SameLine();
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Cog))
            {
                GuideViewerPresenter.ToggleSettingsWindow();
            }
            Common.AddTooltip(TGuideViewer.ToggleSettingsWindow);
        }
    }
}
