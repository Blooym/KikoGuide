using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows
{
    public class SettingsWindow : Window
    {
        public SettingsWindow() : base(Constants.Windows.SettingsTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw()
        {
            var guides = Services.ResourceManager.GetAllGuides();
            foreach (var guide in guides)
            {
                if (guide.LinkedDuty?.CFCondition?.Image != null)
                {
                    SiUI.Icon(guide.LinkedDuty.CFCondition.Image, ScalingMode.Contain);
                }

                ImGui.TextUnformatted(guide.Name.UICurrent);
            }
        }
    }
}
