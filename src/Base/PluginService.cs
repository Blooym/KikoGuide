namespace KikoGuide.Base
{
    using Dalamud.IoC;
    using Dalamud.Plugin;
    using Dalamud.Logging;
    using Dalamud.Game.ClientState;
    using Dalamud.Game.ClientState.Conditions;
    using KikoGuide.Managers;
    using KikoGuide.IPC;

    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
#pragma warning disable CS8618 // Injection is handled by the Dalamud Plugin Framework here.
    sealed internal class PluginService
    {
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; }
        [PluginService] internal static ClientState ClientState { get; private set; }
        [PluginService] internal static Condition Condition { get; private set; }

        internal static CommandManager CommandManager { get; private set; }
        internal static WindowManager WindowManager { get; private set; }
        internal static ResourceManager ResourceManager { get; private set; }
        internal static IPCLoader IPCManager { get; private set; }
        internal static Configuration Configuration { get; private set; }

        /// <summary>
        ///     Initializes the service class.
        /// </summary>
        internal static void Initialize()
        {
            ResourceManager = new ResourceManager();
            Configuration = PluginInterface?.GetPluginConfig() as Configuration ?? new Configuration();
            IPCManager = new IPCLoader();
            WindowManager = new WindowManager();
            CommandManager = new CommandManager();
#if !DEBUG
            ResourceManager.UpdateResources();
#endif
            PluginLog.Debug("PluginService(Initialize): Successfully initialized plugin services.");
        }

        /// <summary>
        ///     Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            IPCManager.Dispose();
            ResourceManager.Dispose();
            WindowManager.Dispose();
            CommandManager.Dispose();

            PluginLog.Debug("PluginService(Initialize): Successfully disposed of plugin services.");
        }
    }
}