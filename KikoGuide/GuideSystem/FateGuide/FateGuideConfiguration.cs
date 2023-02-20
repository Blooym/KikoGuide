using KikoGuide.Resources.Localization;
using Newtonsoft.Json;

namespace KikoGuide.GuideSystem.FateGuide
{
    /// <summary>
    ///     The configuration for instance content guides.
    /// </summary>
    internal sealed class FateGuideConfiguration : GuideConfigurationBase
    {
        /// <summary>
        ///     The singleton instance of the configuration.
        /// </summary>
        public static FateGuideConfiguration Instance { get; } = Load<FateGuideConfiguration>();

        /// <inheritdoc />
        public override int Version { get; }

        /// <inheritdoc />
        [JsonIgnore] public override string Name => Strings.Guide_Fate_TypeName;

        /// <summary>
        ///     Whether to automatically open the guide when entering an instance with a guide.
        /// </summary>
        public bool AutoOpen { get; set; }

        /// <inheritdoc />
        protected override void DrawAction() => FateGuideConfigurationUI.Draw(this);
    }
}
