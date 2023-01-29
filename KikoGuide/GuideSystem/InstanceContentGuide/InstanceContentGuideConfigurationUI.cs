using ImGuiNET;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    internal static class InstanceContentGuideConfigurationUI
    {
        public static void Draw(InstanceContentGuideConfiguration config)
        {
            var autoOpen = config.AutoOpen;

            if (ImGui.Checkbox("Automatically open duty guides", ref autoOpen))
            {
                config.AutoOpen = autoOpen;
                config.Save();
            }
        }
    }
}