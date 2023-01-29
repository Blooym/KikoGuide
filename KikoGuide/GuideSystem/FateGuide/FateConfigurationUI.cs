using ImGuiNET;
using KikoGuide.Resources.Localization;

namespace KikoGuide.GuideSystem.FateGuide
{
    internal sealed class FateConfigurationUI
    {
        public static void Draw(FateConfiguration config)
        {
            var autoOpen = config.AutoOpen;
            if (ImGui.Checkbox(Strings.Guide_Fate_Config_AutoOpen, ref autoOpen))
            {
                config.AutoOpen = autoOpen;
                config.Save();
            }
        }
    }
}