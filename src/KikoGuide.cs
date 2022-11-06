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
        public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<PluginService>();
            PluginService.Initialize();
        }

        public void Dispose() => PluginService.Dispose();
    }
}
