using ImGuiNET;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    internal static class InstanceContentGuideLayout
    {
        public static void Draw(InstanceContentGuideBase guide)
        {
            // No sections
            if (guide.Content.Sections == null)
            {
                return;
            }

            // Tab bar for sections
            if (ImGui.BeginTabBar("Sections"))
            {
                // Draw each section
                foreach (var section in guide.Content.Sections)
                {
                    DrawSection(section);
                }

                ImGui.EndTabBar();
            }
        }

        private static void DrawSection(InstanceContentGuideFormat.Section section)
        {
            if (ImGui.BeginTabItem(section.Title.UICurrent))
            {
                ImGui.Text(section.Title.EN);
                ImGui.EndTabItem();
            }
        }
    }
}