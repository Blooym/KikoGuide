namespace KikoGuide.Managers.IPC;

using System.Collections.Generic;
using CheapLoc;
using Dalamud.Logging;
using Dalamud.Plugin.Ipc;
using KikoGuide.Base;
using KikoGuide.Types;
using KikoGuide.Managers;
using KikoGuide.Interfaces;

/// <summary> 
///     Provider for WotsitIPC
/// </summary>
sealed public class WotsitIPCProvider : IIPCProvider
{
    public IPCProviders ID { get; } = IPCProviders.Wotsit;

    private ICallGateSubscriber<string, string, uint, string>? _wotsitRegister;
    private ICallGateSubscriber<string, bool>? _wotsitUnregister;
    private ICallGateSubscriber<bool>? _wotsitAvailable;
    private const uint WotsitIconID = 21;

    private string? _wotsitOpenListIpc;
    private string? _wotsitOpenEditorIpc;
    private Dictionary<string, Duty> _wotsitDutyIpcs = new Dictionary<string, Duty>();


    /// <summary> 
    /// Initializes the WotsitIPC if it's available and enabled.
    /// </summary>
    public WotsitIPCProvider()
    {
        // You must handle if this integration loads here using enabledIntegrations and any other custom logic.
        if (!PluginService.Configuration.enabledIntegrations.Contains(ID)) { PluginLog.Debug($"WotsitIPCProvider(WotsitIPCProvider): Not enabled, skipping."); return; }

        // Attempt to initialize the plugin, if it fails then do nothing & subscribe to FA.Available.
        try { Initialize(); }
        catch { /* Do nothing */ }

        _wotsitAvailable = PluginService.PluginInterface.GetIpcSubscriber<bool>("FA.Available");
        _wotsitAvailable.Subscribe(Initialize);
    }


    /// <summary>
    ///     Disposes of the IPC for Wotsit.
    /// </summary>
    public void Dispose()
    {
        // Wrap dispose logic inside of a try catch so if it fails it doesn't crash the plugin.
        try
        {
            _wotsitUnregister?.InvokeFunc(PStrings.pluginName);
            _wotsitAvailable?.Unsubscribe(Initialize);
        }
        catch { /* Do nothing */ }
    }


    /// <summary>
    ///     Initializes IPC for Wotsit.
    /// </summary>
    private void Initialize()
    {
        _wotsitRegister = PluginService.PluginInterface.GetIpcSubscriber<string, string, uint, string>("FA.Register");
        _wotsitUnregister = PluginService.PluginInterface.GetIpcSubscriber<string, bool>("FA.UnregisterAll");

        var subscribe = PluginService.PluginInterface.GetIpcSubscriber<string, bool>("FA.Invoke");
        subscribe.Subscribe(HandleInvoke);

        this.RegisterAll();
    }


    /// <summary>
    ///     Registers / Reloads the listings for this plugin.
    /// </summary>
    private void RegisterAll()
    {
        if (_wotsitRegister == null) return;

        foreach (var duty in DutyManager.GetDuties())
        {
            // if (!DutyManager.IsUnlocked(duty) || !duty.HasData()) continue;
            var guid = _wotsitRegister.InvokeFunc(PStrings.pluginName, $"{duty.GetCanonicalName()}", WotsitIconID);
            _wotsitDutyIpcs.Add(guid, duty);
        }

        _wotsitOpenListIpc = _wotsitRegister.InvokeFunc(PStrings.pluginName, Loc.Localize("WotsitIPC.OpenDutyFinder", "Open Duty Finder"), WotsitIconID);
        _wotsitOpenEditorIpc = _wotsitRegister.InvokeFunc(PStrings.pluginName, Loc.Localize("WotsitIPC.OpenDutyEditor", "Open Duty Editor"), WotsitIconID);
    }


    /// <summary> 
    ///     Handles IPC invocations for Wotsit.
    /// </summary>
    private void HandleInvoke(string guid)
    {
        if (_wotsitDutyIpcs.TryGetValue(guid, out var duty))
        {
            PluginService.WindowManager.DutyInfo.presenter.selectedDuty = duty;
            PluginService.WindowManager.DutyInfo.Show();
        }

        else if (guid == _wotsitOpenListIpc) PluginService.WindowManager.DutyList.Show();
        else if (guid == _wotsitOpenEditorIpc) PluginService.WindowManager.Editor.Show();
    }
}