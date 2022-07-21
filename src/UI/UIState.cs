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
        if (!Service.Configuration.autoOpenDuty) return;

        var playerDuty = DutyManager.GetPlayerDuty();

        if (playerDuty != null || playerDuty?.Bosses != null)
        {
            UIState.dutyInfoVisible = true;
            UIState.SelectedDuty = DutyManager.GetPlayerDuty();
        }

        else UIState.dutyInfoVisible = false;
    }
}