using KikoGuide.GuideSystem;

namespace KikoGuide.UserInterface.Windows.GuideViewer.Tabs
{
    internal static class GuideViewerContent
    {
        /// <summary>
        ///     Draws the guide viewer content.
        /// </summary>
        /// <param name="logic">The guide viewer logic.</param>
        /// <param name="guide">The guide to draw.</param>
        public static void Draw(GuideViewerLogic _, GuideBase guide) => guide.Draw();
    }
}
