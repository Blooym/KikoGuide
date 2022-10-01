namespace KikoGuide.Base;

using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Logging;
using Dalamud.Game.ClientState;
using KikoGuide.Managers;

/// <summary>
///     Provides access to necessary instances and services.
/// </summary>
#pragma warning disable CS8618 // [PluginService] injections handled by Dalamud.
sealed internal class PluginService
{
    ////////////////////////
    /// Dalamud Services ///
    ////////////////////////

    [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; }
    [PluginService] internal static ClientState ClientState { get; private set; }


    ////////////////////////
    /// Plugin  Services ///
    ////////////////////////

    internal static CommandManager CommandManager { get; private set; }
    internal static WindowManager WindowManager { get; private set; }
    internal static ResourceManager ResourceManager { get; private set; }
    internal static IPCManager IPCManager { get; private set; }
    internal static Configuration Configuration { get; private set; }


    ////////////////////////
    /// Init and Dispose ///
    ////////////////////////

    /// <summary> 
    ///     Initializes the service class and creates plugin instances.
    /// </summary>
    internal static void Initialize()
    {
        PluginLog.Debug("PluginService(Initialize): Initializing...");

        // Create services and instances
        ResourceManager = new ResourceManager();
        Configuration = PluginInterface?.GetPluginConfig() as Configuration ?? new Configuration();
        IPCManager = new IPCManager();
        WindowManager = new WindowManager();
        CommandManager = new CommandManager();

        PluginLog.Debug("PluginService(Initialize): Successfully initialized.");
    }

    /// <summary> 
    ///    Disposes of the service class and its instances.
    /// </summary>
    internal static void Dispose()
    {
        PluginLog.Debug("PluginService(Initialize): Disposing...");

        IPCManager.Dispose();
        ResourceManager.Dispose();
        WindowManager.Dispose();
        CommandManager.Dispose();

        PluginLog.Debug("PluginService(Initialize): Successfully disposed.");
    }
}