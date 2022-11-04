namespace KikoGuide.UI.Windows.DutyList
{
    using System;
    using System.Collections.Generic;
    using KikoGuide.Base;
    using KikoGuide.Types;
    using KikoGuide.Managers;
    using KikoGuide.UI.Windows.DutyInfo;

    public sealed class DutyListPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Gets the duty list from the duty manager.
        /// </summary>
        public List<Duty> GetDuties() => PluginService.DutyManager.GetDuties();

        /// <summary>
        ///     Handles a duty list selection event.
        /// </summary>
        public void OnDutyListSelection(Duty duty)
        {
            if (PluginService.WindowManager.windowSystem.GetWindow(WindowManager.DutyInfoWindowName) is DutyInfoWindow dutyInfoWindow)
            {
                dutyInfoWindow.IsOpen = true;
                dutyInfoWindow.presenter.selectedDuty = duty;
            }
        }

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal Configuration GetConfiguration() => PluginService.Configuration;
    }
}