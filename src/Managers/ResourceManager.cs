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

        /// <summary>
        ///     Initializes the ResourceManager and associated resources.
        /// </summary>
        internal ResourceManager()
        {
            PluginLog.Debug("ResourceManager(ResourceManager): Initializing...");

            Setup(PluginService.PluginInterface.UiLanguage);
            PluginService.PluginInterface.LanguageChanged += Setup;
            ResourcesUpdated += OnResourceUpdate;

            PluginLog.Debug("ResourceManager(ResourceManager): Initialization complete.");
        }

        /// <summary>
        //      Disposes of the ResourceManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginService.PluginInterface.LanguageChanged -= Setup;
            ResourcesUpdated -= OnResourceUpdate;

            PluginLog.Debug("ResourceManager(Dispose): Successfully disposed.");
        }

        /// <summary> 
        ///     Downloads the repository from GitHub and extracts the resource data.
        /// </summary>
        internal void Update()
        {
            string repoName = PluginConstants.pluginName.Replace(" ", "");
            string zipFilePath = Path.Combine(Path.GetTempPath(), $"{repoName}.zip");
            string zipExtractPath = Path.Combine(Path.GetTempPath(), $"{repoName}-{PluginConstants.repoBranch}", $"{PluginConstants.repoResourcesDir}");
            string pluginExtractPath = Path.Combine(PluginConstants.pluginResourcesDir);

            // NOTE: This is only GitHub compatible, changes will need to be made here for other providers as necessary.
            new Thread(() =>
            {
                try
                {
                    PluginLog.Information($"ResourceManager(Update): Opening new thread to handle resource file download and extraction.");

                    // Download the files from the repository and extract them into the temp directory.
                    using HttpClient client = new();
                    client.GetAsync($"{PluginConstants.repoUrl}archive/refs/heads/{PluginConstants.repoBranch}.zip").ContinueWith((task) =>
                    {
                        using Stream stream = task.Result.Content.ReadAsStreamAsync().Result;
                        using FileStream fileStream = File.Create(zipFilePath);
                        stream.CopyTo(fileStream);
                    }).Wait();
                    PluginLog.Information($"ResourceManager(Update): Downloaded resource files to: {zipFilePath}");

                    // Extract the zip file and copy the resources.
                    ZipFile.ExtractToDirectory(zipFilePath, Path.GetTempPath(), true);
                    foreach (string dirPath in Directory.GetDirectories(zipExtractPath, "*", SearchOption.AllDirectories))
                    {
                        _ = Directory.CreateDirectory(dirPath.Replace(zipExtractPath, pluginExtractPath));
                        PluginLog.Debug($"ResourceManager(Update): Created directory: {dirPath.Replace(zipExtractPath, pluginExtractPath)}");
                    }

                    foreach (string newPath in Directory.GetFiles(zipExtractPath, "*.*", SearchOption.AllDirectories))
                    {
                        PluginLog.Debug($"ResourceManager(Update): Copying file from: {newPath} to: {newPath.Replace(zipExtractPath, pluginExtractPath)}");
                        File.Copy(newPath, newPath.Replace(zipExtractPath, pluginExtractPath), true);
                    }

                    // Cleanup temporary files.
                    File.Delete(zipFilePath);
                    Directory.Delete($"{Path.GetTempPath()}{repoName}-{PluginConstants.repoBranch}", true);
                    PluginLog.Information($"ResourceManager(Update): Deleted temporary files.");

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
            PluginLog.Debug($"ResourceManager(OnResourceUpdate): Resources updated.");
            Setup(PluginService.PluginInterface.UiLanguage);
        }

        /// <summary>
        ///     Sets up the plugin's resources.
        /// </summary>
        private void Setup(string language)
        {
            PluginLog.Information($"ResourceManager(Setup): Setting up resources for language {language}...");

            try { Loc.Setup(File.ReadAllText($"{PluginConstants.pluginlocalizationDir}{language}.json")); PluginService.DutyManager.ClearCache(); }
            catch { Loc.SetupWithFallbacks(); }

            PluginLog.Information("ResourceManager(Setup): Resources setup.");
        }
    }
}