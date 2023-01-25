using Dalamud.Interface.Colors;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using KikoGuide.GuideHandling;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Layouts
{
    internal static class DutyGuideLayout
    {
        private static readonly float GuideHeaderHeight = ImGui.GetContentRegionAvail().Y * 0.2f;
        private static readonly float GuideContentHeight = ImGui.GetContentRegionAvail().Y * 0.5f;
        private static readonly float GuideNotesHeight = ImGui.GetContentRegionAvail().Y * 0.3f;

        public static void Draw(GuideBase guide)
        {
            // Refuse to draw if the guide has no linked duty.
            if (guide.Duty is null)
            {
                SiGui.TextWrappedColoured(ImGuiColors.DalamudRed, "This guide has no linked duty and cannot be used with the DutyGuide layout, please override the draw method with a custom layout.");
                return;
            }

            if (ImGui.BeginChild("GuideHeader", new Vector2(0, GuideHeaderHeight)))
            {
                SiGui.TextHeading(guide.Name);
                ImGui.EndChild();
            }

            if (ImGui.BeginChild("GuideContent", new Vector2(0, GuideContentHeight)))
            {
                ImGui.TextWrapped("Guide content goes here.");
                ImGui.EndChild();
            }

            if (ImGui.BeginChild("GuideNotes", new Vector2(0, GuideNotesHeight)))
            {
                ImGui.TextWrapped("Guide notes go here.");
                ImGui.EndChild();
            }
        }
    }
}