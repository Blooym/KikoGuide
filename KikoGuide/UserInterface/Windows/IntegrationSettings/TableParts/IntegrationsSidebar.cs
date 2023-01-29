using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.UserInterface.Windows.IntegrationSettings.TableParts
{
    internal static class IntegrationsSidebar
    {
        /// <summary>
        /// Draws the integrations sidebar.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(IntegrationsLogic logic) => DrawIntegrationsList(logic);

        /// <summary>
        /// Draws a list of integrations.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawIntegrationsList(IntegrationsLogic logic)
        {
            var integrations = Services.IntegrationManager.Integrations;

            // About integrations
            if (ImGui.Selectable(Strings.UserInterface_Integrations_About_Heading, logic.SelectedIntegration == null))
            {
                logic.SelectedIntegration = null;
            }
            ImGui.Dummy(Spacing.SidebarElementSpacing);

            // Integrations list
            SiGui.Heading(Strings.UserInterface_Integrations_Title);
            if (integrations.Count == 0)
            {
                DrawNoIntegrations(logic);
                return;
            }
            foreach (var integration in Services.IntegrationManager.Integrations)
            {
                if (ImGui.Selectable(integration.Name, logic.SelectedIntegration == integration))
                {
                    logic.SelectedIntegration = integration;
                }
            }
        }

        /// <summary>
        /// The content to draw when there are no integrations.
        /// </summary>
        /// <param name="_"></param>
        private static void DrawNoIntegrations(IntegrationsLogic _) => SiGui.TextWrapped("No integrations are available at this time.");
    }
}