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
        [SirenService] internal static LuminaCacheService<Quest> QuestCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<Fate> FateCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderCondition> ContentFinderConditionCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<ContentFinderConditionTransient> ContentFinderConditionTransientCache { get; private set; } = null!;
        [SirenService] internal static ClipboardService Clipboard { get; private set; } = null!;

        // Plugin services
        internal static WindowManager WindowManager { get; private set; } = null!;
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
            // Inject Dalamud + Sirensong services
            SirenCore.InjectServices<Services>();
            pluginInterface.Create<Services>();

            // Create plugin services and add them to variables for quick access
            Configuration = PluginConfiguration.Load();
            GetOrCreateService<LocalizationManager>();
            GuideManager = GetOrCreateService<GuideManager>();
            WindowManager = GetOrCreateService<WindowManager>();
            GetOrCreateService<CommandManager>();
            IntegrationManager = GetOrCreateService<IntegrationManager>();
        }

        /// <summary>
        /// Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            // Unregister all services
            foreach (var service in ServiceContainer.ToArray())
            {
                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                    BetterLog.Debug($"Disposed of service: {service.GetType().Name}");
                }

                BetterLog.Debug($"Unregistered service: {service.GetType().Name}");
                ServiceContainer.Remove(service);
            }
        }

        /// <summary>
        /// Gets a service from the service container if it exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The service if found, otherwise <see langword="null"/>.</returns>
        internal static T? GetService<T>() where T : class => ServiceContainer.Find(x => x is T) as T;

        /// <summary>
        /// Creates the service if it does not exist, returns the service either way.
        /// </summary>
        /// <remarks>
        /// If you do not dispose of the service yourself, it will be disposed of when the plugin is unloaded.
        /// </remarks>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service that was created or found.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service could not be created.</exception>
        internal static T GetOrCreateService<T>() where T : class
        {
            var existingService = GetService<T>();
            if (existingService is not null)
            {
                return existingService;
            }

            BetterLog.Debug($"Creating service: {typeof(T).FullName}");

            if (Activator.CreateInstance(typeof(T), true) is not T service)
            {
                throw new InvalidOperationException($"Could not create service of type {typeof(T).FullName}.");
            }

            ServiceContainer.Add(service);
            BetterLog.Debug($"Service created: {service.GetType().Name}");
            return service;
        }

        /// <summary>
        /// Removes a service from the service container if it exists and disposes of it if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if removal was successful, otherwise false.</returns>
        internal static bool RemoveService<T>() where T : class
        {
            var service = ServiceContainer.Find(x => x is T);
            if (service is not null)
            {
                ServiceContainer.Remove(service);

                if (service is IDisposable disposable)
                {
                    BetterLog.Debug($"Disposing service: {service.GetType().Name}");
                    disposable.Dispose();
                }

                BetterLog.Debug($"Unregistered service: {service.GetType().Name}");
                return true;
            }
            return false;
        }
    }
}
