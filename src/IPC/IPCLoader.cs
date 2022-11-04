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
        private readonly Dictionary<IPCProviders, IIPCProvider> _ipcProviders = new();

        /// <summary>
        ///     Initializes the IPCLoader and loads all IPC providers.
        /// </summary>
        public IPCLoader()
        {
            PluginLog.Debug("IPCLoader(Constructor): Beginning detection of IPC providers...");

            // Get every IPC provider in the assembly and attempt to initialize it.
            foreach (Type? type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIPCProvider))))
            {
                try
                {
                    if (type == null)
                    {
                        continue;
                    }

                    PluginLog.Debug($"IPCLoader(Constructor): Found  {type.FullName} - Attempting to Initialize");
                    object? ipc = Activator.CreateInstance(type);

                    if (ipc is IIPCProvider provider)
                    {
                        if (!PluginService.Configuration.IPC.EnabledIntegrations.Contains(provider.ID))
                        {
                            PluginLog.Debug($"IPCLoader(Constructor): {type.FullName} is disabled in the configuration. Skipping...");
                            continue;
                        }

                        provider.Enable();
                        _ipcProviders.Add(provider.ID, provider);
                        PluginLog.Debug($"IPCLoader(Constructor): Finished initializing {type.FullName}");
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
            PluginLog.Debug("IPCLoader(Dispose): Disposing of all IPC providers...");

            foreach (IIPCProvider ipc in _ipcProviders.Values)
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
        public bool GetStatus(IPCProviders provider)
        {
            return _ipcProviders.ContainsKey(provider);
        }

        /// <summary>
        ///     Enables an IPC provider.
        /// </summary>
        /// <param name="provider">The provider to enable.</param>
        public void EnableProvider(IPCProviders provider)
        {
            if (_ipcProviders.ContainsKey(provider))
            {
                return;
            }

            try
            {
                Type? type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IIPCProvider)));
                if (type == null)
                {
                    return;
                }

                object? ipc = Activator.CreateInstance(type);
                if (ipc is IIPCProvider ipcProvider)
                {
                    ipcProvider.Enable();
                    _ipcProviders.Add(ipcProvider.ID, ipcProvider);
                    PluginLog.Debug($"IPCLoader(EnableProvider): Finished initializing {type.FullName}");
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
            if (!_ipcProviders.ContainsKey(provider))
            {
                return;
            }

            try
            {
                IIPCProvider ipc = _ipcProviders[provider];
                ipc.Dispose();
                _ = _ipcProviders.Remove(provider);
                PluginLog.Debug($"IPCLoader(DisableProvider): Disabled and disposed of {provider}.");
            }
            catch (Exception e) { PluginLog.Error($"IPCLoader(DisableProvider): Failed to disable and dispose of {provider} - {e.Message}"); }
        }
    }
}