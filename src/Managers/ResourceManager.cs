namespace KikoGuide.Managers;

using System;
using System.Threading;
using System.IO;
using System.Net.Http;
using System.IO.Compression;
using CheapLoc;
using Dalamud.Logging;
using KikoGuide.Base;

/// <summary> Sets up and manages the plugin's resources and localization. </summary>
sealed public class ResourceManager : IDisposable
{
    private bool initialized = false;
    public bool? lastUpdateSuccess;
    public bool updateInProgress;

    public event ResourceUpdateDelegate? ResourcesUpdated;
    public delegate void ResourceUpdateDelegate();


    /// <summary> Initializes the ResourceManager and associated resources. </summary>
    public ResourceManager()
    {
        PluginLog.Debug("ResourceManager: Initializing...");

        Setup(PluginService.PluginInterface.UiLanguage);
        PluginService.PluginInterface.LanguageChanged += Setup;
        ResourcesUpdated += OnResourceUpdate;

        PluginLog.Debug("ResourceManager: Initialization complete.");
    }


    /// <summary> Disposes of the ResourceManager and associated resources. </summary>
    public void Dispose()
    {
        PluginLog.Debug("ResourceManager: Disposing...");

        PluginService.PluginInterface.LanguageChanged -= Setup;
        ResourcesUpdated -= OnResourceUpdate;

        PluginLog.Debug("ResourceManager: Successfully disposed.");
    }



    /// <summary> Downloads the repository from GitHub and extracts the resource data. </summary>
    public void Update()
    {
        var zipFile = Path.Combine(Path.GetTempPath(), "KikoGuide_Source.zip");
        var sourcePath = Path.Combine(Path.GetTempPath(), "kikoGuide-main", "src", "Resources");
        var targetPath = Path.Combine(PStrings.resourcePath);

        new Thread(() =>
        {
            try
            {
                PluginLog.Debug($"ResourceManager: Opening new thread to handle resource download.");

                // Download the file from GitHub and extract it to the temporary location.
                using var client = new HttpClient();
                client.GetAsync($"{PStrings.pluginRepository}archive/refs/heads/main.zip").ContinueWith((task) =>
                {
                    using var stream = task.Result.Content.ReadAsStreamAsync().Result;
                    using var fileStream = File.Create(zipFile);
                    stream.CopyTo(fileStream);
                }).Wait();

                // Extract the zip file and copy the resources.
                ZipFile.ExtractToDirectory(zipFile, Path.GetTempPath(), true);
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)) Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)) File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);

                // Cleanup temporary files.
                File.Delete(zipFile);
                Directory.Delete($"{Path.GetTempPath()}KikoGuide-main", true);

                // Broadcast an event indicating that the resources have been updated.
                ResourcesUpdated?.Invoke();
            }
            catch (Exception e) { PluginLog.Error($"ResourceManager: Error updating resource files: {e.Message}"); }
        }).Start();
    }


    /// <summary> Handles the OnResourceUpdate event. </summary>
    private void OnResourceUpdate()
    {
        PluginLog.Debug($"ResourceManager: Resources updated.");

        Setup(PluginService.PluginInterface.UiLanguage);
    }


    /// <summary> Sets up the plugin's resources. </summary>
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