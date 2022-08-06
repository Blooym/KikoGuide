namespace KikoGuide.Base;

using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Logging;
using Dalamud.Game.ClientState;
using KikoGuide.Managers;

#pragma warning disable CS8618 // Injection is handled by the Dalamud Plugin Framework here.

/// <summary> Provides access to necessary instances and services. </summary>
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
        CommandManager = new CommandManager();
        WindowManager = new WindowManager();
        ResourceManager = new ResourceManager();
        IPCManager = new IPCManager();
        Configuration = PluginInterface?.GetPluginConfig() as Configuration ?? new Configuration();

        PluginLog.Debug("PluginService: Successfully initialized.");
    }

    /// <summary> Disposes of the service class. </summary>
    internal static void Dispose()
    {
        PluginLog.Debug("PluginService: Disposing...");

        CommandManager.Dispose();
        WindowManager.Dispose();
        ResourceManager.Dispose();
        IPCManager.Dispose();

        PluginLog.Debug("PluginService: Successfully disposed.");
    }
}