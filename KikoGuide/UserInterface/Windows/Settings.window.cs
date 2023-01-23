using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.UserInterface.Windows
{
    public sealed class SettingsWindow : Window
    {
        public SettingsWindow() : base(Constants.Windows.SettingsTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw()
        {
            ImGui.Text("Settings");
            ImGui.Text("Coming soon!");
        }
    }
}
