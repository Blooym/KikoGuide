using System.Numerics;
using ImGuiNET;

namespace KikoGuide.UI.ImGuiBasicComponents
{
    /// <summary>
    ///     Components and constants for colours.
    /// </summary>
    internal static class Colours
    {
        /// <summary>
        /// Generic colours
        /// </summary>
        internal static Vector4 Red = new(1.0f, 0.0f, 0.0f, 1.0f);
        internal static Vector4 Green = new(0.0f, 0.5f, 0.0f, 1.0f);
        internal static Vector4 Grey = new(0.5f, 0.5f, 0.5f, 1.0f);

        /// <summary>
        /// State colours
        /// </summary>
        internal static Vector4 Success = new(0.0f, 1.0f, 0.0f, 1.0f);
        internal static Vector4 Warning = new(1.0f, 0.5f, 0.0f, 1.0f);
        internal static Vector4 Error = new(1.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// Misc
        /// </summary>
        internal static Vector4 GuideDisabled = new(0.5f, 0.25f, 0.0f, 1.0f);

        /// <summary>
        ///     Creates a ImGui.TextWrapped() with the given text and colour.
        /// </summary>
        /// <param name="colour"> The colour to show the text in. </param>
        /// <param name="text"> The text to show. </param>
        internal static void TextWrappedColoured(Vector4 colour, string text)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, colour);
            ImGui.TextWrapped(text);
            ImGui.PopStyleColor();
        }

        /// <summary>
        ///     Creates a <see cref="Common.TextWrappedUnformatted(string)" /> with the given text and colour.
        /// </summary>
        /// <param name="colour"> The colour to show the text in. </param>
        /// <param name="text"> The text to show. </param>
        internal static void TextWrappedUnformattedColoured(Vector4 colour, string text)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, colour);
            Common.TextWrappedUnformatted(text);
            ImGui.PopStyleColor();
        }
    }
}
