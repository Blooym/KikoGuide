using Dalamud.Configuration;
using KikoGuide.Common;

namespace KikoGuide.Configuration
{
    /// <summary>
    ///     The configuration for the plugin.
    /// </summary>
    internal sealed class PluginConfiguration : IPluginConfiguration
    {
        private const int CurrentVersion = 1;

        /// <summary>
        ///     The current version of the configuration, used for migrations.
        /// </summary>
        public int Version { get; set; } = CurrentVersion;

        /// <summary>
        ///     Whether or not to automatically open the guide on instance load.
        /// </summary>
        public bool OpenGuideOnInstanceLoad { get; set; }

        /// <summary>
        ///     Saves the configuration, migrating it if necessary.
        /// </summary>
        internal void Save()
        {
            if (this.Version != CurrentVersion)
            {
                Migrate();
            }

            Services.PluginInterface.SavePluginConfig(this);
        }

        /// <summary>
        ///     Migrates the configuration to the current version.
        /// </summary>
        internal static void Migrate() { }
    }
}