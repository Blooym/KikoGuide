namespace KikoGuide.Base
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using Dalamud.Configuration;
    using KikoGuide.IPC;
    using KikoGuide.Types;

    /// <summary>
    ///     Provides access to and determines the Plugin configuration.
    /// </summary>
    [Serializable]
    sealed internal class Configuration : IPluginConfiguration
    {
        /// <summary>
        ///     The current configuration version, incremented on breaking changes.
        /// </summary>
        public int Version { get; set; } = 0;
        public AccessiblityConfiguration Accessiblity { get; set; } = new AccessiblityConfiguration();
        public DisplayConfiguration Display { get; set; } = new DisplayConfiguration();
        public IPCConfiguration IPC { get; set; } = new IPCConfiguration();

        /// <summary>
        ///     Accessibility configuration options.
        /// </summary>
        public class AccessiblityConfiguration
        {
            /// <summary> 
            ///     Whether or not to show shortened strategies when available.
            /// </summary>
            public bool ShortenGuideText { get; set; } = false;
        }

        /// <summary>
        ///     Display configuration options.
        /// </summary>
        public class DisplayConfiguration
        {
            /// <summary>
            ///     Whether or not to show the support button in the UI.
            /// </summary>
            public bool SupportButtonShown { get; set; } = true;

            /// <summary>
            ///     Whether or not to automatically show a duty guide when entering a duty.
            /// </summary>
            public bool AutoOpenInDuty { get; set; } = false;

            /// <summary>
            ///     Whether or not to lock the position of the Duty Guide window.
            /// </summary>
            public bool LockDutyInfoWindowPosition { get; set; } = false;

            /// <summary>
            ///     Whether or not to prevnet resizing of the Duty Guide window.
            /// </summary>
            public bool PreventDutyInfoWindowResize { get; set; } = false;

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
        internal void Save()
        {
            PluginService.PluginInterface.SavePluginConfig(this);
        }

        /// <summary>
        ///     Reads the given file from the plugin configuration directory
        /// </summary>
        /// <param name="fileName">The file to read from, relative to the plugin configuration directory</param>
        /// <returns>The contents of the file</returns>
        internal string ReadFile(string fileName)
        {
            return File.ReadAllText(Path.Combine(PluginService.PluginInterface.GetPluginConfigDirectory(), fileName));
        }

        /// <summary>
        ///     Writes the given file and text to the plugin configuration directory
        /// </summary>
        /// <param name="fileName">The file to write to</param>
        /// <param name="text">The text to write to the file, relative to the plugin configuration directory</param>
        internal void WriteFile(string fileName, string text)
        {
            File.WriteAllText(Path.Combine(PluginService.PluginInterface.GetPluginConfigDirectory(), fileName), text);
        }
    }
}