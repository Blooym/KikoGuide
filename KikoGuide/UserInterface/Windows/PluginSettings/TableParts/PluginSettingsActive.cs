using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.PluginSettings.TableParts
{
    internal sealed class PluginSettingsActive
    {
        public static void Draw(PluginSettingsLogic logic)
        {
            if (logic.SelectedTab == PluginSettingsLogic.ConfigurationTabs.General)
            {
                DrawGeneral(logic);
            }
        }

        private static void DrawGeneral(PluginSettingsLogic _)
        {
            SiGui.Heading(Strings.UserInterface_PluginSettings_General_Heading);
            SiGui.TextWrapped(Strings.UserInterface_PluginSettings_NoConfig);
        }
    }
}