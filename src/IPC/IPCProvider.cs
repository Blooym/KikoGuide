using KikoGuide.Attributes;

namespace KikoGuide.IPC
{
    /// <summary>
    ///     Contains all IPC IDs for the plugin which are automatically added to the UI.
    /// </summary>
    public enum IPCProviders
    {
        [Description("Integrate with Wotsit to provide search capibilities.")]
        Wotsit,

        // [Description("Integrate with Tippy to provide awful tooltips.")]
        // Tippy
    }
}
