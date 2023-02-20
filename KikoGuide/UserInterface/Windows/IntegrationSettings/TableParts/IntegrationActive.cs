using ImGuiNET;
using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.UserInterface.Windows.IntegrationSettings.TableParts
{
    internal static class IntegrationActive
    {
        /// <summary>
        ///     Draws the selected integration.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(IntegrationsLogic logic)
        {
            if (logic.SelectedIntegration == null)
            {
                DrawAboutIntegrations(logic);
                return;
            }

            DrawIntegrationSettings(logic);
        }

        /// <summary>
        ///     Draws the about integrations section.
        /// </summary>
        /// <param name="_"></param>
        private static void DrawAboutIntegrations(IntegrationsLogic _)
        {
            SiGui.Heading(Strings.UserInterface_Integrations_About_Heading);
            SiGui.TextWrapped(Strings.UserInterface_Integrations_About_Body);
        }

        /// <summary>
        ///     Draws the selected integration's settings.
        /// </summary>
        /// <param name="_"></param>
        private static void DrawIntegrationSettings(IntegrationsLogic logic)
        {
            if (logic.SelectedIntegration == null)
            {
                return;
            }

            SiGui.Heading(logic.SelectedIntegration.Name);
            SiGui.TextWrapped(logic.SelectedIntegration.Description);
            ImGui.Dummy(Spacing.SectionSpacing);

            SiGui.Heading(Strings.UserInterface_Integrations_Configure_Heading);
            logic.SelectedIntegration.Draw();
        }
    }
}
