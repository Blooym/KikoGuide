using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.IPC.Interfaces;

namespace KikoGuide.IPC
{
    /// <summary>
    ///     Controls all IPC providers and handles loading and unloading them.
    /// </summary>
    public sealed class IPCLoader : IDisposable
    {
        /// <summary>
        ///     All of the currently registered IPC providers alongside their ID.
        /// </summary>
        private readonly Dictionary<IPCProviders, IIPCProvider> ipcProviders = new();

        /// <summary>
        ///     Initializes the IPCLoader and loads all enabled IPC providers.
        /// </summary>
        public IPCLoader()
        {
            PluginLog.Debug("IPCLoader(Constructor): Beginning detection of IPC providers");

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIPCProvider))))
            {
                try
                {
                    PluginLog.Debug($"IPCLoader(Constructor): Found {type.FullName} - Going to try to initialize it");
                    var ipc = Activator.CreateInstance(type);

                    if (ipc is IIPCProvider provider)
                    {
                        if (!PluginService.Configuration.IPC.EnabledIntegrations.Contains(provider.ID))
                        {
                            PluginLog.Debug($"IPCLoader(Constructor): {type.FullName} is disabled in the configuration, skipped");
                            continue;
                        }

                        provider.Enable();
                        this.ipcProviders.Add(provider.ID, provider);
                        PluginLog.Information($"IPCLoader(Constructor): Integration for {type.FullName} initialized.");
                    }
                }
                catch (Exception e) { PluginLog.Error($"IPCManager(Constructor): Failed to initialize {type.FullName} - {e.Message}"); }
            }
            PluginLog.Debug("IPCLoader(Constructor): Finished detecting IPC providers & initializing.");
        }

        /// <summary>
        ///      Disposes of the IPCLoader and all integrations.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("IPCLoader(Dispose): Disposing of all IPC providers");

            foreach (var ipc in this.ipcProviders.Values)
            {
                try
                {
                    ipc.Dispose();
                    PluginLog.Debug($"IPCLoader(Dispose): Disposed of IPC provider {ipc.ID}.");
                }
                catch (Exception e) { PluginLog.Error($"IPCLoader(Dispose): Failed to dispose of IPC provider {ipc.ID} - {e.Message}"); }
            }

            PluginLog.Debug("IPCLoader(Dispose): Successfully disposed.");
        }

        /// <summary>
        ///     Gets the status of an IPC provider.
        /// </summary>
        /// <param name="provider">The provider to check.</param>
        /// <returns>True if the provider is enabled, false otherwise.</returns>
        public bool GetStatus(IPCProviders provider) => this.ipcProviders.ContainsKey(provider);

        /// <summary>
        ///     Enables an IPC provider.
        /// </summary>
        /// <param name="provider">The provider to enable.</param>
        public void EnableProvider(IPCProviders provider)
        {
            if (this.ipcProviders.ContainsKey(provider))
            {
                return;
            }

            try
            {
                var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IIPCProvider)));
                if (type == null)
                {
                    return;
                }

                var ipc = Activator.CreateInstance(type);
                if (ipc is IIPCProvider ipcProvider)
                {
                    ipcProvider.Enable();
                    this.ipcProviders.Add(ipcProvider.ID, ipcProvider);
                    PluginLog.Information($"IPCLoader(EnableProvider): Integration for {type.FullName} enabled and initialized.");
                }
            }
            catch (Exception e) { PluginLog.Error($"IPCLoader(EnableProvider): Failed to initialize {provider} - {e.Message}"); }
        }

        /// <summary>
        ///     Disables an IPC provider.
        /// </summary>
        /// <param name="provider">The provider to disable.</param>
        public void DisableProvider(IPCProviders provider)
        {
            if (!this.ipcProviders.ContainsKey(provider))
            {
                return;
            }
            try
            {
                var ipc = this.ipcProviders[provider];
                ipc.Dispose();
                this.ipcProviders.Remove(provider);
                PluginLog.Information($"IPCLoader(DisableProvider): Integration for {provider} disabled and disposed.");
            }
            catch (Exception e) { PluginLog.Error($"IPCLoader(DisableProvider): Failed to disable and dispose of {provider} - {e.Message}"); }
        }
    }
}
