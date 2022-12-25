using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.Base;

namespace KikoGuide
{
    internal sealed class KikoPlugin : IDalamudPlugin
    {
        /// <summary>
        ///     The plugin name, fetched from PluginConstants.
        /// </summary>
        public string Name => PluginConstants.PluginName;

        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        /// <param name="pluginInterface"> The plugin interface. </param>
        public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<PluginService>();
            PluginService.Initialize();
        }

        /// <summary>
        ///     Disposes of the plugin.
        /// </summary>
        public void Dispose() => PluginService.Dispose();
    }
}
