namespace KikoGuide.UI;

using KikoGuide.Managers;
using KikoGuide.Base;

/// <summary>
///     The UIState class is used to manage the global state of the UI.
/// </summary>
internal class UIState
{
    internal static bool settingsVisible = false;
    internal static bool listVisible = false;
    internal static bool dutyInfoVisible = false;
    internal static Duty? SelectedDuty = DutyManager.GetPlayerDuty();

    /// <summary>
    ///     Handles territory change even and changes the UI state accordingly.
    /// </summary>
    internal static void OnTerritoryChange(object? sender, ushort e)
    {
        // Skip if the config says to ignore this.
        if (!Service.Configuration.autoOpenDuty) return;

        // Get the player duty and check if it has valid, if so then display it. (Typically on duty enter)
        var playerDuty = DutyManager.GetPlayerDuty();
        if (playerDuty != null || playerDuty?.Bosses != null)
        {
            UIState.dutyInfoVisible = true;
            UIState.SelectedDuty = DutyManager.GetPlayerDuty();
        }

        // If the player duty does not have any valid data, hide the UI & clear it (typically on duty exit)
        else
        {
            UIState.dutyInfoVisible = false;
            UIState.SelectedDuty = null;
        }
    }
}