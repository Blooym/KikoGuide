using System.Numerics;
using ImGuiNET;

namespace KikoGuide.UI.ImGuiBasicComponents
{
    /// <summary>
    ///     A collection of badge components.
    /// </summary>
    internal static class Badges
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
            if (tooltip != null)
            {
                Common.AddTooltip(tooltip);
            }
        }
    }
}
