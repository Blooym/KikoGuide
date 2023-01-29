using KikoGuide.Resources.Localization;
using Newtonsoft.Json;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    /// <summary>
    /// The configuration for instance content guides.
    /// </summary>
    internal sealed class InstanceContentGuideConfiguration : GuideConfigurationBase
    {
        /// <summary>
        /// The singleton instance of the configuration.
        /// </summary>
        public static InstanceContentGuideConfiguration Instance { get; } = Load<InstanceContentGuideConfiguration>();

        /// <inheritdoc />
        public override int Version { get; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string Name { get; } = Strings.Guide_InstanceContent_TypeName;

        /// <inheritdoc /> 
        protected override void DrawAction() => InstanceContentGuideConfigurationUI.Draw(this);

        /// <summary>
        /// Whether to automatically open the guide when entering an instance with a guide.
        /// </summary>
        public bool AutoOpen { get; set; }
    }
}