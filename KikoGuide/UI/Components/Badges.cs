namespace KikoGuide.UI.Components;

using System.Numerics;
using ImGuiNET;
using CheapLoc;

static class Badges
{
    /// <summary> Draws a badge labelling the item as "NEW" on the same line. </summary>
    public static void New()
    {
        ImGui.SameLine();
        ImGui.TextColored(Colours.Blue, Loc.Localize("UI.Tags.New", "NEW"));
        Components.Tooltips.AddTooltip(Loc.Localize("Components.Tags.New.Tooltip", "New addition!"));
    }


    /// <summary> Draws a custom badge with the given colour, text and optional tooltip on the same line. </summary>
    public static void Custom(Vector4 colour, string tagText, string? tooltipText = null)
    {
        ImGui.SameLine();
        ImGui.TextColored(colour, tagText);
        if (tooltipText != null) Components.Tooltips.AddTooltip(tooltipText);
    }


    /// <summary> Draws a ? that when hovering will show the given text as a tooltip on the same line. </summary>
    public static void Questionmark(string text)
    {
        ImGui.SameLine();
        ImGui.TextColored(Colours.Grey, "[?]");
        Components.Tooltips.AddTooltip(text);
    }
}