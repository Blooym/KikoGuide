namespace KikoGuide.Base
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using Dalamud.Configuration;
    using KikoGuide.Managers.IPC;

    /// <summary>
    ///     Provides access to and determines the Plugin configuration.
    /// </summary>
    [Serializable]
    sealed internal class Configuration : IPluginConfiguration
    {
        /// <summary> 
        ///     The current configuration version. Incremented whenever breaking changes are made to the configuration.
        /// </summary>
        public int Version { get; set; } = 0;

        /// <summary> 
        ///     Whether or not to automatically show duty information upon entering a new (supported) duty.
        /// </summary>
        public bool autoOpenDuty { get; set; } = false;

        /// <summary> 
        ///     Whether or not to show the support button in the UI. 
        /// </summary>
        public bool supportButtonShown { get; set; } = true;

        /// <summary> 
        ///     Whether or not to show shortened strategies when available.
        /// </summary>
        public bool shortenStrategies { get; set; } = false;

        /// <summary> 
        ///     Determines which mechanics are hidden when drawing mechanics within the UI. 
        /// </summary>
        public List<int> hiddenMechanics { get; set; } = new List<int>();

        /// <summary>
        ///     A list of enabled integrations.
        /// </summary>
        public List<IPCProviders> enabledIntegrations { get; set; } = new List<IPCProviders>();

        /// <summary>
        ///     Saves the current configuration (and any modifications) to the config file.
        /// </summary>
        internal void Save()
        {
            PluginService.PluginInterface.SavePluginConfig(this);
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

        /// <summary>
        ///     Reads the given file from the plugin configuration directory
        /// </summary>
        /// <param name="fileName">The file to read from, relative to the plugin configuration directory</param>
        /// <returns>The contents of the file</returns>
        internal string ReadFile(string fileName)
        {
            return File.ReadAllText(Path.Combine(PluginService.PluginInterface.GetPluginConfigDirectory(), fileName));
        }
    }
}