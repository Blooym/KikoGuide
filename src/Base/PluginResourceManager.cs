namespace KikoGuide.Base;

using System;
using System.Threading;
using System.IO;
using System.Net;
using System.IO.Compression;
using CheapLoc;
using Dalamud.Logging;
using KikoGuide.Managers;

/// <summary> Sets up and manages the plugin's resources and localization. </summary>
public static class PluginResourceManager
{
    public static bool? lastUpdateSuccess;
    public static bool updateInProgress;
    public static event ResourceUpdateDelegate? ResourcesUpdated;
    public delegate void ResourceUpdateDelegate();


    /// <summary> Initializes the PluginResourceManager and associated resources. </summary>
    public static void Initialize()
    {
        PluginLog.Debug("PluginResourceManager: Initializing...");

        Setup(PluginService.PluginInterface.UiLanguage);
        PluginService.PluginInterface.LanguageChanged += Setup;
        ResourcesUpdated += OnResourceUpdate;

        PluginLog.Debug("PluginResourceManager: Initialization complete.");
    }


    /// <summary> Disposes of the PluginResourceManager and associated resources. </summary>
    public static void Dispose()
    {
        PluginLog.Debug("PluginResourceManager: Disposing...");

        PluginService.PluginInterface.LanguageChanged -= Setup;
        ResourcesUpdated -= OnResourceUpdate;

        PluginLog.Debug("PluginResourceManager: Successfully disposed.");
    }


    /// <summary> Downloads the repository from GitHub and extracts the resource data. </summary>
    public static void Update()
    {
        new Thread(() =>
        {
            try
            {
                PluginLog.Debug($"PluginResourceManager: Opening new thread to handle resource download.");
                updateInProgress = true;

                // Create a new WebClient to download the data and some paths for installation.
                var webClient = new WebClient();
                var zipFile = Path.Combine(Path.GetTempPath(), "KikoGuide_Source.zip");
                var sourcePath = Path.Combine(Path.GetTempPath(), "KikoGuide-main", "src", "Resources");
                var targetPath = Path.Combine(PluginStrings.resourcePath);

                // Download the file into the system temp directory to make sure it can be cleaned up by the OS incase of a crash.
                webClient.DownloadFile($"{PluginStrings.pluginRepository}archive/refs/heads/main.zip", zipFile);

                // Extract the zip file into the system temp directory and delete the zip file.
                ZipFile.ExtractToDirectory(zipFile, Path.GetTempPath(), true);

                // Create directories & copy files.
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)) Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)) File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);

                // Delete the temporary files.
                File.Delete(zipFile);
                Directory.Delete($"{Path.GetTempPath()}KikoGuide-main", true);

                // Set update statuses to their values.
                lastUpdateSuccess = true;
                updateInProgress = false;

                // Set the last manual update time to now.
                PluginService.Configuration.lastResourceUpdate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                PluginService.Configuration.Save();

                // Broadcast an event indicating that the resources have been updated & refresh the UI.
                ResourcesUpdated?.Invoke();
            }

            catch (Exception e)
            {
                // Set update statuses to their values & log the error.
                lastUpdateSuccess = false;
                updateInProgress = false;
                PluginLog.Error($"UpdateManager: Error updating resource files: {e.Message}");
            }

        }).Start();
    }


    /// <summary> Handles the OnResourceUpdate event. </summary>
    private static void OnResourceUpdate()
    {
        PluginLog.Debug($"PluginResourceManager: Resources updated.");

        Setup(PluginService.PluginInterface.UiLanguage);
    }


    /// <summary> Sets up the plugin's resources. </summary>
    private static void Setup(string language)
    {
        PluginLog.Debug($"PluginResourceManager: Setting up resources for language {language}...");

        DutyManager.ClearCache();
        try { Loc.Setup(File.ReadAllText($"{PluginStrings.localizationPath}\\Plugin\\{language}.json")); }
        catch { Loc.SetupWithFallbacks(); }

        PluginLog.Debug("PluginResourceManager: Resources setup.");
    }
}