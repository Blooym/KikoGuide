using KikoGuide.Common;
using KikoGuide.Configuration;

namespace KikoGuide.UserInterface.Windows.PluginSettings
{
    internal sealed class PluginSettingsLogic
    {
        /// <summary>
        /// Gets the plugin configuration.
        /// </summary>
        public static PluginConfiguration Configuration { get; } = Services.Configuration;
    }
}