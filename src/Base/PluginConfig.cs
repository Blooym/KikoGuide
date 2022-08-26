namespace KikoGuide.Base;

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
    ///     Stores the last resource update timestamp, automatically updated by the plugin backend. Should NOT be set manually.
    /// </summary>
    public long lastResourceUpdate { get; set; } = 0;


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
    /// <param name="text">The text to write to the file</param>
    internal void WriteFile(string fileName, string text)
    {
        File.WriteAllText(Path.Combine(PluginService.PluginInterface.GetPluginConfigDirectory(), fileName), text);
    }
}
