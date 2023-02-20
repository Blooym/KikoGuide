namespace KikoGuide.Integrations.Interfaces
{
    internal interface IIntegrationConfiguration
    {
        /// <summary>
        ///     The version of the configuration.
        /// </summary>
        int Version { get; }

        /// <summary>
        ///     Whether or not the integration is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        ///     Draws the configuration layout for the integration.
        /// </summary>
        void Save();

        /// <summary>
        ///     Loads the configuration from the file.
        /// </summary>
        /// <typeparam name="T">The type of the configuration to load.</typeparam>
        /// <returns>The loaded configuration.</returns>
        static T Load<T>() where T : IIntegrationConfiguration, new() => new();
    }
}
