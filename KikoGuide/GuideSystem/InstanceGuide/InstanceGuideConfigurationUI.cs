using ImGuiNET;
using KikoGuide.Resources.Localization;

namespace KikoGuide.GuideSystem.InstanceGuide
{
    internal static class InstanceGuideConfigurationUI
    {
        public static void Draw(InstanceGuideConfiguration config)
        {
            var autoOpen = config.AutoOpen;

            if (ImGui.Checkbox(Strings.Guide_InstanceContent_Config_AutoOpen, ref autoOpen))
            {
                config.AutoOpen = autoOpen;
                config.Save();
            }
        }
    }
}
