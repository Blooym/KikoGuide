using System;
using System.Collections.Generic;
using CheapLoc;
using Dalamud.Plugin.Ipc;
using KikoGuide.Base;
using KikoGuide.IPC.Interfaces;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.Windows.Editor;
using KikoGuide.UI.Windows.GuideList;
using KikoGuide.UI.Windows.GuideViewer;

namespace KikoGuide.IPC.Providers
{
    /// <summary>
    ///     Provider for Wotsit
    /// </summary>
    internal sealed class WotsitIPC : IIPCProvider
    {
        public IPCProviders ID { get; } = IPCProviders.Wotsit;
        public bool ForcefullyDisabled { get; private set; }
        public bool Initialized { get; private set; }

        /// <summary>
        ///     The IconID that represents KikoGuide in Wotsit.
        /// </summary>
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
        ///     Invoke CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<string, bool>? wotsitInvoke;

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
            if (!this.Initialized)
            {
                return;
            }

            try
            {
                this.wotsitUnregister?.InvokeFunc(PluginConstants.PluginName);
                this.wotsitAvailable?.Unsubscribe(this.Initialize);
                this.wotsitInvoke?.Unsubscribe(this.HandleInvoke);
                PluginService.PluginInterface.LanguageChanged -= this.OnLanguageChange;

                this.wotsitRegister = null;
                this.wotsitUnregister = null;
                this.wotsitAvailable = null;
                this.wotsitInvoke = null;
                this.wotsitOpenListIpc = null;
                this.wotsitOpenEditorIpc = null;
                this.wotsitGuideIpcs.Clear();
                this.Initialized = false;
            }
            catch { /* Ignore */ }
        }

        /// <summary>
        ///     Initializes IPC for Wotsit.
        /// </summary>
        private void Initialize()
        {
            if (this.Initialized)
            {
                throw new InvalidOperationException("Wotsit IPC already initialized.");
            }

            this.wotsitRegister = PluginService.PluginInterface.GetIpcSubscriber<string, string, uint, string>(LabelProviderRegister);
            this.wotsitUnregister = PluginService.PluginInterface.GetIpcSubscriber<string, bool>(LabelProviderUnregisterAll);
            this.wotsitInvoke = PluginService.PluginInterface.GetIpcSubscriber<string, bool>(LabelProviderInvoke);

            this.wotsitInvoke?.Subscribe(this.HandleInvoke);
            this.RegisterAll();

            PluginService.PluginInterface.LanguageChanged += this.OnLanguageChange;

            this.Initialized = true;
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

            foreach (var guide in PluginService.GuideManager.GetAllGuides())
            {
                if (!guide.IsSupported() || guide.IsHidden())
                {
                    continue;
                }

                var guid = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCOpenGuideFor(guide.GetCanonicalName()), WotsitIconID);
                this.wotsitGuideIpcs.Add(guid, guide);
            }

            this.wotsitOpenListIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCOpenGuideList, WotsitIconID);
            this.wotsitOpenEditorIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCOpenGuideEditor, WotsitIconID);
        }

        /// <summary>
        ///     Handles IPC invocations for Wotsit.
        /// </summary>
        /// <param name="guid">The GUID of the invoked method.</param>
        private void HandleInvoke(string guid)
        {
            if (this.wotsitGuideIpcs.TryGetValue(guid, out var guide))
            {
                if (PluginService.WindowManager.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow guideViewerWindow)
                {
                    guideViewerWindow.Presenter.SetSelectedGuide(guide);
                    guideViewerWindow.IsOpen = true;
                }
            }
            else if (guid == this.wotsitOpenListIpc)
            {
                if (PluginService.WindowManager.GetWindow(TWindowNames.GuideList) is GuideListWindow guideListWIndow)
                {
                    guideListWIndow.IsOpen = true;
                }
            }
            else if (guid == this.wotsitOpenEditorIpc)
            {
                if (PluginService.WindowManager.GetWindow(TWindowNames.GuideEditor) is EditorWindow guideEditorWindow)
                {
                    guideEditorWindow.IsOpen = true;
                }
            }
        }

        /// <summary>
        ///     When the resources are updated, we need to re-register in-case of a language change.
        /// </summary>
        /// <param name="language"></param>
        private void OnLanguageChange(string language)
        {
            this.wotsitUnregister?.InvokeFunc(PluginConstants.PluginName);
            this.RegisterAll();
        }

        /// <summary>
        ///     Translations for Wotsit.
        /// </summary>
        private static class WotsitTranslations
        {
            internal static string WotsitIPCOpenGuideList => Loc.Localize("IPC.Wotsit.OpenGuideList", "Open Guide List");
            internal static string WotsitIPCOpenGuideEditor => Loc.Localize("IPC.Wotsit.OpenGuideEditor", "Open Guide Editor");
            internal static string WotsitIPCOpenGuideFor(string guideName) => string.Format(Loc.Localize("IPC.Wotsit.OpenGuideFor", "Open Guide for {0}"), guideName);
        }
    }
}
