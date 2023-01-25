using KikoGuide.Common;
using KikoGuide.GuideHandling;
using KikoGuide.UserInterface.Interfaces;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerLogic : IWindowLogic
    {
        public static GuideBase? GetCurrentGuide() => Services.GuideManager.CurrentGuide;
        public static void SetCurrentGuide(GuideBase? guide) => Services.GuideManager.CurrentGuide = guide;
    }
}