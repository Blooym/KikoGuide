using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Configuration;
using KikoGuide.IPC;
using KikoGuide.Types;

namespace KikoGuide.Base
{
    /// <summary>
    ///     Provides access to and determines the Plugin configuration.
    /// </summary>
    [Serializable]
    internal sealed class Configuration : IPluginConfiguration
    {
        /// <summary>
        ///     The current configuration version, incremented on breaking changes.
        /// </summary>
        public int Version { get; set; }
        public AccessiblityConfiguration Accessiblity { get; set; } = new AccessiblityConfiguration();
        public DisplayConfiguration Display { get; set; } = new DisplayConfiguration();
        public IPCConfiguration IPC { get; set; } = new IPCConfiguration();

        /// <summary>
        ///     Accessibility configuration options.
        /// </summary>
        public class AccessiblityConfiguration
        {
            /// <summary> 
            ///     Whether or not to show shorter text when possible.
            /// </summary>
            public bool ShortenGuideText { get; set; }
        }

        /// <summary>
        ///     Display configuration options.
        /// </summary>
        public class DisplayConfiguration
        {
            /// <summary>

            ///     Whether or not to show the support button in the UI.
            /// </summary>
            public bool DonateButtonShown { get; set; } = true;

            /// <summary>
            ///     Whether or not to automatically show/hide a guide when entering/leaving a duty.
            /// </summary>
            public bool AutoToggleGuideForDuty { get; set; }

            /// <summary>
            ///     Whether or not to lock the position of the Duty Guide window.
            /// </summary>
            public bool PreventDutyInfoWindowMovement { get; set; }

            /// <summary>
            ///     Whether or not to prevnet resizing of the Duty Guide window.
            /// </summary>
            public bool PreventDutyInfoWindowResize { get; set; }

            /// <summary>
            ///     Mechanics that are hidden when drawing mechanics within the UI.
            /// </summary>
            public List<DutyMechanics> DisabledMechanics { get; set; } = new List<DutyMechanics>();
        }

        /// <summary>
        ///     IPC configuration options.
        /// </summary>
        public class IPCConfiguration
        {
            /// <summary>
            ///     Whether or not to show the IPC button in the UI.
            /// </summary>
            public List<IPCProviders> EnabledIntegrations { get; set; } = new List<IPCProviders>();
        }

        /// <summary>
        ///     Saves the current configuration (and any modifications) to the config file.
        /// </summary>
        internal void Save() => PluginService.PluginInterface.SavePluginConfig(this);

        /// <summary>
        ///     Reads the given file from the plugin configuration directory
        /// </summary>
        /// <param name="fileName">The file to read from, relative to the plugin configuration directory</param>
        /// <returns>The contents of the file</returns>
        internal static string ReadFile(string fileName) => File.ReadAllText(Path.Combine(PluginService.PluginInterface.GetPluginConfigDirectory(), fileName));

        /// <summary>
        ///     Writes the given file and text to the plugin configuration directory
        /// </summary>
        /// <param name="fileName">The file to write to</param>
        /// <param name="text">The text to write to the file, relative to the plugin configuration directory</param>
        internal static void WriteFile(string fileName, string text) => File.WriteAllText(Path.Combine(PluginService.PluginInterface.GetPluginConfigDirectory(), fileName), text);
    }
}