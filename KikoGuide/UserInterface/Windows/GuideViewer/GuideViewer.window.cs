using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerWindow : Window
    {
        public GuideViewerLogic Logic { get; } = new();
        public GuideViewerWindow() : base(Constants.Windows.GuideViewerTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void OnClose() => GuideViewerLogic.SetCurrentGuide(null);

        public override void Draw()
        {
            var currentGuide = GuideViewerLogic.GetCurrentGuide();

            if (currentGuide == null)
            {
                DrawNoGuideSelected();
                return;
            }

            currentGuide.Draw();
        }

        private static void DrawNoGuideSelected()
        {
            ImGui.TextUnformatted("No guide selected.");
            ImGui.TextUnformatted("Select a guide from the list to view it.");
        }
    }
}
