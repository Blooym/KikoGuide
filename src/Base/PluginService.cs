namespace KikoGuide.Base;

using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Logging;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;

#pragma warning disable CS8618 // Injection is handled by the Dalamud Plugin Framework here.

/// <summary> Provides access to necessary instances and services. </summary>
sealed internal class PluginService
{
    // Dalamud Services & Instances
    [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] internal static CommandManager Commands { get; private set; }
    [PluginService] internal static ClientState ClientState { get; private set; }
    internal static Configuration Configuration { get; private set; }

    /// <summary> Initializes the service class. </summary>
    internal static void Initialize()
    {
        PluginLog.Debug("PluginService: Initializing...");

        Configuration = PluginInterface?.GetPluginConfig() as Configuration ?? new Configuration();

        PluginLog.Debug("PluginService: Successfully initialized.");
    }
}