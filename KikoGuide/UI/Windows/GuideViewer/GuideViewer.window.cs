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
    public sealed class GuideViewerWindow : Window, IDisposable
    {
        internal GuideViewerPresenter Presenter = new();

        public GuideViewerWindow() : base(TWindowNames.GuideViewer)
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

        private static bool CanShowExtendedInfo(Guide guide) => ImGui.GetWindowWidth() - ImGui.CalcTextSize(TGuideViewer.GuideHeading(guide.Name)).X > 180;

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

            // Show the guide.
            Colours.TextWrappedColoured(Colours.Grey, TGuideViewer.GuideHeading(guide.Name));
            if (CanShowExtendedInfo(guide))
            { ImGui.SameLine(); this.DrawHeaderButtons(); }
            ImGui.Separator();

            // Draw guide sections.
            if (guide.Sections == null || guide.Sections.Count == 0 || !guide.IsSupported())
            {
                ImGui.TextWrapped(TGuideViewer.NoGuideInfoAvailable);
                return;
            }
            GuideSectionComponent.Draw(guide.Sections);
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
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 100);
                if (ImGuiComponents.IconButton(FontAwesomeIcon.Flag))
                {
                    Util.OpenLink(issueReportUrl);
                }
                Common.AddTooltip($"{TGuideViewer.ReportIssueWithGuide}");
                ImGui.SameLine();
            }
            else
            {
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 70);
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
