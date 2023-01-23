using System;
using System.IO;
using System.Reflection;
using CheapLoc;
using KikoGuide.Common;

namespace KikoGuide.Resources
{
    public sealed class ResourceManager : IDisposable
    {
        /// <summary>
        ///     The singleton instance of <see cref="ResourceManager" />.
        /// </summary>
        private static ResourceManager? instance;

        /// <summary>
        ///     Gets the singleton instance of <see cref="ResourceManager" />.
        /// </summary>
        public static ResourceManager Instance => instance ??= new();

        /// <summary>
        ///     Creates a new resource manager and sets up resources.
        /// </summary>
        private ResourceManager()
        {
            SetupLocalization(Services.PluginInterface.UiLanguage);
            Services.PluginInterface.LanguageChanged += this.OnLanguageChange;
        }

        /// <summary>
        ///     Disposes of the <see cref="ResourceManager" />
        /// </summary>
        public void Dispose()
        {
            Services.PluginInterface.LanguageChanged -= this.OnLanguageChange;
            instance = null!;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Language change handler.
        /// </summary>
        /// <param name="newLanguage">The new language</param>
        private void OnLanguageChange(string newLanguage) => SetupLocalization(newLanguage);

        /// <summary>
        ///     Sets up localization for the given language, or uses fallbacks if not found.
        /// </summary>
        /// <param name="language">The language to use.</param>
        private static void SetupLocalization(string language)
        {
            try
            {
                using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"KikoGuide.Resources.Localization.{language}.json");

                if (resource == null)
                {
                    throw new FileNotFoundException($"Could not find resource file for language {language}.");
                }

                using var reader = new StreamReader(resource);
                Loc.Setup(reader.ReadToEnd());
                BetterLog.Debug($"Loaded localization for language {language}.");
            }
            catch (Exception)
            {
                BetterLog.Debug("Using fallback language for localization.");
                Loc.SetupWithFallbacks();
            }
        }
    }
}
