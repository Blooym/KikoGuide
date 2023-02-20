using ImGuiNET;
using KikoGuide.Resources.Localization;

namespace KikoGuide.GuideSystem.FateGuide
{
    internal sealed class FateGuideConfigurationUI
    {
        /// <summary>
        ///     Draws the FATE configuration UI.
        /// </summary>
        /// <param name="config"></param>
        public static void Draw(FateGuideConfiguration config)
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
