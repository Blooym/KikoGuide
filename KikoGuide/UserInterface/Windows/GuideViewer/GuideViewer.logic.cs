using KikoGuide.Common;
using KikoGuide.Configuration;
using KikoGuide.GuideSystem;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerLogic
    {
        public static GuideBase? GetSelectedGuide() => Services.GuideManager.SelectedGuide;
        public static PluginConfiguration Configuration => Services.Configuration;
    }
}