using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows.IntegrationSettings.TableParts;

namespace KikoGuide.UserInterface.Windows.IntegrationSettings
{
    internal sealed class IntegrationSettingsWindow : Window
    {
        /// <inheritdoc/>
        public IntegrationSettingsLogic Logic { get; } = new();

        /// <inheritdoc/>
        public IntegrationSettingsWindow() : base(Constants.Windows.IntegrationSettingsTitle)
        {
            this.Size = new(600, 400);
            this.SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(600, 400),
                MaximumSize = new(1200, 700),
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlags.NoScrollbar;
        }

        /// <inheritdoc/>
        public override void Draw()
        {
            if (ImGui.GetWindowSize().X < 80 || ImGui.GetWindowSize().Y < 80)
            {
                return;
            }

            if (ImGui.BeginTable("IntegrationSettings", 2, ImGuiTableFlags.BordersInnerV))
            {
                ImGui.TableSetupColumn("IntegrationsSidebar", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.3f);
                ImGui.TableSetupColumn("IntegartionsList", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.7f);

                // Sidebar
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("IntegrationsSidebarChild"))
                {
                    IntegrationsSidebar.Draw(this.Logic);
                    ImGui.EndChild();
                }

                ImGui.TableNextColumn();
                if (ImGui.BeginChild("IntegrationsListChild"))
                {

                    SelectedIntegration.Draw(this.Logic);
                    ImGui.EndChild();
                }

                ImGui.EndTable();
            }
        }
    }
}