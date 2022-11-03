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
        /// <param name="colour"> The colour of the badge. </param>
        /// <param name="tag"> The text to show on the badge. </param>
        /// <param name="tooltip"> The tooltip to show on hover if set. </param>
        public static void Custom(Vector4 colour, string tag, string? tooltip = null)
        {
            ImGui.SameLine();
            ImGui.TextColored(colour, tag);
            if (tooltip != null) Common.AddTooltip(tooltip);
        }

        /// <summary> 
        ///     Draws a [?] box, hovering will show a tooltip with the given text.
        /// </summary>
        /// <param name="tooltip"> The tooltip to show on hover. </param>
        public static void Questionmark(string tooltip)
        {
            ImGui.SameLine();
            ImGui.TextColored(Colours.Grey, "[?]");
            Common.AddTooltip(tooltip);
        }
    }
}