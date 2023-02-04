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
            SiGui.Heading("Debug Information");
            SiGui.TextWrapped("Send a screenshot of this to the developer if you're having issues.");
            ImGui.Dummy(Spacing.SectionSpacing);

            SiGui.Heading("Git Information");
            SiGui.Text("Branch: " + Constants.Build.GitBranch);
            SiGui.Text("Commit Hash: #" + Constants.Build.GitCommitHash);
            SiGui.Text("Commit Date: " + Constants.Build.GitCommitDate);
            SiGui.Text("Commit Message: " + Constants.Build.GitCommitMessage);
            ImGui.Dummy(Spacing.SectionSpacing);

            SiGui.Heading("Build Information");
            SiGui.Text("Version: " + Constants.Build.VersionInformational);
            SiGui.Text("Build Configuration: " + Constants.Build.BuildConfiguration);
            SiGui.Text("Is Pre-Release: " + Constants.Build.IsPreRelease);
            ImGui.Dummy(Spacing.SectionSpacing);

            SiGui.Heading("Plugin Information");
            SiGui.Text("Source: " + Services.PluginInterface.SourceRepository);
            SiGui.Text("Guides: " + Services.GuideManager.GetGuides().Count);
        }
    }
}