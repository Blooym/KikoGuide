namespace KikoGuide.Base;

using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Logging;
using Dalamud.Game.ClientState;
using KikoGuide.Managers;

/// <summary> Provides access to necessary instances and services. </summary>
#pragma warning disable CS8618 // Injection is handled by the Dalamud Plugin Framework here.
sealed internal class PluginService
{
    // Dalamud Services & Instances
    [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; }
    [PluginService] internal static ClientState ClientState { get; private set; }

    // Internal Services & Instances
    internal static CommandManager CommandManager { get; private set; }
    internal static WindowManager WindowManager { get; private set; }
    internal static ResourceManager ResourceManager { get; private set; }
    internal static IPCManager IPCManager { get; private set; }
    internal static Configuration Configuration { get; private set; }

    /// <summary> Initializes the service class. </summary>
    internal static void Initialize()
    {
        PluginLog.Debug("PluginService: Initializing...");

        // Create services and instances
        ResourceManager = new ResourceManager();
        Configuration = PluginInterface?.GetPluginConfig() as Configuration ?? new Configuration();
        IPCManager = new IPCManager();
        WindowManager = new WindowManager();
        CommandManager = new CommandManager();

        PluginLog.Debug("PluginService: Successfully initialized.");
    }

    /// <summary> Disposes of the service class. </summary>
    internal static void Dispose()
    {
        PluginLog.Debug("PluginService: Disposing...");

        IPCManager.Dispose();
        ResourceManager.Dispose();
        WindowManager.Dispose();
        CommandManager.Dispose();

        PluginLog.Debug("PluginService: Successfully disposed.");
    }
}