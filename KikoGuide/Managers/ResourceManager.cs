using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using CheapLoc;
using Dalamud.Logging;
using KikoGuide.Base;

namespace KikoGuide.Managers
{
    /// <summary>
    ///     Sets up and manages the plugin's resources and localization.
    /// </summary>
    internal sealed class ResourceManager : IDisposable
    {
        internal event ResourceUpdateDelegate? ResourcesUpdated;
        internal delegate void ResourceUpdateDelegate();
        private bool initialized;

        /// <summary>
        ///     Initializes the ResourceManager and associated resources.
        /// </summary>
        internal ResourceManager()
        {
            PluginLog.Debug("ResourceManager(ResourceManager): Initializing...");

            this.Setup(PluginService.PluginInterface.UiLanguage);
            PluginService.PluginInterface.LanguageChanged += this.Setup;
            ResourcesUpdated += this.OnResourceUpdate;

            PluginLog.Debug("ResourceManager(ResourceManager): Initialization complete.");
        }

        /// <summary>
        ///      Disposes of the ResourceManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginService.PluginInterface.LanguageChanged -= this.Setup;
            ResourcesUpdated -= this.OnResourceUpdate;

            PluginLog.Debug("ResourceManager(Dispose): Successfully disposed.");
        }

        /// <summary>
        ///     Downloads the repository from GitHub and extracts the resource data.
        /// </summary>
        internal void Update()
        {
            var repoName = PluginConstants.PluginName.Replace(" ", "");
            var zipFilePath = Path.Combine(Path.GetTempPath(), $"{repoName}.zip");
            var zipExtractPath = Path.Combine(Path.GetTempPath(), $"{repoName}-{PluginConstants.RepoBranch}", PluginConstants.RepoResourcesDir);
            var pluginExtractPath = Path.Combine(PluginConstants.PluginResourcesDir);

            // NOTE: This is only GitHub compatible, changes will need to be made here for other providers as necessary.
            new Thread(() =>
            {
                try
                {
                    PluginLog.Information("ResourceManager(Update): Opening new thread to handle resource file download and extraction.");

                    // Download the files from the repository and extract them into the temp directory.
                    using HttpClient client = new();
                    client.GetAsync($"{PluginConstants.RepoUrl}archive/refs/heads/{PluginConstants.RepoBranch}.zip").ContinueWith((task) =>
                    {
                        using var stream = task.Result.Content.ReadAsStreamAsync().Result;
                        using var fileStream = File.Create(zipFilePath);
                        stream.CopyTo(fileStream);
                    }).Wait();
                    PluginLog.Information("ResourceManager(Update): Downloaded resource files to: {zipFilePath}");

                    // Extract the zip file and copy the resources.
                    ZipFile.ExtractToDirectory(zipFilePath, Path.GetTempPath(), true);
                    foreach (var dirPath in Directory.GetDirectories(zipExtractPath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(zipExtractPath, pluginExtractPath));
                        PluginLog.Debug($"ResourceManager(Update): Created directory: {dirPath.Replace(zipExtractPath, pluginExtractPath)}");
                    }

                    foreach (var newPath in Directory.GetFiles(zipExtractPath, "*.*", SearchOption.AllDirectories))
                    {
                        PluginLog.Debug($"ResourceManager(Update): Copying file from: {newPath} to: {newPath.Replace(zipExtractPath, pluginExtractPath)}");
                        File.Copy(newPath, newPath.Replace(zipExtractPath, pluginExtractPath), true);
                    }

                    // Cleanup temporary files.
                    File.Delete(zipFilePath);
                    Directory.Delete($"{Path.GetTempPath()}{repoName}-{PluginConstants.RepoBranch}", true);
                    PluginLog.Information("ResourceManager(Update): Deleted temporary files.");

                    // Broadcast an event indicating that the resources have been updated.
                    ResourcesUpdated?.Invoke();
                }
                catch (Exception e) { PluginLog.Error($"ResourceManager(Update): Error updating resource files: {e.Message}"); }
            }).Start();
        }

        /// <summary>
        ///     Handles the OnResourceUpdate event.
        /// </summary>
        private void OnResourceUpdate()
        {
            PluginLog.Debug("ResourceManager(OnResourceUpdate): Resources updated.");
            this.Setup(PluginService.PluginInterface.UiLanguage);
        }

        /// <summary>
        ///     Sets up the plugin's resources.
        /// </summary>
        /// <param name="language">The two-letter language code to use.</param>)
        /// </summary>
        private void Setup(string language)
        {
            PluginLog.Information($"ResourceManager(Setup): Setting up resources for language {language}...");

            if (this.initialized)
            {
                GuideManager.ClearCache();
            }

            try
            { Loc.Setup(File.ReadAllText($"{PluginConstants.PluginlocalizationDir}{language}.json")); }
            catch { Loc.SetupWithFallbacks(); }

            this.initialized = true;
            PluginLog.Information("ResourceManager(Setup): Resources setup.");
        }
    }
}
