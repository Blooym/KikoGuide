namespace KikoGuide.Integrations.Interfaces
{
    internal interface IIntegration
    {
        /// <summary>
        ///     The name of the integration.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The description of the integration.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Whether or not the integration should be considered disabled.
        /// </summary>
        bool ForceDisabled { get; }

        /// <summary>
        ///     The configuration of the integration.
        /// </summary>
        IIntegrationConfiguration Configuration { get; }

        /// <summary>
        ///     Enables the integration.
        /// </summary>
        void Enable();

        /// <summary>
        ///     Disables the integration.
        /// </summary>
        void Disable();

        /// <summary>
        ///     Draws the integration.
        /// </summary>
        void Draw();
    }
}
