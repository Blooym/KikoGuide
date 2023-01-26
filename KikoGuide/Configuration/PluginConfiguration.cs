using System;
using Dalamud.Configuration;
using KikoGuide.Common;

namespace KikoGuide.Configuration
{
    /// <summary>
    /// The configuration for the plugin.
    /// </summary>
    internal sealed class PluginConfiguration : IPluginConfiguration
    {
        /// <summary>
        /// The current version of the configuration, used for migrations.
        /// </summary>
        private const int CurrentVersion = 1;

        /// <summary>
        /// The version of the configuration.
        /// </summary>
        public int Version { get; set; } = CurrentVersion;

        /// <summary>
        /// Whether or not to automatically open the guide on instance load.
        /// </summary>
        public bool OpenGuideOnInstanceLoad { get; set; }

        /// <summary>
        /// Loads the configuration and migrates it if necessary.
        /// </summary>
        /// <returns></returns>
        internal static PluginConfiguration Load()
        {
            try
            {
                return Services.PluginInterface.GetPluginConfig() as PluginConfiguration ?? new();
            }
            catch (Exception ex)
            {
                BetterLog.Error($"Failed to load plugin configuration, reverting to default: {ex}");
                return new PluginConfiguration();
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        internal void Save() => Services.PluginInterface.SavePluginConfig(this);
    }
}