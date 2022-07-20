namespace KikoGuide.UI.Components;

using ImGuiNET;

static class Tooltips
{
    public static void AddTooltip(string text)
    {
        if (ImGui.IsItemHovered()) ImGui.SetTooltip(text);
    }
}