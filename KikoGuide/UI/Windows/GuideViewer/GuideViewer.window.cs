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

            if (GuideViewerPresenter.Configuration.Display.PreventGuideViewerMovement)
            {
                this.Flags |= ImGuiWindowFlags.NoMove;
            }

            if (GuideViewerPresenter.Configuration.Display.PreventGuideViewerResize)
            {
                this.Flags |= ImGuiWindowFlags.NoResize;
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

            // No guide sections for this guide, cannot show anything.
            if (guide.Sections == null || guide.Sections.Count == 0 || !guide.IsSupported())
            {
                ImGui.TextWrapped(TGuideViewer.NoInfoAvailable);
                return;
            }
            GuideSectionComponent.Draw(guide.Sections);
        }

        private void DrawHeaderButtons()
        {
            var guideWindowNoMove = GuideViewerPresenter.Configuration.Display.PreventGuideViewerMovement;
            var guideWindowNoResize = GuideViewerPresenter.Configuration.Display.PreventGuideViewerResize;
            var issueReportUrl = $"{PluginConstants.RepoUrl}/issues/new?labels=type+|+guide-problem&template=guide_issue_report.yaml&title=Guide+Issue+Report%3A+{this.Presenter?.SelectedGuide?.Name} (PluginVer. {GuideViewerPresenter.PluginVersion})";

            ImGui.SameLine();
            if (this.Presenter?.SelectedGuide != null)
            {
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 160);
                if (ImGuiComponents.IconButton(FontAwesomeIcon.Flag))
                {
                    Util.OpenLink(issueReportUrl);
                }
                Common.AddTooltip($"{TGuideViewer.ReportIssueWithGuide}");
                ImGui.SameLine();
            }
            else
            {
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 120);
            }

            if (ImGuiComponents.IconButton(guideWindowNoMove ? FontAwesomeIcon.Lock : FontAwesomeIcon.Unlock))
            {
                this.Flags ^= ImGuiWindowFlags.NoMove;
                GuideViewerPresenter.Configuration.Display.PreventGuideViewerMovement ^= true;
                GuideViewerPresenter.Configuration.Save();
            }
            Common.AddTooltip(guideWindowNoMove ? TGuideViewer.UnlockWindowMovement : TGuideViewer.LockWindowMovement);

            ImGui.SameLine();
            if (ImGuiComponents.IconButton(guideWindowNoResize ? FontAwesomeIcon.Compress : FontAwesomeIcon.Expand))
            {
                this.Flags ^= ImGuiWindowFlags.NoResize;
                GuideViewerPresenter.Configuration.Display.PreventGuideViewerResize ^= true;
                GuideViewerPresenter.Configuration.Save();
            }
            Common.AddTooltip(guideWindowNoResize ? TGuideViewer.LockWindowResize : TGuideViewer.UnlockWindowResize);

            ImGui.SameLine();
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Cog))
            {
                GuideViewerPresenter.ToggleSettingsWindow();
            }
            Common.AddTooltip(TGuideViewer.ToggleSettingsWindow);
        }
    }

}
