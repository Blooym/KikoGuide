using KikoGuide.Integrations;

namespace KikoGuide.UserInterface.Windows.IntegrationSettings
{
    internal sealed class IntegrationsLogic
    {
        /// <summary>
        /// The currently selected integration to display configuration for.
        /// </summary>
        public IntegrationBase? SelectedIntegration { get; set; }
    }
}