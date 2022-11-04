namespace KikoGuide.UI.Windows.Settings
{
    using System;
    using System.IO;
    using CheapLoc;
    using Dalamud.Interface.ImGuiFileDialog;
    using Dalamud.Interface.Internal.Notifications;
    using KikoGuide.Base;
    using KikoGuide.IPC;

    sealed public class SettingsPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal Configuration GetConfiguration() => PluginService.Configuration;

        /// <summary>
        ///     Sets an IPCProvider as enabled.
        /// </summary>
        public void SetIPCProviderEnabled(IPCProviders provider) => PluginService.IPC.EnableProvider(provider);

        /// <summary>
        ///     Sets an IPCProvider as disabled.
        /// </summary>
        public void SetIPCProviderDisabled(IPCProviders provider) => PluginService.IPC.DisableProvider(provider);

#if DEBUG
        public FileDialogManager dialogManager = new FileDialogManager();

        /// <summary>
        ///     Handles the directory select event and saves the location to that directory.
        /// </summary>
        public void OnDirectoryPicked(bool success, string path)
        {
            if (!success) return;
            var directory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(path);
            Loc.ExportLocalizable();
            File.Copy(Path.Combine(path, "KikoGuide_Localizable.json"), Path.Combine(path, "en.json"), true);
            Directory.SetCurrentDirectory(directory);
            PluginService.PluginInterface.UiBuilder.AddNotification("Localization exported successfully.", PluginConstants.pluginName, NotificationType.Success);
        }
#endif
    }
}