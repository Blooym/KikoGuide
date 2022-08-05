namespace KikoGuide.UI.Screens.DutyInfo;

using System;
using KikoGuide.Base;
using KikoGuide.Types;
using KikoGuide.Managers;

sealed public class DutyInfoPresenter : IDisposable
{
    public DutyInfoPresenter()
    {
        PluginService.ClientState.TerritoryChanged += this.OnTerritoryChange;
    }

    public void Dispose()
    {
        PluginService.ClientState.TerritoryChanged -= this.OnTerritoryChange;
    }

    public bool isVisible = false;
    public Duty? selectedDuty = null;

    /// <summary> Handles territory change even and changes the UI state accordingly. </summary>
    public void OnTerritoryChange(object? sender, ushort e)
    {
        var playerDuty = DutyManager.GetPlayerDuty();

        // If the player has entered a duty with data, set the UI to that duty and display it if the 
        // Configuration setting is set to do so.
        if (playerDuty != null && playerDuty?.Bosses?.Count > 0)
        {
            this.selectedDuty = playerDuty;
            if (PluginService.Configuration.autoOpenDuty) this.isVisible = true;
        }

        // If the player has entered a territory that does not have any data, deselect the duty & hide the UI
        else
        {
            this.selectedDuty = null;
            this.isVisible = false;
        }
    }
}