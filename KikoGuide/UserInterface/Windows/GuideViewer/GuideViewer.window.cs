using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.GuideSystem;
using KikoGuide.Resources.Localization;
using KikoGuide.UserInterface.Windows.GuideViewer.Tabs;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Windowing;

namespace KikoGuide.UserInterface.Windows.GuideViewer
{
    internal sealed class GuideViewerWindow : Window
    {

        /// <inheritdoc />
        public GuideViewerWindow() : base(Constants.WindowTitles.GuideViewer)
        {
            this.Size = new Vector2(450, 450);
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(450, 450), MaximumSize = new Vector2(1920, 1080),
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlags.NoScrollbar;

            if (GuideViewerLogic.Configuration.GuideViewer.LockWindow)
            {
                this.Flags |= ImGuiWindowFlagExtra.LockedPosAndSize;
            }
        }

        /// <inheritdoc />
        public GuideViewerLogic Logic { get; } = new();

        /// <inheritdoc />
        public override bool DrawConditions() => GuideViewerLogic.GetSelectedGuide() != null;

        /// <inheritdoc />
        public override void Draw()
        {
            var selectedGuide = GuideViewerLogic.GetSelectedGuide();

            if (selectedGuide == null)
            {
                return;
            }

            // Heading shared by all guides
            if (ImGui.BeginChild("GuideViewerHeading", new Vector2(0, 30)))
            {
                this.DrawHeading(selectedGuide);
            }
            ImGui.EndChild();

            // The active content of the guide
            if (ImGui.BeginChild("GuideViewerContent", default, true))
            {
                switch (this.Logic.ActiveTab)
                {
                    case GuideViewerLogic.SelectedTabState.Guide:
                        GuideViewerContent.Draw(this.Logic, selectedGuide);
                        break;
                    case GuideViewerLogic.SelectedTabState.Note:
                        GuideViewerNote.Draw(this.Logic, selectedGuide.Note);
                        break;
                }
            }
            ImGui.EndChild();
        }

        private void DrawHeading(GuideBase selectedGuide)
        {
            var guideViewerLocked = GuideViewerLogic.Configuration.GuideViewer.LockWindow;

            // Icon and name
            SiGui.Icon(selectedGuide.Icon, ScalingMode.None, new Vector2(ImGuiHelpers.GlobalScale * 20));
            ImGui.SameLine();
            SiGui.TextDisabled(selectedGuide.Name);
            ImGui.SameLine();

            // Window actions
            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - ImGuiHelpers.GlobalScale * 25 * 2);
            ImGui.BeginGroup();
            if (ImGuiComponents.IconButton(guideViewerLocked ? FontAwesomeIcon.Lock : FontAwesomeIcon.Unlock))
            {
                this.Flags ^= ImGuiWindowFlagExtra.LockedPosAndSize;
                GuideViewerLogic.Configuration.GuideViewer.LockWindow = !guideViewerLocked;
                GuideViewerLogic.Configuration.Save();
            }
            SiGui.AddTooltip(guideViewerLocked ? Strings.UserInterface_GuideViewer_Tooltip_UnlockViewer : Strings.UserInterface_GuideViewer_Tooltip_LockViewer);

            ImGui.SameLine();
            if (ImGuiComponents.IconButton(this.Logic.ActiveTab == GuideViewerLogic.SelectedTabState.Guide ? FontAwesomeIcon.StickyNote : FontAwesomeIcon.Book))
            {
                this.Logic.ActiveTab = this.Logic.ActiveTab == GuideViewerLogic.SelectedTabState.Guide ? GuideViewerLogic.SelectedTabState.Note : GuideViewerLogic.SelectedTabState.Guide;
            }
            SiGui.AddTooltip(this.Logic.ActiveTab == GuideViewerLogic.SelectedTabState.Guide ? Strings.UserInterface_GuideViewer_Tooltip_ShowNote : Strings.UserInterface_GuideViewer_Tooltip_ShowGuide);

            ImGui.EndGroup();
            ImGui.Separator();
        }
    }
}
