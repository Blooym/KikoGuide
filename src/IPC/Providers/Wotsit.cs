using System.Collections.Generic;
using CheapLoc;
using Dalamud.Plugin.Ipc;
using KikoGuide.Base;
using KikoGuide.IPC.Interfaces;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.Windows.DutyInfo;
using KikoGuide.UI.Windows.DutyList;
using KikoGuide.UI.Windows.Editor;

namespace KikoGuide.IPC.Providers
{
    /// <summary> 
    ///     Provider for Wotsit
    /// </summary>
    public sealed class WotsitIPC : IIPCProvider
    {
        public IPCProviders ID { get; } = IPCProviders.Wotsit;

        private const uint WotsitIconID = 21;

        /// <summary>
        ///     Available label.
        /// </summary>
        private const string LabelProviderAvailable = "FA.Available";

        /// <summary>
        ///     Register label.
        /// </summary>
        private const string LabelProviderRegister = "FA.Register";

        /// <summary>
        ///     UnregisterAll label.
        /// </summary>
        private const string LabelProviderUnregisterAll = "FA.UnregisterAll";

        /// <summary>
        ///     Invoke label.
        /// </summary>
        private const string LabelProviderInvoke = "FA.Invoke";

        /// <summary>
        ///     Register CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<string, string, uint, string>? wotsitRegister;

        /// <summary>
        ///     Unregister CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<string, bool>? wotsitUnregister;

        /// <summary>
        ///     Available CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<bool>? wotsitAvailable;

        /// <summary>
        ///     Stored GUID for OpenListIPC.
        /// </summary>
        private string? wotsitOpenListIpc;

        /// <summary>
        ///     Stored GUID for OpenEditorIPC.
        /// </summary>
        private string? wotsitOpenEditorIpc;

        /// <summary>
        ///     Stored GUIDs for Duty's.
        /// </summary>
        private readonly Dictionary<string, Duty> wotsitDutyIpcs = new();

        public void Enable()
        {
            try
            { this.Initialize(); }
            catch { /* Ignore */ }

            this.wotsitAvailable = PluginService.PluginInterface.GetIpcSubscriber<bool>(LabelProviderAvailable);
            this.wotsitAvailable.Subscribe(this.Initialize);
        }

        public void Dispose()
        {
            try
            {
                this.wotsitAvailable?.Unsubscribe(this.Initialize);
                this.wotsitUnregister?.InvokeFunc(PluginConstants.PluginName);
            }
            catch { /* Ignore */ }
        }

        /// <summary>
        ///     Initializes IPC for Wotsit.
        /// </summary>
        private void Initialize()
        {
            this.wotsitRegister = PluginService.PluginInterface.GetIpcSubscriber<string, string, uint, string>(LabelProviderRegister);
            this.wotsitUnregister = PluginService.PluginInterface.GetIpcSubscriber<string, bool>(LabelProviderUnregisterAll);

            var subscribe = PluginService.PluginInterface.GetIpcSubscriber<string, bool>(LabelProviderInvoke);
            subscribe.Subscribe(this.HandleInvoke);

            this.RegisterAll();
        }

        /// <summary>
        ///     Registers / Reloads the listings for this plugin.
        /// </summary>
        private void RegisterAll()
        {
            if (this.wotsitRegister == null)
            {
                return;
            }

            foreach (var duty in PluginService.DutyManager.GetDuties())
            {
                var guid = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, $"{duty.GetCanonicalName()}", WotsitIconID);
                this.wotsitDutyIpcs.Add(guid, duty);
            }

            this.wotsitOpenListIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, Loc.Localize("WotsitIPC.OpenDutyFinder", "Open Duty Finder"), WotsitIconID);
            this.wotsitOpenEditorIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, Loc.Localize("WotsitIPC.OpenDutyEditor", "Open Duty Editor"), WotsitIconID);
        }

        /// <summary> 
        ///     Handles IPC invocations for Wotsit.
        /// </summary>
        private void HandleInvoke(string guid)
        {
            if (this.wotsitDutyIpcs.TryGetValue(guid, out var duty))
            {
                if (PluginService.WindowManager.WindowSystem.GetWindow(WindowManager.DutyInfoWindowName) is DutyInfoWindow dutyInfoWindow)
                {
                    dutyInfoWindow.Presenter.SelectedDuty = duty;
                    dutyInfoWindow.IsOpen = true;
                }
            }

            else if (guid == this.wotsitOpenListIpc)
            {
                if (PluginService.WindowManager.WindowSystem.GetWindow(WindowManager.DutyListWindowName) is DutyListWindow dutyListWindow)
                {
                    dutyListWindow.IsOpen = true;
                }
            }

            else if (guid == this.wotsitOpenEditorIpc)
            {
                if (PluginService.WindowManager.WindowSystem.GetWindow(WindowManager.EditorWindowName) is EditorWindow dutyEditorWindow)
                {
                    dutyEditorWindow.IsOpen = true;
                }
            }
        }
    }
}