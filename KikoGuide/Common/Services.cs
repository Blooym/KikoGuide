using System.Diagnostics.CodeAnalysis;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.Configuration;
using KikoGuide.GuideSystem;
using KikoGuide.Integrations;
using KikoGuide.Resources.Localization;
using KikoGuide.UserInterface;
using Lumina.Excel.GeneratedSheets;
using Sirensong;
using Sirensong.Caching;
using Sirensong.IoC;

namespace KikoGuide.Common
{
    /// <summary>
    ///     Provides access to service instances for Dalamud, Sirensong & The Plugin.
    /// </summary>
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    internal sealed class Services
    {
        /// <inheritdoc cref="MiniServiceContainer" />
        internal static readonly MiniServiceContainer Container = new();

        // Dalamud services
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; set; } = null!;
        [PluginService] internal static ClientState ClientState { get; set; } = null!;
        [PluginService] internal static CommandManager Commands { get; set; } = null!;
        [PluginService] internal static Framework Framework { get; set; } = null!;

        // Sirensong services
        [SirenService] internal static LuminaCacheService<Quest> QuestCache { get; set; } = null!;
        [SirenService] internal static LuminaCacheService<Fate> FateCache { get; set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderCondition> ContentFinderConditionCache { get; set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderConditionTransient> ContentFinderConditionTransientCache { get; set; } = null!;

        // Plugin services
        internal static WindowManager WindowManager { get; private set; } = null!;
        internal static GuideManager GuideManager { get; private set; } = null!;
        internal static IntegrationManager IntegrationManager { get; private set; } = null!;
        internal static PluginConfiguration Configuration { get; private set; } = null!;

        /// <summary>
        ///     Initializes the service class.
        /// </summary>
        internal static void Initialize(DalamudPluginInterface pluginInterface)
        {
            // Inject Dalamud + Sirensong services
            SirenCore.InjectServices<Services>();
            pluginInterface.Create<Services>();

            // Create plugin services and add them to variables for quick access
            Configuration = PluginConfiguration.Load();
            Container.GetOrCreateService<LocalizationManager>();
            GuideManager = Container.GetOrCreateService<GuideManager>();
            WindowManager = Container.GetOrCreateService<WindowManager>();
            Container.GetOrCreateService<CommandHandling.CommandManager>();
            IntegrationManager = Container.GetOrCreateService<IntegrationManager>();
        }

        /// <summary>
        ///     Disposes of the service class.
        /// </summary>
        internal static void Dispose() => Container.Dispose();
    }
}
