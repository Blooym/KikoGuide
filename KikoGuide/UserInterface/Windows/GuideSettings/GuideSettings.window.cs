using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows.GuideSettings.TableParts;

namespace KikoGuide.UserInterface.Windows.GuideSettings
{
    internal sealed class GuideSettingsWindow : Window
    {

        /// <inheritdoc />
        public GuideSettingsWindow() : base(Constants.WindowTitles.GuideTypeSettings)
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
        public GuideSettingsLogic Logic { get; } = new();

        /// <inheritdoc />
        public override void Draw()
        {
            if (ImGui.BeginTable("GuideSettings", 2, ImGuiTableFlags.BordersInnerV))
            {
                ImGui.TableSetupColumn("GuideSettingsSidebar", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.3f);
                ImGui.TableSetupColumn("GuideSettingsList", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.7f);
                ImGui.TableNextRow();

                // Sidebar
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("GuideSettingsSidebarChild"))
                {
                    GuideSettingsSidebar.Draw(this.Logic);
                }
                ImGui.EndChild();

                // Listings
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("GuideSettingsListChild"))
                {
                    GuideSettingsActive.Draw(this.Logic);
                }
                ImGui.EndChild();

                ImGui.EndTable();
            }
        }
    }
}
