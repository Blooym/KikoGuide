using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.UserInterface.Windows.PluginSettings.TableParts
{
    internal sealed class PluginSettingsActive
    {
        public static void Draw(PluginSettingsLogic logic)
        {
            switch (logic.SelectedTab)
            {
                case PluginSettingsLogic.ConfigurationTabs.General:
                    DrawGeneral(logic);
                    break;
                case PluginSettingsLogic.ConfigurationTabs.Debug:
                    DrawDebug(logic);
                    break;
                default:
                    break;
            }
        }

        private static void DrawGeneral(PluginSettingsLogic _)
        {
            SiGui.Heading(Strings.UserInterface_PluginSettings_General_Heading);
            SiGui.TextWrapped(Strings.UserInterface_PluginSettings_NoConfig);
        }

        private static void DrawDebug(PluginSettingsLogic _)
        {


            SiGui.Heading("Debug");
            SiGui.TextWrapped("The information below should be attached alongside any bug reports/support requests to help diagnose any problems you may be having, you can see a preview of the information below before copying it to your clipboard if you wish.");
            ImGui.Dummy(Spacing.CollapsibleHeaderSpacing);
            if (ImGui.Button("Copy Debug Information"))
            {
                Services.Clipboard.Copy(Constants.Build.DebugString);
            }
            ImGui.Dummy(Spacing.SectionSpacing);

            SiGui.Heading("Detected Information");

            if (ImGui.BeginChild("DebugInformation"))
            {
                SiGui.TextWrapped(Constants.Build.DebugString);
            };
            ImGui.EndChild();
        }
    }
}