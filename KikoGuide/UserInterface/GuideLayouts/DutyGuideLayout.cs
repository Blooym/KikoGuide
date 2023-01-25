using ImGuiNET;
using KikoGuide.GuideHandling;

namespace KikoGuide.UserInterface.GuideLayouts
{
    internal static class DutyGuideLayout
    {
        public static void Draw(Guide guide)
        {
            ImGui.Text($"Guide: {guide.Name}");
            ImGui.Separator();
        }
    }
}