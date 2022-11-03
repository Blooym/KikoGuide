namespace KikoGuide.Managers
{

    using System;
    using System.Threading;
    using System.IO;
    using System.Net.Http;
    using System.IO.Compression;
    using CheapLoc;
    using Dalamud.Logging;
    using KikoGuide.Base;

    /// <summary> 
    ///     Sets up and manages the plugin's resources and localization.
    /// </summary>
    sealed internal class ResourceManager : IDisposable
    {
        private readonly string _cachePath = Path.Combine(Path.GetTempPath(), PStrings.pluginName);
        private readonly DateTime _cacheExpiration = DateTime.Now.AddDays(1);
        private bool _initialized = false;
        internal bool? lastUpdateSuccess;
        internal bool updateInProgress;
        internal event ResourceUpdateDelegate? ResourcesUpdated;
        internal delegate void ResourceUpdateDelegate();

        /// <summary> 
        ///     Initializes the ResourceManager and associated resources. 
        /// </summary>
        internal ResourceManager()
        {
            PluginLog.Debug("ResourceManager(ResourceManager): Initializing...");

            this.Setup(PluginService.PluginInterface.UiLanguage);
            PluginService.PluginInterface.LanguageChanged += this.Setup;
            this.ResourcesUpdated += this.OnResourceUpdate;

            PluginLog.Debug("ResourceManager(ResourceManager): Initialization complete.");
        }

        /// <summary> 
        ///     Disposes of the ResourceManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("ResourceManager(Dispose): Disposing...");

            PluginService.PluginInterface.LanguageChanged -= Setup;
            ResourcesUpdated -= OnResourceUpdate;

            PluginLog.Debug("ResourceManager(Dispose): Successfully disposed.");
        }

        /// <summary> 
        ///     Downloads the repository from GitHub and extracts the resource data into the plugin's directory.
        /// </summary>
        internal void UpdateResources()
        {
            var repoName = PStrings.pluginName.Replace(" ", "");
            var zipFilePath = Path.Combine(Path.GetTempPath(), $"{repoName}.zip");
            var zipExtractPath = Path.Combine(Path.GetTempPath(), $"{repoName}-{PStrings.repoBranch}", $"{PStrings.repoResourcesDir}");
            var pluginExtractPath = Path.Combine(PStrings.pluginResourcesDir);

            new Thread(() =>
            {
                try
                {
                    PluginLog.Debug($"ResourceManager(UpdateResources): Opening new thread to handle resource download.");
                    this.updateInProgress = true;

                    // Download the files from the repository and extract them into the temp directory.
                    using var client = new HttpClient();
                    client.GetAsync($"{PStrings.repoUrl}archive/refs/heads/{PStrings.repoBranch}.zip").ContinueWith((task) =>
                    {
                        using var stream = task.Result.Content.ReadAsStreamAsync().Result;
                        using var fileStream = File.Create(zipFilePath);
                        stream.CopyTo(fileStream);
                    }).Wait();

                    // Extract the zip file and copy the resources.
                    ZipFile.ExtractToDirectory(zipFilePath, Path.GetTempPath(), true);
                    foreach (string dirPath in Directory.GetDirectories(zipExtractPath, "*", SearchOption.AllDirectories)) Directory.CreateDirectory(dirPath.Replace(zipExtractPath, pluginExtractPath));
                    foreach (string newPath in Directory.GetFiles(zipExtractPath, "*.*", SearchOption.AllDirectories)) File.Copy(newPath, newPath.Replace(zipExtractPath, pluginExtractPath), true);

                    // Cleanup temporary files.
                    File.Delete(zipFilePath);
                    Directory.Delete($"{Path.GetTempPath()}{repoName}-{PStrings.repoBranch}", true);

                    // Broadcast an event indicating that the resources have been updated.
                    ResourcesUpdated?.Invoke();
                }
                catch (Exception e)
                {
                    PluginLog.Error($"ResourceManager(UpdateResources): Error updating resource files: {e.Message}");

                    this.lastUpdateSuccess = false;
                    this.updateInProgress = false;
                }
            }).Start();
        }

        /// <summary>
        ///     Handles the OnResourceUpdate event.
        /// </summary>
        private void OnResourceUpdate()
        {
            PluginLog.Debug($"ResourceManager(OnResourceUpdate): Resources updated.");

            PluginService.Configuration.Save();
            this.lastUpdateSuccess = true;
            this.updateInProgress = false;

            Setup(PluginService.PluginInterface.UiLanguage);
        }

        /// <summary>
        ///     Sets up the plugin's resources.
        /// </summary>
        private void Setup(string language)
        {
            PluginLog.Debug($"ResourceManager(Setup): Setting up resources for language {language}...");

            if (_initialized) DutyManager.ClearCache();

            try { Loc.Setup(File.ReadAllText($"{PStrings.pluginlocalizationDir}\\Plugin\\{language}.json")); }
            catch { Loc.SetupWithFallbacks(); }

            _initialized = true;
            PluginLog.Debug("ResourceManager(Setup): Resources setup.");
        }
    }
}