using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.UserInterface.Windows
{
    public sealed class GuideViewerWindow : Window
    {
        public GuideViewerWindow() : base(Constants.Windows.GuideViewerTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw() { }
    }
}
