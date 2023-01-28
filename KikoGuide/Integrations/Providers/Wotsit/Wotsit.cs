using System;

namespace KikoGuide.Integrations.Providers.Wotsit
{
    internal sealed class Wotsit : IntegrationBase
    {
        public override bool Activated { get; protected set; }
        public override WotsitConfiguration Configuration { get; } = IntegrationConfigurationBase.Load<WotsitConfiguration>();

        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {

        }

        protected override Action? DrawAction { get; } = () =>
        {
        };
    }
}