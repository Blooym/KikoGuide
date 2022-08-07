namespace KikoGuide.Managers;

using System;
using Dalamud.Logging;
using KikoGuide.Managers.IPC;

/// <summary> Controls all IPC for the plugin and is responsible for registering and disposing IPC. </summary>
sealed public class IPCManager : IDisposable
{
    private WotsitIPC _wotsitIPC;

    /// <summary> Initializes the IPCManager. </summary>
    public IPCManager()
    {
        PluginLog.Debug("IPCManager: Initializing...");

        _wotsitIPC = new WotsitIPC();

        PluginLog.Debug("IPCManager: Successfully initialized.");
    }

    /// <summary> Disposes of the IPCManager and all integrations. </summary>
    public void Dispose()
    {
        PluginLog.Debug("IPCManager: Disposing...");

        _wotsitIPC.Dispose();

        PluginLog.Debug("IPCManager: Successfully disposed.");
    }
}