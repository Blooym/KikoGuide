using ImGuiNET;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.PluginSettings.TableParts
{
    internal sealed class PluginSettingsSidebar
    {
        public static void Draw(PluginSettingsLogic logic)
        {
            SiGui.Heading("Plugin Settings");

            if (ImGui.Selectable("General", logic.SelectedTab == PluginSettingsLogic.ConfigurationTabs.General))
            {
                logic.SelectedTab = PluginSettingsLogic.ConfigurationTabs.General;
            }
        }
    }
}