namespace KikoGuide.Base;

using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;

/// <summary>
///     Service is an internal class that provides internal access to necessary services.
/// </summary>
#pragma warning disable CS8618 // Injection is handled by the Dalamud Plugin Framework.
sealed internal class Service
{
    [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] internal static ClientState ClientState { get; private set; }
    [PluginService] internal static CommandManager CommandManager { get; private set; }
    internal static Configuration Configuration { get; private set; }

    internal static void Initialize(Configuration configuration) => Configuration = configuration;
}