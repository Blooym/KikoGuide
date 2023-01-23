using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
namespace KikoGuide.UserInterface.Windows
{
    public sealed class GuideListWindow : Window
    {
        public GuideListWindow() : base(Constants.Windows.GuideListTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw()
        {
        }
    }
}
