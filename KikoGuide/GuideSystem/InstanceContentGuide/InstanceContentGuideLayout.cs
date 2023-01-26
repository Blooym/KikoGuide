using ImGuiNET;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    internal static class InstanceContentGuideLayout
    {
        public static void Draw(InstanceContentGuide guide)
        {
            ImGui.Text("Content goes here.");
            ImGui.Text(guide.Name);
        }
    }
}