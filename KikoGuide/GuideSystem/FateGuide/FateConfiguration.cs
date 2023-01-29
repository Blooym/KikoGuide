using KikoGuide.Resources.Localization;
using Newtonsoft.Json;

namespace KikoGuide.GuideSystem.FateGuide
{
    /// <summary>
    /// The configuration for instance content guides.
    /// </summary>
    internal sealed class FateConfiguration : GuideConfigurationBase
    {
        /// <summary>
        /// The singleton instance of the configuration.
        /// </summary>
        public static FateConfiguration Instance { get; } = Load<FateConfiguration>();

        /// <inheritdoc />
        public override int Version { get; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string Name => Strings.Guide_Fate_TypeName;

        /// <inheritdoc /> 
        protected override void DrawAction() => FateConfigurationUI.Draw(this);

        /// <summary>
        /// Whether to automatically open the guide when entering an instance with a guide.
        /// </summary>
        public bool AutoOpen { get; set; }
    }
}