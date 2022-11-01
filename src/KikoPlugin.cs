namespace KikoGuide
{
    using Dalamud.IoC;
    using Dalamud.Plugin;
    using KikoGuide.Base;

    sealed internal class KikoPlugin : IDalamudPlugin
    {
        /// <summary> 
        ///     The plugin name, fetched from PStrings.
        /// </summary>
        public string Name => PStrings.pluginName;

        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<PluginService>();
            PluginService.Initialize();

#if !DEBUG
        PluginService.ResourceManager.UpdateResources();
#endif
        }

        /// <summary>
        ///     Handles disposing of all resources used by the plugin.
        /// </summary>
        public void Dispose() => PluginService.Dispose();
    }
}