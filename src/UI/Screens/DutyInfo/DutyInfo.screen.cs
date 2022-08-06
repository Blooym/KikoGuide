namespace KikoGuide.UI.Screens.DutyInfo;

using System;
using System.Numerics;
using CheapLoc;
using ImGuiNET;
using KikoGuide.Base;
using KikoGuide.Interfaces;
using KikoGuide.UI.Components.Duty;

sealed public class DutyInfoScreen : IScreen
{
    public DutyInfoPresenter presenter = new DutyInfoPresenter();

    public void Draw() => DrawInfoWindow();
    public void Dispose() => this.presenter.Dispose();
    public void Show() => this.presenter.isVisible = true;
    public void Hide() => this.presenter.isVisible = false;

    /// <summary> Draws the duty info window. </summary>
    private void DrawInfoWindow()
    {
        if (!presenter.isVisible) return;

        var selectedDuty = presenter.selectedDuty;

        ImGui.SetNextWindowSize(new Vector2(380, 420), ImGuiCond.FirstUseEver);
        if (ImGui.Begin(String.Format(Loc.Localize("UI.Screens.SettingDutyInfo.Title", "{0} - Duty Information"), PluginStrings.pluginName), ref presenter.isVisible, ImGuiWindowFlags.NoScrollbar))
        {

            if (selectedDuty == null || selectedDuty.Bosses == null)
            {
                ImGui.Text(Loc.Localize("UI.Screens.SettingDutyInfo.NoDuty", "No duty selected, use /kikolist to see all available duties."));
                return;
            }

            DutyHeadingComponent.Draw(selectedDuty);
            foreach (var boss in selectedDuty.Bosses) DutyBossComponent.Draw(boss);
            ImGui.End();
        }
    }
}
