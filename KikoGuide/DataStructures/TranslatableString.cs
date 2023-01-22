using Dalamud;
using KikoGuide.Common;
using Newtonsoft.Json;

namespace KikoGuide.DataStructures
{
    /// <summary>
    ///     Represents a translatable string.
    /// </summary>
    public record struct TranslatableString
    {
        public required string EN { get; init; } // Default language
        public string DE { get; init; }
        public string FR { get; init; }
        public string JA { get; init; }

        /// <summary>
        ///     Gets the string for given ClientLanguage.
        /// </summary>
        /// <param name="language">The ClientLanguage to get the string for.</param>
        /// <returns>The string for the specified ClientLanguage, or the English string if the ClientLanguage is not supported or missing in the data.</returns>
        public string this[ClientLanguage language] => language switch
        {
            ClientLanguage.English => this.EN,
            ClientLanguage.German => string.IsNullOrEmpty(this.DE) ? this.EN : this.DE,
            ClientLanguage.French => string.IsNullOrEmpty(this.FR) ? this.EN : this.FR,
            ClientLanguage.Japanese => string.IsNullOrEmpty(this.JA) ? this.EN : this.JA,
            _ => this.EN,
        };

        /// <summary>
        ///     Gets the string for given ISO code.
        /// </summary>
        /// <param name="isoCode">The ISO code to get the string for.</param>
        /// <returns>The string for the specified ISO code, or the English string if the ISO code is not supported or missing in the data.</returns>
        public string this[string isoCode] => isoCode switch
        {
            "en" => this.EN,
            "de" => string.IsNullOrEmpty(this.DE) ? this.EN : this.DE,
            "fr" => string.IsNullOrEmpty(this.FR) ? this.EN : this.FR,
            "ja" => string.IsNullOrEmpty(this.JA) ? this.EN : this.JA,
            _ => this.EN,
        };

        /// <summary>
        ///     Gets the string for the current ClientLanguage.
        /// </summary>
        [JsonIgnore] public string CCurrent => this[Services.ClientState.ClientLanguage];

        /// <summary>
        ///     Gets the string for the current ISO code.
        /// </summary>
        [JsonIgnore] public string UICurrent => this[Services.PluginInterface.UiLanguage];
    }
}
