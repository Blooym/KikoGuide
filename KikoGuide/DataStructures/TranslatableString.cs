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
        ///     Gets the string for the current game language or English if not found.
        /// </summary>
        [JsonIgnore] public string GameCurrent => this[Services.ClientState.ClientLanguage];

        /// <summary>
        ///     Returns the string for the current Dalamud UI language or English if not found.
        /// </summary>
        [JsonIgnore] public string UICurrent => this[Services.PluginInterface.UiLanguage];


        /// <summary>
        ///     Whether the string is null or empty.
        /// </summary>
        /// <returns>True if the string is null or empty, false otherwise.</returns>
        public bool IsNullOrEmpty() => string.IsNullOrEmpty(this.EN);

        /// <summary>
        ///     Whether the string is null or whitespace.
        /// </summary>
        /// <returns>True if the string is null or whitespace, false otherwise.</returns>
        public bool IsNullOrWhiteSpace() => string.IsNullOrWhiteSpace(this.EN);
    }
}
