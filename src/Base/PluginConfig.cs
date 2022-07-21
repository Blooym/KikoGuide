namespace KikoGuide.Base;

using System;
using System.Collections.Generic;
using Dalamud.Configuration;

[Serializable]
class Configuration : IPluginConfiguration
{
    /// <summary> 
    ///     The current configuration version. Incremented whenever breaking changes are made to the configuration.
    /// </summary>
    public int Version { get; set; } = 0;


    /// <summary> 
    ///     Whether or not to automatically show duty information upon entering a new (supported) duty.
    ///     Handled by the TerritoryChange event listener inside of <see cref="UI.UIState"/>. 
    /// </summary>
    public bool autoOpenDuty { get; set; } = false;


    /// <summary> 
    ///     Whether or not to show the support button in the UI. 
    ///     UI elements must check this value when being drawn if they contain a support button.
    /// </summary>
    public bool supportButtonShown { get; set; } = true;


    /// <summary> 
    ///     Whether or not to show shortened strategies when available.
    ///     This setting is only respected when <see cref="Managers.Boss" /> contains a valid "TLDR" key
    /// </summary>
    public bool shortenStrategies { get; set; } = false;


    /// <summary> 
    ///     Determines which mechanics are hidden when drawing mechanics within the UI. 
    ///     UI elements using mechanic data should check this value when being drawn and handle it accordingly.
    /// </summary>
    public List<int> hiddenMechanics { get; set; } = new List<int>();


    /// <summary> 
    ///     Stores the last resource update timestamp, automatically updated by the plugin backend. Should NOT be set manually.
    /// </summary>
    public long lastResourceUpdate { get; set; } = 0;


#if DEBUG
    /// <summary> 
    ///     The output location of localizable files to when using the export button. Available to debug builds only.
    /// </summary>
    public string localizableOutputDir { get; set; } = Service.PluginInterface.AssemblyLocation?.Directory?.FullName ?? "";
    int IPluginConfiguration.Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#endif


    /// <summary>
    ///    Saves the current configuration (and any modifications) to the config file.
    /// </summary>
    internal void Save()
    {
        Service.PluginInterface.SavePluginConfig(this);
    }
}
