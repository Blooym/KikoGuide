using KikoGuide.Common;
using KikoGuide.Configuration;

namespace KikoGuide.UserInterface.Windows.Settings
{
    internal sealed class SettingsLogic
    {
        public static PluginConfiguration Configuration { get; } = Services.Configuration;
    }
}