using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.CommandHandling;
using KikoGuide.Configuration;
using KikoGuide.GuideSystem;
using KikoGuide.Resources.Localization;
using KikoGuide.UserInterface;
using Lumina.Excel.GeneratedSheets;
using Sirensong;
using Sirensong.Caching;
using Sirensong.IoC;
using Sirensong.UserInterface.Services;

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
        [PluginService] internal static Framework Framework { get; private set; } = null!;

        // Sirensong services
        [SirenService] internal static ClipboardService ClipboardService { get; private set; } = null!;
        [SirenService] internal static ImageCacheService ImageCacheService { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<Quest> QuestCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderCondition> ContentFinderConditionCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderConditionTransient> ContentFinderConditionTransientCache { get; private set; } = null!;

        // Plugin services
        internal static CommandManager CommandManager { get; private set; } = null!;
        internal static WindowManager WindowManager { get; private set; } = null!;
        internal static LocalizationManager ResourceManager { get; private set; } = null!;
        internal static GuideManager GuideManager { get; private set; } = null!;
        internal static PluginConfiguration Configuration { get; private set; } = null!;

        /// <summary>
        /// Initializes the service class.
        /// </summary>
        internal static void Initialize(DalamudPluginInterface pluginInterface)
        {
            SirenCore.InjectServices<Services>();
            pluginInterface.Create<Services>();

            ResourceManager = LocalizationManager.Instance;
            Configuration = PluginConfiguration.Load();
            GuideManager = GuideManager.Instance;
            WindowManager = WindowManager.Instance;
            CommandManager = CommandManager.Instance;
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
