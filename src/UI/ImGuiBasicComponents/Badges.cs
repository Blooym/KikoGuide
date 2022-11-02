namespace KikoGuide.UI.ImGuiBasicComponents
{
    using System.Numerics;
    using ImGuiNET;

    /// <summary>
    ///     A collection of badge components.
    /// </summary>
    static class Badges
    {
        /// <summary>
        ///     Draws a custom badge with the given colour, text and optional tooltip on the same line.
        /// </summary>
        public static void Custom(Vector4 colour, string tagText, string? tooltipText = null)
        {
            ImGui.SameLine();
            ImGui.TextColored(colour, tagText);
            if (tooltipText != null) Tooltips.AddTooltip(tooltipText);
        }

        /// <summary> Draws a ? that when hovering will show the given text as a tooltip on the same line. </summary>
        public static void Questionmark(string text)
        {
            ImGui.SameLine();
            ImGui.TextColored(Colours.Grey, "[?]");
            Tooltips.AddTooltip(text);
        }
    }
}