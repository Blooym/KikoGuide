namespace KikoGuide.GuideSystem.Interfaces
{
    /// <summary>
    ///     The interface for all guides configurations.
    /// </summary>
    internal interface IGuideConfiguration
    {
        /// <summary>
        ///     The version of the configuration.
        /// </summary>
        int Version { get; }

        /// <summary>
        ///     The user-friendly name of the guide-type to be displayed.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Draws the configuration layout for the guide.
        /// </summary>
        void Draw();

        /// <summary>
        ///     Saves the configuration to the file.
        /// </summary>
        void Save();

        /// <summary>
        ///     Loads the configuration from the file.
        /// </summary>
        /// <typeparam name="T">The type of the configuration to load.</typeparam>
        /// <returns>The loaded configuration.</returns>
        static T Load<T>() where T : IGuideConfiguration, new() => new();
    }
}
