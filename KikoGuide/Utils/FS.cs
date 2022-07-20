namespace KikoGuide.Utils;

using System.Diagnostics;
using KikoGuide.Base;

public class FS
{
    public static string resourcePath = Service.PluginInterface.AssemblyLocation.DirectoryName + "\\Resources\\";

    public static void OpenBrowser(string url)
    {
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }
}