using System;
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

            foreach (var tab in Enum.GetValues(typeof(PluginSettingsLogic.ConfigurationTabs)))
            {
                if (tab is PluginSettingsLogic.ConfigurationTabs configurationTab)
                {
                    if (ImGui.Selectable(configurationTab.ToString(), logic.SelectedTab == configurationTab))
                    {
                        logic.SelectedTab = configurationTab;
                    }
                }
            }
        }
    }
}
