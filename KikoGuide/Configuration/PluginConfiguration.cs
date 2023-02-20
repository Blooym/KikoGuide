using System;
using Dalamud.Configuration;
using KikoGuide.Common;

namespace KikoGuide.Configuration
{
    /// <summary>
    ///     The configuration for the plugin.
    /// </summary>
    internal sealed class PluginConfiguration : IPluginConfiguration
    {
        /// <summary>
        ///     The current version of the configuration, used for migrations.
        /// </summary>
        private const int CurrentVersion = 1;

        /// <summary>
        ///     The configuration for the guide viewer.
        /// </summary>
        public GuideViewerConfiguration GuideViewer { get; set; } = new();

        /// <summary>
        ///     The version of the configuration.
        /// </summary>
        public int Version { get; set; } = CurrentVersion;

        /// <summary>
        ///     Loads the configuration and migrates it if necessary.
        /// </summary>
        /// <returns></returns>
        internal static PluginConfiguration Load()
        {
            try
            {
                return Services.PluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
            }
            catch (Exception e)
            {
                BetterLog.Error($"Failed to load configuration, making new one: {e}");
                return new PluginConfiguration();
            }
        }

        /// <summary>
        ///     Saves the configuration.
        /// </summary>
        internal void Save() => Services.PluginInterface.SavePluginConfig(this);

        /// <summary>
        ///     The configuration for the guide viewer.
        /// </summary>
        internal sealed class GuideViewerConfiguration
        {
            /// <summary>
            ///     Whether the guide viewer window posistion and size is locked.
            /// </summary>
            /// x
            public bool LockWindow { get; set; }
        }
    }
}
