using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.Base;

namespace KikoGuide
{
    internal sealed class KikoPlugin : IDalamudPlugin
    {
        /// <summary> 
        ///     The plugin name, fetched from PStrings.
        /// </summary>
        public string Name => PluginConstants.pluginName;

        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            _ = pluginInterface.Create<PluginService>();
            PluginService.Initialize();
        }

        public void Dispose()
        {
            PluginService.Dispose();
        }
    }
}