using KikoGuide.Common;
using KikoGuide.Configuration;
using KikoGuide.GuideSystem;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerLogic
    {
        /// <summary>
        /// Gets the currently selected guide in the guide manager.
        /// </summary>
        /// <returns>The currently selected guide or null if none is selected.</returns>
        public static GuideBase? GetSelectedGuide() => Services.GuideManager.SelectedGuide;

        /// <summary>
        /// Gets the plugin configuration.
        /// </summary>
        public static PluginConfiguration Configuration => Services.Configuration;
    }
}