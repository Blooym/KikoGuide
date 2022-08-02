namespace KikoGuide.UI.Screens.DutyInfo;

using System;
using System.Numerics;
using CheapLoc;
using ImGuiNET;
using KikoGuide.Base;
using KikoGuide.UI.Components.Duty;

sealed class DutyInfoScreen : IDisposable
{
    public DutyInfoPresenter presenter = new DutyInfoPresenter();

    /// <summary> Disposes of the Duty info screen and any resources it uses. </summary>
    public void Dispose() => this.presenter.Dispose();

    /// <summary> Draws all UI elements associated with the DutyInfo screen. </summary>
    public void Draw() => DrawInfoWindow();

    /// <summary> Draws the duty info window. </summary>
    private void DrawInfoWindow()
    {
        if (!presenter.isVisible) return;

        var selectedDuty = presenter.selectedDuty;

        ImGui.SetNextWindowSize(new Vector2(380, 420), ImGuiCond.FirstUseEver);
        if (ImGui.Begin(String.Format(Loc.Localize("UI.DutyInfo.Title", "{0} - Duty Information"), PStrings.pluginName), ref presenter.isVisible, ImGuiWindowFlags.NoScrollbar))
        {

            if (selectedDuty == null || selectedDuty.Bosses == null)
            {
                ImGui.Text(Loc.Localize("UI.DutyInfo.NoDuty", "No duty selected, use /kikolist to see all available duties."));
                return;
            }

            DutyHeadingComponent.Draw(selectedDuty);
            DutyBossListComponent.Draw(selectedDuty.Bosses);
            ImGui.End();
        }
    }
}
