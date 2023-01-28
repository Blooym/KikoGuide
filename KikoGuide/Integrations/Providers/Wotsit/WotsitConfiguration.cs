namespace KikoGuide.Integrations.Providers.Wotsit
{
    internal sealed class WotsitConfiguration : IntegrationConfigurationBase
    {
        public override int Version { get; } = 1;
        public override string Name { get; } = "Wotsit";
    }
}