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
using KikoGuide.UI.ImGuiFullComponents.DutyInfo;

namespace KikoGuide.UI.Windows.DutyInfo
{
    public sealed class DutyInfoWindow : Window, IDisposable
    {
        internal DutyInfoPresenter Presenter = new();

        public DutyInfoWindow() : base(WindowManager.DutyInfoWindowName)
        {
            this.Flags |= ImGuiWindowFlags.NoScrollbar;

            this.Size = new Vector2(380, 420);
            this.SizeCondition = ImGuiCond.FirstUseEver;

            if (DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowMovement)
            {
                this.Flags |= ImGuiWindowFlags.NoMove;
            }

            if (DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowResize)
            {
                this.Flags |= ImGuiWindowFlags.NoResize;
            }
        }

        public void Dispose() => this.Presenter.Dispose();

        private static bool CanShowExtendedInfo(Duty duty) => ImGui.GetWindowWidth() - ImGui.CalcTextSize(TStrings.DutyHeadingTitle(duty.Name)).X > 150;

        /// <summary>
        ///     Draws the duty info window.
        /// </summary>
        public override void Draw()
        {
            var duty = this.Presenter.SelectedDuty;

            if (duty == null)
            { ImGui.TextWrapped(TStrings.DutyInfoNoneSelected); return; }
            if (!duty.IsUnlocked())
            { ImGui.TextWrapped(TStrings.DutyInfoNotUnlocked); return; }

            Colours.TextWrappedColoured(Colours.Grey, TStrings.DutyHeadingTitle(duty.Name));
            if (CanShowExtendedInfo(duty))
            { ImGui.SameLine(); this.DrawHeaderButtons(); }
            ImGui.Separator();

            if (duty.Sections == null || duty.Sections.Count == 0)
            {
                ImGui.TextWrapped("There is no information available for this duty.");
                return;
            }
            DutyInfoComponent.Draw(duty.Sections);
        }

        private void DrawHeaderButtons()
        {
            var dutyWindowNoMove = DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowMovement;
            var dutyWindowNoResize = DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowResize;

            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 130);
            if (ImGuiComponents.IconButton(dutyWindowNoMove ? FontAwesomeIcon.Lock : FontAwesomeIcon.Unlock))
            {
                this.Flags ^= ImGuiWindowFlags.NoMove;
                DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowMovement ^= true;
                DutyInfoPresenter.Configuration.Save();
            }
            Common.AddTooltip(dutyWindowNoMove ? "Unlock Window Movement" : "Lock Window Movement");

            ImGui.SameLine();
            if (ImGuiComponents.IconButton(dutyWindowNoResize ? FontAwesomeIcon.Compress : FontAwesomeIcon.Expand))
            {
                this.Flags ^= ImGuiWindowFlags.NoResize;
                DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowResize ^= true;
                DutyInfoPresenter.Configuration.Save();
            }
            Common.AddTooltip(dutyWindowNoResize ? "Unlock Window Resizing" : "Lock Window Resizing");

            ImGui.SameLine();
            if (ImGuiComponents.IconButton(FontAwesomeIcon.Cog))
            {
                DutyInfoPresenter.ToggleSettingsWindow();
            }
            Common.AddTooltip("Toggle Settings Window");
        }
    }

}