using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows.GuideList.TableParts;

namespace KikoGuide.UserInterface.Windows.GuideList
{
    internal sealed class GuideListWindow : Window
    {
        public GuideListLogic Logic { get; } = new();
        public GuideListWindow() : base(Constants.Windows.GuideListTitle)
        {
            this.Size = new(800, 460);
            this.SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(800, 460),
                MaximumSize = new(7680, 4320),
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlags.NoScrollbar;
            this.IsOpen = true;
        }

        public override void Draw()
        {
            // No guides available
            if (GuideListLogic.UnlockedGuides == 0)
            {
                DrawNoGuidesAvailable();
                return;
            }

            if (ImGui.BeginTable("GuideList", 2, ImGuiTableFlags.BordersInnerV))
            {
                ImGui.TableSetupColumn("Sidebar", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.28f);
                ImGui.TableSetupColumn("Listings", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.72f);

                // Sidebar
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("SidebarChild"))
                {
                    Sidebar.Draw(this.Logic);
                    ImGui.EndChild();
                }

                // Listings
                ImGui.TableNextColumn();
                if (ImGui.BeginChild("ListingsChild"))
                {
                    Listings.Draw(this.Logic);
                    ImGui.EndChild();
                }

                ImGui.EndTable();
            }
        }

        /// <summary>
        ///     Draw a warning message if no guides were found.
        /// </summary>
        private static void DrawNoGuidesAvailable()
        {
            ImGui.TextUnformatted("You have not yet unlocked any guides! Check back another time.");
            ImGui.TextUnformatted("If you believe this is an error, please contact the developer.");
        }
    }
}
