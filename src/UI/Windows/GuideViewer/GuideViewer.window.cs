using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Localization;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.GuideSection;

namespace KikoGuide.UI.Windows.GuideViewer
{
    public sealed class GuideViewerWindow : Window, IDisposable
    {
        internal GuideViewerPresenter Presenter = new();

        public GuideViewerWindow() : base(WindowManager.GuideViewerWindowName)
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

        private static bool CanShowExtendedInfo(Guide guide) => ImGui.GetWindowWidth() - ImGui.CalcTextSize(TGuideViewer.GuideHeading(guide.Name)).X > 150;

        /// <summary>
        ///     Draws the guide viewer window.
        /// </summary>
        public override void Draw()
        {
            var guide = this.Presenter.SelectedGuide;

            // No guide selected, show non-selected message.
            if (guide == null)
            {
                ImGui.TextWrapped(TStrings.GuideInfoNoneSelected);
                return;
            }

            // Player does not have the guide unlocked and have the setting enabled to hide locked guides.
            if (!guide.IsUnlocked() && GuideViewerPresenter.Configuration.Display.HideLockedGuides)
            {
                ImGui.TextWrapped(TStrings.GuideInfoNotUnlocked);
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

            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 110);
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
