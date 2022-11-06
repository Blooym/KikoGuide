using System.Collections.Generic;
using CheapLoc;
using Dalamud.Plugin.Ipc;
using KikoGuide.Base;
using KikoGuide.IPC.Interfaces;
using KikoGuide.Localization;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.Windows.Editor;
using KikoGuide.UI.Windows.GuideList;
using KikoGuide.UI.Windows.GuideViewer;

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
        ///     Stored GUIDs for Guides's.
        /// </summary>
        private readonly Dictionary<string, Guide> wotsitGuideIpcs = new();

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

            foreach (var guide in GuideManager.GetGuides())
            {
                var guid = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCOpenGuideFor(guide.GetCanonicalName()), WotsitIconID);
                this.wotsitGuideIpcs.Add(guid, guide);
            }

            this.wotsitOpenListIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCOpenGuideList, WotsitIconID);
            this.wotsitOpenEditorIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCOpenGuideEditor, WotsitIconID);
        }

        /// <summary>
        ///     Handles IPC invocations for Wotsit.
        /// </summary>
        private void HandleInvoke(string guid)
        {
            if (this.wotsitGuideIpcs.TryGetValue(guid, out var guide))
            {
                if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow guideViewerWindow)
                {
                    guideViewerWindow.Presenter.SelectedGuide = guide;
                    guideViewerWindow.IsOpen = true;
                }
            }

            else if (guid == this.wotsitOpenListIpc)
            {
                if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideList) is GuideListWindow guideListWIndow)
                {
                    guideListWIndow.IsOpen = true;
                }
            }

            else if (guid == this.wotsitOpenEditorIpc)
            {
                if (PluginService.WindowManager.WindowSystem.GetWindow(TWindowNames.GuideEditor) is EditorWindow guideEditorWindow)
                {
                    guideEditorWindow.IsOpen = true;
                }
            }
        }

        /// <summary>
        ///     Translations for Wotsit.
        /// </summary>
        private static class WotsitTranslations
        {
            public static string WotsitIPCOpenGuideList => Loc.Localize("IPC.Wotsit.OpenGuideList", "Open Guide List");
            public static string WotsitIPCOpenGuideEditor => Loc.Localize("IPC.Wotsit.OpenGuideEditor", "Open Guide Editor");
            public static string WotsitIPCOpenGuideFor(string guideName) => string.Format(Loc.Localize("IPC.Wotsit.OpenGuideFor", "Open Guide for {0}"), guideName);
        }
    }
}
