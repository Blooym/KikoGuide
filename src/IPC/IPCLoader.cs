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
        ///     Initializes the IPCManager and loads all IPC providers.
        /// </summary>
        public IPCLoader()
        {
            PluginLog.Debug("IPCManager(IPCManager): Beginning detection of IPC providers...");

            // Get every IPC provider in the assembly and attempt to initialize it.
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIPCProvider))))
            {
                try
                {

                    if (type != null)
                    {
                        PluginLog.Debug($"IPCManager(IPCManager): Found  {type.FullName} - Attempting to Initialize");
                        var ipc = Activator.CreateInstance(type);

                        if (ipc is IIPCProvider provider)
                        {
                            if (!PluginService.Configuration.IPC.EnabledIntegrations.Contains(provider.ID))
                            {
                                PluginLog.Debug($"IPCManager(IPCManager): {type.FullName} is disabled in the configuration. Skipping...");
                                continue;
                            }

                            provider.Enable();
                            _ipcProviders.Add(provider.ID, provider);
                            PluginLog.Debug($"IPCManager(IPCManager): Finished initializing {type.FullName}");
                        }
                    }
                }
                catch (Exception e) { PluginLog.Error($"IPCManager(IPCManager): Failed to initialize {type.FullName} - {e.Message}"); }
            }
            PluginLog.Debug("IPCManager(IPCManager): Finished detecting IPC providers & initializing.");
        }


        /// <summary>
        ///      Disposes of the IPCManager and all integrations.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("IPCManager(Dispose): Disposing of all IPC providers...");

            foreach (var ipc in _ipcProviders.Values)
            {
                try
                {
                    PluginLog.Debug($"IPCManager(Dispose): Disposing of IPC provider {ipc.ID}...");
                    ipc.Dispose();
                    PluginLog.Debug($"IPCManager(Dispose): Disposed of IPC provider {ipc.ID}.");
                }
                catch (Exception e) { PluginLog.Error($"IPCManager(Dispose): Failed to dispose of IPC provider {ipc.ID} - {e.Message}"); }
            }

            PluginLog.Debug("IPCManager(Dispose): Successfully disposed.");
        }
    }
}