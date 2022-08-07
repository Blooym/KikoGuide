namespace KikoGuide;

using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.Base;

internal class KikoPlugin : IDalamudPlugin
{
    public string Name => PStrings.pluginName;

    public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<PluginService>();
        PluginService.Initialize();

#if !DEBUG
        PluginService.RM.Update();
#endif

    }

    ///<summary> Handles disposing of all resources used by the plugin. </summary>
    public void Dispose() => PluginService.Dispose();
}
