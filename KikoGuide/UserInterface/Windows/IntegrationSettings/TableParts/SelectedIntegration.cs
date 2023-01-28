using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.IntegrationSettings.TableParts
{
    internal static class SelectedIntegration
    {
        /// <summary>
        /// Draws the selected integration.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(IntegrationSettingsLogic logic)
        {
            if (logic.SelectedIntegration == null)
            {
                return;
            }

            SiGui.TextHeading(logic.SelectedIntegration.Configuration.Name);
            logic.SelectedIntegration.Draw();
        }
    }
}