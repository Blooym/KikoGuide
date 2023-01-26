using KikoGuide.Common;
using KikoGuide.GuideSystem;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerLogic
    {
        public static GuideBase? GetCurrentGuide() => Services.GuideManager.CurrentGuide;
        public static void SetCurrentGuide(GuideBase? guide) => Services.GuideManager.CurrentGuide = guide;
    }
}