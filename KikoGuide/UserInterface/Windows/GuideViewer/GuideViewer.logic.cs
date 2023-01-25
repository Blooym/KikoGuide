using KikoGuide.Common;
using KikoGuide.GuideHandling;
using KikoGuide.UserInterface.Interfaces;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerLogic : IWindowLogic
    {
        public static Guide? GetCurrentGuide() => Services.GuideManager.CurrentGuide;
        public static void SetCurrentGuide(Guide? guide) => Services.GuideManager.CurrentGuide = guide;
    }
}