using System;
using System.Collections.Generic;
using KikoGuide.Base;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.Windows.GuideViewer;

namespace KikoGuide.UI.Windows.GuideList
{
    public sealed class GuideListPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Gets the guide list from the GuideManager.
        /// </summary>
        public static List<Guide> GetGuides() => GuideManager.GetGuides();

        /// <summary>
        ///     Handles a guide list selection event.
        /// </summary>
        public static void OnGuideListSelection(Guide guide)
        {
            if (PluginService.WindowManager.WindowSystem.GetWindow(WindowManager.GuideViewerWindowName) is GuideViewerWindow guideViewerWindow)
            {
                guideViewerWindow.IsOpen = true;
                guideViewerWindow.Presenter.SelectedGuide = guide;
            }
        }

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration GetConfiguration() => PluginService.Configuration;
    }
}