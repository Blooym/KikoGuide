using KikoGuide.Resources.Localization;
using Newtonsoft.Json;

namespace KikoGuide.GuideSystem.InstanceGuide
{
    /// <summary>
    /// The configuration for instance content guides.
    /// </summary>
    internal sealed class InstanceGuideConfiguration : GuideConfigurationBase
    {
        /// <summary>
        /// The singleton instance of the configuration.
        /// </summary>
        public static InstanceGuideConfiguration Instance { get; } = Load<InstanceGuideConfiguration>();

        /// <inheritdoc />
        public override int Version { get; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string Name => Strings.Guide_InstanceContent_TypeName;

        /// <inheritdoc /> 
        protected override void DrawAction() => InstanceGuideConfigurationUI.Draw(this);

        /// <summary>
        /// Whether to automatically open the guide when entering an instance with a guide.
        /// </summary>
        public bool AutoOpen { get; set; }
    }
}