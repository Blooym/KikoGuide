using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows.GuideList.TableParts;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Windowing;

namespace KikoGuide.UserInterface.Windows.GuideList
{
    internal sealed class GuideListWindow : Window
    {
        public GuideListLogic Logic { get; } = new();
        public GuideListWindow() : base(Constants.Windows.GuideListTitle)
        {
            this.Size = new(800, 380);
            this.SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(600, 380),
                MaximumSize = new(7680, 4320),
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlagExtras.NoScroll;
            this.IsOpen = true;
        }

        public override void Draw()
        {
            // No guides found, show a warning message.
            if (GuideListLogic.TotalGuides == 0)
            {
                DrawNoGuidesFound();
                return;
            }

            // Begin the table.
            if (ImGui.BeginTable("GuideList", 2, ImGuiTableFlags.BordersInnerV))
            {
                ImGui.TableSetupColumn("Sidebar", ImGuiTableColumnFlags.WidthFixed, ImGui.GetWindowWidth() * 0.30f);
                ImGui.TableSetupColumn("Listings", ImGuiTableColumnFlags.WidthFixed, ImGui.GetWindowWidth() * 0.70f);

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
        ///     Draw a warning message if no guides were found.
        /// </summary>
        private static void DrawNoGuidesFound()
        {
            SiGui.TextWrappedColoured(ImGuiColors.DalamudRed, "WARNING! No guides were able to load.");
            ImGui.Separator();

            SiGui.TextWrappedColoured(ImGuiColors.DalamudOrange, "Your installation may be corrupted or otherwise broken, if you encounter this issue please report it on GitHub.");
            ImGui.Dummy(new(0, 10));

            if (ImGui.Button("Report on GitHub"))
            {
                Util.OpenLink(Constants.Links.GitHub);
            }
        }
    }
}
