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
        public static List<Duty> GetDuties() => PluginService.DutyManager.GetDuties();

        /// <summary>
        ///     Handles a duty list selection event.
        /// </summary>
        public static void OnDutyListSelection(Duty duty)
        {
            if (PluginService.WindowManager.WindowSystem.GetWindow(WindowManager.DutyInfoWindowName) is DutyInfoWindow dutyInfoWindow)
            {
                dutyInfoWindow.IsOpen = true;
                dutyInfoWindow.Presenter.SelectedDuty = duty;
            }
        }

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration GetConfiguration() => PluginService.Configuration;
    }
}