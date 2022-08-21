namespace KikoGuide.UI.Components;

using ImGuiNET;

/// <summary>
///     A collection of tooltip components.
/// </summary>
static class Tooltips
{
    /// <summary> Adds a tooltip on hover to the last item. </summary>
    public static void AddTooltip(string text)
    {
        if (ImGui.IsItemHovered()) ImGui.SetTooltip(text);
    }
}