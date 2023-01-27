using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.UserInterface.Windows.Settings
{
    internal sealed class SettingsWindow : Window
    {
        public SettingsLogic Logic { get; } = new();
        public SettingsWindow() : base(Constants.Windows.SettingsTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw()
        {

        }
    }
}
