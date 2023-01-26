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
            this.Size = new(400, 600);
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlags.NoScrollbar;
        }

        public override bool DrawConditions() => GuideViewerLogic.GetCurrentGuide() != null;

        public override void OnClose() => GuideViewerLogic.SetCurrentGuide(null);

        public override void Draw() => GuideViewerLogic.GetCurrentGuide()?.Draw();
    }
}
