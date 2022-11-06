namespace KikoGuide.Base
{
    /// <summary> 
    ///     A collection of constants used throughout the plugin.
    /// </summary>
    internal sealed class PluginConstants
    {
        /// <summary> 
        ///    This is the name that will be shown in all UI elements, does not change InternalName.
        /// </summary>
        internal static readonly string PluginName = "Kiko Guide";

        /// <summary>
        ///     The repository to linked to this plugin.
        /// </summary>
        internal static readonly string RepoUrl = "https://github.com/BitsOfAByte/KikoGuide/";

        /// <summary>
        ///     The raw repository to linked to this plugin.
        /// </summary>
        internal static readonly string RawRepoUrl = "https://raw.githubusercontent.com/BitsOfAByte/KikoGuide/";

        /// <summary>
        ///     The production branch of the repository.
        /// </summary>
        internal static readonly string RepoBranch = "main";

        /// <summary>
        ///     The resources directory relative to the base of the repository.
        /// </summary>
        internal static readonly string RepoResourcesDir = "src/Resources/";

        /// <summary>
        ///     The support button URL.
        /// </summary>
        internal static readonly string DonateButtonUrl = "https://github.com/sponsors/BitsOfAByte";

        /// <summary>
        ///     The path to the plugin's resources folder with trailing slashes, relative to the plugin assembly location with trailing slashes.
        /// </summary>
        internal static readonly string PluginResourcesDir = $"{PluginService.PluginInterface.AssemblyLocation.DirectoryName}\\Resources\\";

        /// <summary>
        ///     The path to the plugin's localization folder with trailing slashes.
        /// </summary>
        internal static readonly string PluginlocalizationDir = PluginResourcesDir + "Localization\\";

        /// <summary>
        ///    The fallback language to use if the user's language is not supported for localization (ISO 639-1 code).
        /// </summary>
        internal static readonly string FallbackLanguage = "en";
    }
}