using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Windowing;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerWindow : Window
    {
        /// <inheritdoc/>
        public GuideViewerLogic Logic { get; } = new();

        /// <inheritdoc/>
        public GuideViewerWindow() : base(Constants.Windows.GuideViewerTitle)
        {
            this.Size = new(400, 300);
            this.SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(400, 300),
                MaximumSize = new(1920, 1080),
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlags.NoScrollbar;

            if (GuideViewerLogic.Configuration.LockGuideViewer)
            {
                this.Flags |= ImGuiWindowFlagExtras.LockedPosAndSize;
            }
        }

        /// <inheritdoc/>
        public override bool DrawConditions() => GuideViewerLogic.GetSelectedGuide() != null;

        /// <inheritdoc/>
        public override void Draw()
        {
            var selectedGuide = GuideViewerLogic.GetSelectedGuide();
            var guideViewerLocked = GuideViewerLogic.Configuration.LockGuideViewer;
            if (selectedGuide == null)
            {
                return;
            }

            // Heading shared by all guides
            if (ImGui.BeginChild("GuideViewerHeading", new(0, 30)))
            {
                // Icon and name
                SiGui.Icon(selectedGuide.Icon, ScalingMode.None, new(ImGuiHelpers.GlobalScale * 20));
                ImGui.SameLine();
                ImGui.TextDisabled(selectedGuide.Name);
                ImGui.SameLine();

                // Window actions
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - (ImGuiHelpers.GlobalScale * 15 * 2));
                ImGui.BeginGroup();
                if (ImGuiComponents.IconButton(guideViewerLocked ? FontAwesomeIcon.Lock : FontAwesomeIcon.Unlock))
                {
                    this.Flags ^= ImGuiWindowFlagExtras.LockedPosAndSize;
                    GuideViewerLogic.Configuration.LockGuideViewer = !guideViewerLocked;
                    GuideViewerLogic.Configuration.Save();

                }
                ImGui.EndGroup();
                ImGui.Separator();
                ImGui.EndChild();
            }

            // The guide's content
            if (ImGui.BeginChild("GuideViewerContent"))
            {
                selectedGuide.Draw();
                ImGui.EndChild();
            }
        }
    }
}
