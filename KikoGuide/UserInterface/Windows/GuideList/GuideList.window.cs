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

        public override void Draw()
        {
            if (!GuideListLogic.IsLoggedIn)
            {
                ImGui.TextUnformatted("Please log in to a character in order to view guides.");
                return;
            }

            // No guides available
            if (GuideListLogic.UnlockedGuides == 0)
            {
                DrawNoGuidesAvailable();
                return;
            }

            // Quick fix for the table crashing using docking
            if (ImGui.GetWindowSize().X < 80 || ImGui.GetWindowSize().Y < 80)
            {
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
        /// Draw a warning message if no guides were found.
        /// </summary>
        private static void DrawNoGuidesAvailable()
        {
            ImGui.TextUnformatted("You have not yet unlocked any guides! Check back another time.");
            ImGui.TextUnformatted("If you believe this is an error, please contact the developer.");
        }
    }
}
