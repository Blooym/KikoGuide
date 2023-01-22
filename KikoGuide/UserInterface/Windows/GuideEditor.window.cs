using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.UserInterface.Windows
{
    public class GuideEditorWindow : Window
    {
        public GuideEditorWindow() : base(Constants.Windows.GuideEditorTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw() { }
    }
}
