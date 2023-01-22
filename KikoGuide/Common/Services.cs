using Dalamud.Data;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.CommandHandling;
using KikoGuide.Resources;
using KikoGuide.UserInterface;
using Sirensong;

namespace KikoGuide.Common
{
    /// <summary>
    ///     Provides access to service instances for Dalamud, Sirensong & The Plugin.
    /// </summary>
    internal sealed class Services
    {
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static ClientState ClientState { get; private set; } = null!;
        [PluginService] internal static ObjectTable ObjectTable { get; private set; } = null!;
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; } = null!;
        [PluginService] internal static DataManager Data { get; private set; } = null!;

        // Plugin services
        internal static CommandManager CommandManager { get; private set; } = null!;
        internal static WindowManager WindowManager { get; private set; } = null!;
        internal static ResourceManager ResourceManager { get; private set; } = null!;

        /// <summary>
        ///     Initializes the service class.
        /// </summary>
        internal static void Initialize(DalamudPluginInterface pluginInterface)
        {
            SirenCore.InjectServices<Services>();
            pluginInterface.Create<Services>();

            ResourceManager = ResourceManager.Instance;
            WindowManager = WindowManager.Instance;
            CommandManager = CommandManager.Instance;
        }

        /// <summary>
        ///     Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            CommandManager.Dispose();
            WindowManager.Dispose();
            ResourceManager.Dispose();
        }
    }
}
