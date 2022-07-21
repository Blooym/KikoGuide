namespace KikoGuide.Base;

internal static class PStrings
{
    /// <summary> The displayed plugin name insode of the Dalamud plugins list and UI titles. </summary>
    internal static readonly string pluginName = "Kiko Guide";

    /// <summary> The repository to attempt to fetch the latest resource files from. Will always attempt to use "main" branch. </summary>
    internal static readonly string pluginRepository = "https://github.com/BitsOfAByte/KikoGuide/";

    /// <summary> The path to the plugin's resources folder with trailing slashes </summary>
    public static string resourcePath = $"{Service.PluginInterface.AssemblyLocation.DirectoryName}\\Resources\\";

    /// <summary> The path to the plugin's localization folder with trailing slashes </summary>
    public static string localizationPath = resourcePath + "Localization\\";

    /// <summary> Resource fallback language 2-letter code. </summary>
    public static string fallbackLanguage = "en";
}