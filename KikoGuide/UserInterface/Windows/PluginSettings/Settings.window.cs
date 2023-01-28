using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.UserInterface.Windows.PluginSettings
{
    internal sealed class PluginSettingsWindow : Window
    {
        /// <inheritdoc/>
        public PluginSettingsLogic Logic { get; } = new();

        /// <inheritdoc/>
        public PluginSettingsWindow() : base(Constants.Windows.SettingsTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        /// <inheritdoc/>
        public override void Draw()
        {

        }
    }
}
