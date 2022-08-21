namespace KikoGuide.Managers;

using System;
using System.Threading;
using System.IO;
using System.Net;
using System.IO.Compression;
using CheapLoc;
using Dalamud.Logging;
using KikoGuide.Base;

/// <summary> 
///     Sets up and manages the plugin's resources and localization.
/// </summary>
sealed public class ResourceManager : IDisposable
{
    private bool initialized = false;
    public bool? lastUpdateSuccess;
    public bool updateInProgress;

    public event ResourceUpdateDelegate? ResourcesUpdated;
    public delegate void ResourceUpdateDelegate();


    /// <summary> 
    ///     Initializes the ResourceManager and associated resources. 
    /// </summary>
    public ResourceManager()
    {
        PluginLog.Debug("ResourceManager: Initializing...");

        Setup(PluginService.PluginInterface.UiLanguage);
        PluginService.PluginInterface.LanguageChanged += Setup;
        ResourcesUpdated += OnResourceUpdate;

        PluginLog.Debug("ResourceManager: Initialization complete.");
    }


    /// <summary> 
    ///     Disposes of the ResourceManager and associated resources.
    /// </summary>
    public void Dispose()
    {
        PluginLog.Debug("ResourceManager: Disposing...");

        PluginService.PluginInterface.LanguageChanged -= Setup;
        ResourcesUpdated -= OnResourceUpdate;

        PluginLog.Debug("ResourceManager: Successfully disposed.");
    }


    /// <summary> 
    ///     Downloads the repository from GitHub and extracts the resource data into the plugin's directory.
    /// </summary>
    public void Update()
    {
        var repoName = PStrings.pluginName.Replace(" ", "");
        var zipFilePath = Path.Combine(Path.GetTempPath(), $"{repoName}.zip");
        var zipExtractPat = Path.Combine(Path.GetTempPath(), $"{repoName}-{PStrings.repoBranch}", $"{PStrings.repoResourcesDir}");
        var pluginExtractPath = Path.Combine(PStrings.pluginResourcesDir);

        new Thread(() =>
        {
            try
            {
                PluginLog.Debug($"ResourceManager: Opening new thread to handle resource download.");
                updateInProgress = true;

                // Download the files from the repository and extract them into the temp directory.
                using var webClient = new WebClient();
                webClient.DownloadFile($"{PStrings.repoUrl}archive/refs/heads/{PStrings.repoBranch}.zip", zipFilePath);
                ZipFile.ExtractToDirectory(zipFilePath, Path.GetTempPath(), true);

                // Move the relevant files into the plugin's resources directory.
                foreach (string dirPath in Directory.GetDirectories(zipExtractPat, "*", SearchOption.AllDirectories)) Directory.CreateDirectory(dirPath.Replace(zipExtractPat, pluginExtractPath));
                foreach (string newPath in Directory.GetFiles(zipExtractPat, "*.*", SearchOption.AllDirectories)) File.Copy(newPath, newPath.Replace(zipExtractPat, pluginExtractPath), true);

                // Delete the temporary files.
                File.Delete(zipFilePath);
                Directory.Delete($"{Path.GetTempPath()}{repoName}-{PStrings.repoBranch}", true);

                // Set update statuses to their values.
                lastUpdateSuccess = true;
                updateInProgress = false;

                // Set the last manual update time to now.
                PluginService.Configuration.lastResourceUpdate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                PluginService.Configuration.Save();

                // Broadcast an event indicating that the resources have been updated.
                ResourcesUpdated?.Invoke();
            }

            catch (Exception e)
            {
                // Set update statuses to their values & log the error.
                lastUpdateSuccess = false;
                updateInProgress = false;
                PluginLog.Error($"ResourceManager: Error updating resource files: {e.Message}");
            }
        }).Start();
    }


    /// <summary>
    ///     Handles the OnResourceUpdate event.
    /// </summary>
    private void OnResourceUpdate()
    {
        PluginLog.Debug($"ResourceManager: Resources updated.");

        Setup(PluginService.PluginInterface.UiLanguage);
    }


    /// <summary>
    ///     Sets up the plugin's resources.
    /// </summary>
    private void Setup(string language)
    {
        PluginLog.Debug($"ResourceManager: Setting up resources for language {language}...");

        if (initialized) DutyManager.ClearCache();

        try { Loc.Setup(File.ReadAllText($"{PStrings.localizationPath}\\Plugin\\{language}.json")); }
        catch { Loc.SetupWithFallbacks(); }

        initialized = true;
        PluginLog.Debug("ResourceManager: Resources setup.");
    }
}