using ImGuiNET;
using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.PluginSettings.TableParts
{
    internal sealed class PluginSettingsSidebar
    {
        public static void Draw(PluginSettingsLogic logic)
        {
            SiGui.Heading(Strings.UserInterface_PluginSettings_Title);

            if (ImGui.Selectable(Strings.UserInterface_PluginSettings_General_Heading, logic.SelectedTab == PluginSettingsLogic.ConfigurationTabs.General))
            {
                logic.SelectedTab = PluginSettingsLogic.ConfigurationTabs.General;
            }
        }
    }
}