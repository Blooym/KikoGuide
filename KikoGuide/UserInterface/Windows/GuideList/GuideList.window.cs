using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows.GuideList.TableParts;

namespace KikoGuide.UserInterface.Windows.GuideList
{
    internal sealed class GuideListWindow : Window
    {
        /// <inheritdoc/>
        public GuideListLogic Logic { get; } = new();

        /// <inheritdoc/>
        public GuideListWindow() : base(Constants.WindowTitles.GuideList)
        {
            this.Size = new(800, 520);
            this.SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(800, 520),
                MaximumSize = new(1200, 700),
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlags.NoScrollbar;
            this.IsOpen = true;
        }

        /// <inheritdoc/>
        public override void Draw()
        {
            if (ImGui.BeginTable("GuideList", 2, ImGuiTableFlags.BordersInnerV))
            {
                ImGui.TableSetupColumn("Sidebar", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.28f);
                ImGui.TableSetupColumn("Listings", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.72f);
                ImGui.TableNextRow();

                // Sidebar
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("GuideListSidebarChild"))
                {
                    GuideListSidebar.Draw(this.Logic);
                }
                ImGui.EndChild();

                ImGui.TableNextColumn();
                if (ImGui.BeginChild("GuideListListingsChild"))
                {
                    GuideListListings.Draw(this.Logic);
                }
                ImGui.EndChild();

                ImGui.EndTable();
            }
        }
    }
}
