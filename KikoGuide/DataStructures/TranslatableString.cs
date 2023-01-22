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
        /// <returns>The string for the specified ClientLanguage, or the English string if the ClientLanguage is not supported.</returns>
        public string this[ClientLanguage language] => language switch
        {
            ClientLanguage.English => this.EN,
            ClientLanguage.German => string.IsNullOrEmpty(this.DE) ? this.EN : this.DE,
            ClientLanguage.French => string.IsNullOrEmpty(this.FR) ? this.EN : this.FR,
            ClientLanguage.Japanese => string.IsNullOrEmpty(this.JA) ? this.EN : this.JA,
            _ => this.EN,
        };

        /// <summary>
        ///     Gets the string for the current ClientLanguage.
        /// </summary>
        [JsonIgnore] public string Current => this[Services.ClientState.ClientLanguage];
    }
}