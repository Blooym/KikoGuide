using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.CommandHandling;
using KikoGuide.Configuration;
using KikoGuide.GuideHandling;
using KikoGuide.Resources;
using KikoGuide.UserInterface;
using Sirensong;

namespace KikoGuide.Common
{
    /// <summary>
    /// Provides access to service instances for Dalamud, Sirensong & The Plugin.
    /// </summary>
    internal sealed class Services
    {
        // Dalamud services
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static ClientState ClientState { get; private set; } = null!;
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; } = null!;
        [PluginService] internal static DataManager Data { get; private set; } = null!;
        [PluginService] internal static Framework Framework { get; private set; } = null!;

        // Plugin services
        internal static CommandManager CommandManager { get; private set; } = null!;
        internal static WindowManager WindowManager { get; private set; } = null!;
        internal static ResourceManager ResourceManager { get; private set; } = null!;
        internal static GuideManager GuideManager { get; private set; } = null!;

        // Other plugin stuff
        internal static PluginConfiguration Configuration { get; private set; } = null!;

        /// <summary>
        /// Initializes the service class.
        /// </summary>
        internal static void Initialize(DalamudPluginInterface pluginInterface)
        {
            SirenCore.InjectServices<Services>();
            pluginInterface.Create<Services>();

            ResourceManager = ResourceManager.Instance;
            GuideManager = GuideManager.Instance;
            WindowManager = WindowManager.Instance;
            CommandManager = CommandManager.Instance;
            Configuration = PluginConfiguration.Load();
        }

        /// <summary>
        /// Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            CommandManager.Dispose();
            WindowManager.Dispose();
            GuideManager.Dispose();
            ResourceManager.Dispose();
        }
    }
}
