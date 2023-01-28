using ImGuiNET;
using KikoGuide.Common;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.IntegrationSettings.TableParts
{
    internal static class IntegrationsSidebar
    {
        /// <summary>
        /// Draws the integrations sidebar.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(IntegrationSettingsLogic logic) => DrawIntegrationsList(logic);

        /// <summary>
        /// Draws a list of integrations.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawIntegrationsList(IntegrationSettingsLogic logic)
        {
            SiGui.TextHeading("Integrations");

            foreach (var integration in Services.IntegrationManager.Integrations)
            {
                if (ImGui.Selectable(integration.Configuration.Name, logic.SelectedIntegration == integration))
                {
                    logic.SelectedIntegration = integration;
                }
            }
        }
    }
}