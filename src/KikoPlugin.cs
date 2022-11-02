namespace KikoGuide
{
    using Dalamud.IoC;
    using Dalamud.Plugin;
    using KikoGuide.Base;
    using KikoGuide.IPC;

    sealed internal class KikoPlugin : IDalamudPlugin
    {
        /// <summary> 
        ///     The plugin name, fetched from PStrings.
        /// </summary>
        public string Name => PStrings.pluginName;

        /// <summary>
        ///     The IPCLoader instance.
        /// </summary>
        public IPCLoader IPCLoader { get; private set; }

        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<PluginService>();
            PluginService.Initialize();

            // Must be initialized after PluginService due to dependency.
            IPCLoader = new();
        }

        public void Dispose()
        {
            IPCLoader.Dispose();
            PluginService.Dispose();
        }

    }
}