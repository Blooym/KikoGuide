using System;
using System.Collections.Generic;
using KikoGuide.Base;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.Windows.DutyInfo;

namespace KikoGuide.UI.Windows.DutyList
{
    public sealed class DutyListPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Gets the duty list from the duty manager.
        /// </summary>
        public static List<Duty> GetDuties()
        {
            return PluginService.DutyManager.GetDuties();
        }

        /// <summary>
        ///     Handles a duty list selection event.
        /// </summary>
        public static void OnDutyListSelection(Duty duty)
        {
            if (PluginService.WindowManager.windowSystem.GetWindow(WindowManager.DutyInfoWindowName) is DutyInfoWindow dutyInfoWindow)
            {
                dutyInfoWindow.IsOpen = true;
                dutyInfoWindow._presenter.selectedDuty = duty;
            }
        }

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration GetConfiguration()
        {
            return PluginService.Configuration;
        }
    }
}