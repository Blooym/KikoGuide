namespace KikoGuide.IPC
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using Dalamud.Logging;
    using KikoGuide.Base;

    /// <summary>
    ///     Controls all IPC providers and handles loading and unloading them.
    /// </summary>
    sealed public class IPCLoader : IDisposable
    {
        /// <summary> 
        ///     All of the currently registered IPC providers alongside their ID.
        /// </summary>
        private Dictionary<IPCProviders, IIPCProvider> _ipcProviders = new Dictionary<IPCProviders, IIPCProvider>();

        /// <summary>
        ///     Initializes the IPCLoader and loads all IPC providers.
        /// </summary>
        public IPCLoader()
        {
            PluginLog.Debug("IPCLoader(Constructor): Beginning detection of IPC providers...");

            // Get every IPC provider in the assembly and attempt to initialize it.
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIPCProvider))))
            {
                try
                {
                    if (type == null) continue;

                    PluginLog.Debug($"IPCLoader(Constructor): Found  {type.FullName} - Attempting to Initialize");
                    var ipc = Activator.CreateInstance(type);

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

            foreach (var ipc in _ipcProviders.Values)
            {
                try
                {
                    PluginLog.Debug($"IPCLoader(Dispose): Disposing of IPC provider {ipc.ID}...");
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
        public bool GetStatus(IPCProviders provider) => _ipcProviders.ContainsKey(provider);

        /// <summary>
        ///     Enables an IPC provider.
        /// </summary>
        /// <param name="provider">The provider to enable.</param>
        public void EnableProvider(IPCProviders provider)
        {
            if (_ipcProviders.ContainsKey(provider)) return;

            try
            {
                var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IIPCProvider)));
                if (type == null) return;

                var ipc = Activator.CreateInstance(type);
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
            if (!_ipcProviders.ContainsKey(provider)) return;

            try
            {
                var ipc = _ipcProviders[provider];
                ipc.Dispose();
                _ipcProviders.Remove(provider);
                PluginLog.Debug($"IPCLoader(DisableProvider): Disabled and disposed of {provider}.");
            }
            catch (Exception e) { PluginLog.Error($"IPCLoader(DisableProvider): Failed to disable and dispose of {provider} - {e.Message}"); }
        }
    }
}