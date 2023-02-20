using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows.PluginSettings.TableParts;

namespace KikoGuide.UserInterface.Windows.PluginSettings
{
    internal sealed class PluginSettingsWindow : Window
    {

        /// <inheritdoc />
        public PluginSettingsWindow() : base(Constants.WindowTitles.Settings)
        {
            this.Size = new Vector2(600, 400);
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(600, 400), MaximumSize = new Vector2(1200, 700),
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlags.NoScrollbar;
        }

        /// <inheritdoc />
        public PluginSettingsLogic Logic { get; } = new();

        /// <inheritdoc />
        public override void Draw()
        {
            if (ImGui.BeginTable("PluginSettings", 2, ImGuiTableFlags.BordersInnerV))
            {
                ImGui.TableSetupColumn("PluginSettingsSidebar", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.3f);
                ImGui.TableSetupColumn("PluginSettingsList", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.7f);
                ImGui.TableNextRow();

                // Sidebar
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("PluginSettingsSidebarChild"))
                {
                    PluginSettingsSidebar.Draw(this.Logic);
                }
                ImGui.EndChild();

                // Listings
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("PluginSettingsListChild"))
                {
                    PluginSettingsActive.Draw(this.Logic);
                }
                ImGui.EndChild();

                ImGui.EndTable();
            }
        }
    }
}
