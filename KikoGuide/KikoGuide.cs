using Dalamud.Plugin;
using KikoGuide.Common;
using Sirensong;

namespace KikoGuide
{
    internal sealed class KikoGuide : IDalamudPlugin
    {
        /// <summary>
        /// The plugin's name.
        /// </summary>
        public string Name => Constants.PluginName;

        /// <summary>
        /// The plugin's main entry point.
        /// </summary>
        public KikoGuide(DalamudPluginInterface pluginInterface)
        {
            SirenCore.Initialize(pluginInterface, this.Name);
            Services.Initialize(pluginInterface);
        }

        /// <summary>
        /// Disposes of the plugin.
        /// </summary>
        public void Dispose()
        {
            SirenCore.Dispose();
            Services.Dispose();
        }
    }
}
