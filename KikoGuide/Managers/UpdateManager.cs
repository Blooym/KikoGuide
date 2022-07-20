namespace KikoGuide.Managers;

using System;
using System.Net;
using System.Threading;
using System.IO;
using System.IO.Compression;

using Dalamud.Logging;
using KikoGuide.Base;

internal static class UpdateManager
{
    // The status of the last attempted updated, false if failed, true if successful. 
    internal static bool? lastUpdateSuccess;

    // If an update is currently being attempted, this will be true.
    internal static bool updateInProgress;

    // <summary>
    // Downloads the repository from GitHub and extracts the resource data.
    // </summary>
    internal static void UpdateResources()
    {
        // To prevent blocking the main thread, we'll use a background thread.
        Thread downloadThread = new Thread(() =>
        {

            try
            {
                PluginLog.Debug($"UpdateManager: Opening new thread to handle duty data download.");
                updateInProgress = true;

                // Create a new WebClient to download the data and some paths for installation.
                var webClient = new WebClient();
                var zipFile = Path.Combine(Path.GetTempPath(), "KikoGuide_Source.zip");
                var sourcePath = Path.Combine(Path.GetTempPath(), "KikoGuide-main", "KikoGuide", "Resources");
                var targetPath = Path.Combine(Utils.FS.resourcePath);

                // Download the file into the system temp directory to make sure it can be cleaned up by the OS incase of a crash.
                webClient.DownloadFile($"{PStrings.PluginRepository}archive/refs/heads/main.zip", zipFile);

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
                Service.Configuration.lastResourceUpdate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Service.Configuration.Save();

                // Trigger a UI reload to show the new data.
                KikoPlugin.OnLanguageChange(Service.PluginInterface.UiLanguage);
            }

            catch (Exception e)
            {
                // Set update statuses to their values & log the error.
                lastUpdateSuccess = false;
                updateInProgress = false;
                PluginLog.Error($"UpdateManager: Error updating resource files: {e.Message}");
            }

        });

        downloadThread.Start();
    }
}