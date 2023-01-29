using System;
using System.Collections.Generic;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.CommandHandling;
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
        [SirenService] internal static LuminaCacheService<Quest> QuestCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<Fate> FateCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderCondition> ContentFinderConditionCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderConditionTransient> ContentFinderConditionTransientCache { get; private set; } = null!;

        // Plugin services
        internal static CommandManager CommandManager { get; private set; } = null!;
        internal static WindowManager WindowManager { get; private set; } = null!;
        internal static LocalizationManager ResourceManager { get; private set; } = null!;
        internal static GuideManager GuideManager { get; private set; } = null!;
        internal static IntegrationManager IntegrationManager { get; private set; } = null!;
        internal static PluginConfiguration Configuration { get; private set; } = null!;

        // Additional services
        private static readonly List<object> ServiceContainer = new();

        /// <summary>
        /// Initializes the service class.
        /// </summary>
        internal static void Initialize(DalamudPluginInterface pluginInterface)
        {
            // Inject services
            SirenCore.InjectServices<Services>();
            pluginInterface.Create<Services>();

            // Create plugin services
            Configuration = PluginConfiguration.Load();
            ResourceManager = LocalizationManager.Instance;
            GuideManager = GuideManager.Instance;
            WindowManager = WindowManager.Instance;
            CommandManager = CommandManager.Instance;
            IntegrationManager = IntegrationManager.Instance;
        }

        /// <summary>
        /// Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            // Dispose of plugin services
            CommandManager.Dispose();
            WindowManager.Dispose();
            GuideManager.Dispose();
            IntegrationManager.Dispose();
            ResourceManager.Dispose();

            // Unregister external services
            foreach (var service in ServiceContainer.ToArray())
            {
                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                ServiceContainer.Remove(service);
            }
        }

        /// <summary>
        /// Registers a class as a service and injects it into the service container.
        /// </summary>
        /// <remarks>
        /// if you do not dispose of the service yourself, it will be disposed of when the plugin is unloaded.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns>The service if it was registered, otherwise false.</returns>
        internal static bool RegisterService<T>() where T : class, new()
        {
            if (ServiceContainer.Find(x => x is T) is not null)
            {
                return false;
            }
            var service = new T();
            ServiceContainer.Add(service);

            BetterLog.Debug($"Registered service: {service.GetType().Name}");

            return true;
        }

        /// <summary>
        /// Gets an external service from the service container if it exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The service if found, otherwise <see langword="null"/>.</returns>
        internal static T? GetService<T>() where T : class => ServiceContainer.Find(x => x is T) as T;

        /// <summary>
        /// Unregisters a service from the service container and disposes of it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal static bool UnregisterService<T>() where T : class
        {
            var service = ServiceContainer.Find(x => x is T);
            if (service is not null)
            {
                ServiceContainer.Remove(service);

                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                BetterLog.Debug($"Unregistered service: {service.GetType().Name}");

                return true;
            }

            return false;
        }
    }
}
