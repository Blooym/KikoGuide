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
        public DutyInfoPresenter _presenter = new();

        public DutyInfoWindow() : base(WindowManager.DutyInfoWindowName)
        {
            Flags |= ImGuiWindowFlags.NoScrollbar;

            Size = new Vector2(380, 420);
            SizeCondition = ImGuiCond.FirstUseEver;

            if (DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowMovement)
            {
                Flags |= ImGuiWindowFlags.NoMove;
            }

            if (DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowResize)
            {
                Flags |= ImGuiWindowFlags.NoResize;
            }
        }

        public void Dispose()
        {
            _presenter.Dispose();
        }

        private static bool CanShowExtendedInfo(Duty duty)
        {
            return ImGui.GetWindowWidth() - ImGui.CalcTextSize(TStrings.DutyHeadingTitle(duty.Name)).X > 150;
        }

        /// <summary>
        ///     Draws the duty info window.
        /// </summary>
        public override void Draw()
        {
            Duty? duty = _presenter.selectedDuty;

            if (duty == null)
            { ImGui.TextWrapped(TStrings.DutyInfoNoneSelected); return; }
            if (!duty.IsUnlocked())
            { ImGui.TextWrapped(TStrings.DutyInfoNotUnlocked); return; }

            Colours.TextWrappedColoured(Colours.Grey, TStrings.DutyHeadingTitle(duty.Name));
            if (CanShowExtendedInfo(duty)) { ImGui.SameLine(); DrawHeaderButtons(); }
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
            bool dutyWindowNoMove = DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowMovement;
            bool dutyWindowNoResize = DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowResize;

            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 130);
            if (ImGuiComponents.IconButton(dutyWindowNoMove ? FontAwesomeIcon.Lock : FontAwesomeIcon.Unlock))
            {
                Flags ^= ImGuiWindowFlags.NoMove;
                DutyInfoPresenter.Configuration.Display.PreventDutyInfoWindowMovement ^= true;
                DutyInfoPresenter.Configuration.Save();
            }
            Common.AddTooltip(dutyWindowNoMove ? "Unlock Window Movement" : "Lock Window Movement");

            ImGui.SameLine();
            if (ImGuiComponents.IconButton(dutyWindowNoResize ? FontAwesomeIcon.Compress : FontAwesomeIcon.Expand))
            {
                Flags ^= ImGuiWindowFlags.NoResize;
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