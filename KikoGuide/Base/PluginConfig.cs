namespace KikoGuide.Base;

using System;
using System.Collections.Generic;

using Dalamud.Configuration;

[Serializable]
class Configuration : IPluginConfiguration
{
    // The configuration version.
    public int Version { get; set; } = 0;

    // Whether or not to automatically show duty information upon entering a new duty.
    public bool autoOpenDuty { get; set; } = false;

    // Whether or not to show the support button in the UI.
    public bool supportButtonShown { get; set; } = true;

    // Whether or not to show TLDRs of duty information when available.
    public bool shortenStrategies { get; set; } = false;

    // This setting determines which mechanics are hidden when drawing tables of boss mechanics within the UI.
    public List<int> hiddenMechanics { get; set; } = new List<int>();

    // Stores the last resource update timestamp.
    public long lastResourceUpdate { get; set; } = 0;

#if DEBUG
    // This setting determines where to output localizable files to when using the export button, only available in debug builds.
    public string localizableOutputDir { get; set; } = Service.PluginInterface.AssemblyLocation?.Directory?.FullName ?? "";
    int IPluginConfiguration.Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#endif


    protected internal void Save()
    {
        Service.PluginInterface.SavePluginConfig(this);
    }
}
