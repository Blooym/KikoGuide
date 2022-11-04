namespace KikoGuide.IPC.Providers
{
    using System.Collections.Generic;
    using CheapLoc;
    using Dalamud.Plugin.Ipc;
    using KikoGuide.Base;
    using KikoGuide.IPC;
    using KikoGuide.Managers;
    using KikoGuide.Types;
    using KikoGuide.UI.Windows.DutyInfo;
    using KikoGuide.UI.Windows.DutyList;
    using KikoGuide.UI.Windows.Editor;

    /// <summary> 
    ///     Provider for WotsitIPC
    /// </summary>
    public sealed class WotsitIPC : IIPCProvider
    {
        public IPCProviders ID { get; } = IPCProviders.Wotsit;

        private ICallGateSubscriber<string, string, uint, string>? _wotsitRegister;
        private ICallGateSubscriber<string, bool>? _wotsitUnregister;
        private ICallGateSubscriber<bool>? _wotsitAvailable;
        private const uint WotsitIconID = 21;

        private string? _wotsitOpenListIpc;
        private string? _wotsitOpenEditorIpc;
        private Dictionary<string, Duty> _wotsitDutyIpcs = new Dictionary<string, Duty>();

        public void Enable()
        {
            try { Initialize(); }
            catch { /* Ignore */ }

            _wotsitAvailable = PluginService.PluginInterface.GetIpcSubscriber<bool>("FA.Available");
            _wotsitAvailable.Subscribe(Initialize);
        }

        public void Dispose()
        {
            try
            {
                _wotsitAvailable?.Unsubscribe(Initialize);
                _wotsitUnregister?.InvokeFunc(PluginConstants.pluginName);
            }
            catch { /* Ignore */ }
        }

        /// <summary>
        ///     Initializes IPC for Wotsit.
        /// </summary>
        private void Initialize()
        {
            this._wotsitRegister = PluginService.PluginInterface.GetIpcSubscriber<string, string, uint, string>("FA.Register");
            this._wotsitUnregister = PluginService.PluginInterface.GetIpcSubscriber<string, bool>("FA.UnregisterAll");

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

            foreach (var duty in PluginService.DutyManager.GetDuties())
            {
                var guid = _wotsitRegister.InvokeFunc(PluginConstants.pluginName, $"{duty.GetCanonicalName()}", WotsitIconID);
                this._wotsitDutyIpcs.Add(guid, duty);
            }

            this._wotsitOpenListIpc = _wotsitRegister.InvokeFunc(PluginConstants.pluginName, Loc.Localize("WotsitIPC.OpenDutyFinder", "Open Duty Finder"), WotsitIconID);
            this._wotsitOpenEditorIpc = _wotsitRegister.InvokeFunc(PluginConstants.pluginName, Loc.Localize("WotsitIPC.OpenDutyEditor", "Open Duty Editor"), WotsitIconID);
        }

        /// <summary> 
        ///     Handles IPC invocations for Wotsit.
        /// </summary>
        private void HandleInvoke(string guid)
        {
            if (_wotsitDutyIpcs.TryGetValue(guid, out var duty))
            {
                if (PluginService.WindowManager.windowSystem.GetWindow(WindowManager.DutyInfoWindowName) is DutyInfoWindow dutyInfoWindow)
                {
                    dutyInfoWindow._presenter.selectedDuty = duty;
                    dutyInfoWindow.IsOpen = true;
                }
            }

            else if (guid == _wotsitOpenListIpc)
            {
                if (PluginService.WindowManager.windowSystem.GetWindow(WindowManager.DutyListWindowName) is DutyListWindow dutyListWindow) dutyListWindow.IsOpen = true;
            }

            else if (guid == _wotsitOpenEditorIpc)
            {
                if (PluginService.WindowManager.windowSystem.GetWindow(WindowManager.EditorWindowName) is EditorWindow dutyEditorWindow) dutyEditorWindow.IsOpen = true;
            }
        }
    }
}