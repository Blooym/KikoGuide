namespace KikoGuide.UI;

using KikoGuide.Managers;
using KikoGuide.Base;

internal class KikoUIState
{
    internal static bool settingsVisible = false;
    internal static bool listVisible = false;
    internal static bool dutyInfoVisible = false;
    internal static Duty? SelectedDuty = DutyManager.GetPlayerDuty();

    // <summary>
    // Handles territory change event & sets the current player duty accordingly.
    // </summary>
    internal static void OnTerritoryChange(object? sender, ushort e)
    {
        if (!Service.Configuration.autoOpenDuty) return;

        var playerDuty = DutyManager.GetPlayerDuty();

        if (playerDuty != null || playerDuty?.Bosses != null)
        {
            KikoUIState.dutyInfoVisible = true;
            KikoUIState.SelectedDuty = DutyManager.GetPlayerDuty();
        }

        else KikoUIState.dutyInfoVisible = false;
    }
}