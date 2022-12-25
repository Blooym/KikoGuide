using KikoGuide.Attributes;

namespace KikoGuide.IPC
{
    /// <summary>
    ///     Contains all IPC IDs for the plugin which are automatically added to the UI.
    /// </summary>
    internal enum IPCProviders
    {
        [Description("Integrate with Wotsit to provide search capibilities.")]
        Wotsit = 0,
    }
}
