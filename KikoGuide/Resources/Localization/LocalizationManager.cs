using System;
using System.Globalization;
using KikoGuide.Common;

namespace KikoGuide.Resources.Localization
{
    internal sealed class LocalizationManager : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Gets the singleton instance of <see cref="LocalizationManager" />.
        /// </summary>
        public static LocalizationManager Instance { get; } = new();

        /// <summary>
        /// Creates a new resource manager and sets up resources.
        /// </summary>
        private LocalizationManager()
        {
            SetupLocalization(Services.PluginInterface.UiLanguage);
            Services.PluginInterface.LanguageChanged += SetupLocalization;
        }

        /// <summary>
        /// Disposes of the <see cref="LocalizationManager" />
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                Services.PluginInterface.LanguageChanged -= SetupLocalization;
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Sets up localization for the given language, or uses fallbacks if not found.
        /// </summary>
        /// <param name="language">The language to use.</param>
        private static void SetupLocalization(string language)
        {
            try
            {
                BetterLog.Information($"Setting up localization for {language}");
                Strings.Culture = new CultureInfo(language);
            }
            catch (Exception e)
            {
                BetterLog.Error($"Failed to set language to {language}: {e.Message}");
            }
        }
    }
}
