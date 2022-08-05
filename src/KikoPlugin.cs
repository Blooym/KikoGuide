namespace KikoGuide;

using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.Base;

internal class KikoPlugin : IDalamudPlugin
{
    public string Name => PluginStrings.pluginName;

    public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        // Initialize plugin services & managers.
        pluginInterface.Create<PluginService>();
        PluginService.Initialize();
        PluginWindowManager.Initialize();
        PluginCommandManager.Initialize();
        PluginResourceManager.Initialize();
#if !DEBUG
        UpdateManager.UpdateResources();
#endif
    }

    ///<summary> Handles disposing of all resources used by the plugin. </summary>
    public void Dispose()
    {
        PluginWindowManager.Dispose();
        PluginCommandManager.Dispose();
        PluginResourceManager.Dispose();
    }
}
