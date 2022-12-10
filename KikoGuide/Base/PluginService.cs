using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using KikoGuide.IPC;
using KikoGuide.Managers;

namespace KikoGuide.Base
{
    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
    internal sealed class PluginService
    {
#pragma warning disable CS8618, RCS1170 // Injection is handled by the Dalamud Plugin Framework here, we can ignore theses warnings.
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; }
        [PluginService] internal static ClientState ClientState { get; private set; }
        [PluginService] internal static Framework Framework { get; private set; }

        internal static CommandManager CommandManager { get; private set; }
        internal static WindowManager WindowManager { get; private set; }
        internal static ResourceManager ResourceManager { get; private set; }
        internal static Configuration Configuration { get; private set; }
        internal static IPCLoader IPC { get; private set; }
        internal static GuideManager GuideManager { get; private set; }
#pragma warning restore CS8618, RCS1170

        /// <summary>
        ///     Initializes the service class.
        /// </summary>
        internal static void Initialize()
        {
            ResourceManager = new ResourceManager();
            Configuration = PluginInterface?.GetPluginConfig() as Configuration ?? new Configuration();
            GuideManager = new GuideManager();
            WindowManager = new WindowManager();
            CommandManager = new CommandManager();
            IPC = new IPCLoader();

#if !DEBUG
            ResourceManager.Update();
            Configuration.RemoveInvalidEnumValues();
#endif

            PluginLog.Debug("PluginService(Initialize): Successfully initialized plugin services.");
        }

        /// <summary>
        ///     Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            IPC.Dispose();
            ResourceManager.Dispose();
            WindowManager.Dispose();
            CommandManager.Dispose();

            PluginLog.Debug("PluginService(Initialize): Successfully disposed of plugin services.");
        }
    }
}
