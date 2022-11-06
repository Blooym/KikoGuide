using KikoGuide.Base;
using KikoGuide.IPC;

namespace KikoGuide.UI.ImGuiFullComponents.IPCProviderCombo
{
    public sealed class IPCProviderComboPresenter
    {
        internal static Configuration Configuration => PluginService.Configuration;
        internal static IPCLoader IPC => PluginService.IPC;
    }
}
