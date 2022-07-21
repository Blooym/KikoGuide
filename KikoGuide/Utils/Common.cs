namespace KikoGuide.Utils;

using System.Diagnostics;

public class Common
{
    public static void OpenBrowser(string url)
    {
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }
}