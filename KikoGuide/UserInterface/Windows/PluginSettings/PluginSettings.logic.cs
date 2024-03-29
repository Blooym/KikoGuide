using KikoGuide.Common;
using KikoGuide.Configuration;

namespace KikoGuide.UserInterface.Windows.PluginSettings
{
    internal sealed class PluginSettingsLogic
    {

        /// <summary>
        ///     The available sidebar tabs.
        /// </summary>
        public enum ConfigurationTabs
        {
            General,
            Debug,
        }

        /// <summary>
        ///     The currently selected sidebar tab.
        /// </summary>
        public ConfigurationTabs SelectedTab = ConfigurationTabs.General;

        /// <summary>
        ///     Gets the plugin configuration.
        /// </summary>
        public static PluginConfiguration Configuration { get; } = Services.Configuration;
    }
}
