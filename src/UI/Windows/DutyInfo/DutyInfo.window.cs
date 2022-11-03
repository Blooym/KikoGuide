namespace KikoGuide.UI.Windows.DutyInfo
{
    using System.Numerics;
    using ImGuiNET;
    using KikoGuide.Localization;
    using KikoGuide.Managers;
    using KikoGuide.UI.ImGuiFullComponents.DutyInfo;
    using System;
    using Dalamud.Interface.Windowing;


    sealed public class DutyInfoWindow : Window, IDisposable
    {
        public DutyInfoPresenter presenter = new DutyInfoPresenter();

        public DutyInfoWindow() : base(WindowManager.DutyInfoWindowName)
        {
            Flags |= ImGuiWindowFlags.NoScrollbar;

            Size = new Vector2(380, 420);
            SizeCondition = ImGuiCond.FirstUseEver;
        }

        public void Dispose() => this.presenter.Dispose();

        /// <summary>
        ///     Draws the duty info window.
        /// </summary>
        public override void Draw()
        {
            var selectedDuty = presenter.selectedDuty;

            if (selectedDuty == null)
            {
                ImGui.TextWrapped(TStrings.DutyInfoNoneSelected);
                return;
            }

            if (!DutyManager.IsUnlocked(selectedDuty))
            {
                ImGui.TextWrapped(TStrings.DutyInfoNotUnlocked);
                return;
            }

            DutyInfoComponent.Draw(selectedDuty);
        }
    }
}