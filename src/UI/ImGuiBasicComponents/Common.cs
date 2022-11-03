namespace KikoGuide.UI.ImGuiBasicComponents
{
    using System;
    using ImGuiNET;

    /// <summary>
    ///     A collection of common reusable components.
    /// </summary>
    static class Common
    {
        /// <summary>
        ///     Draws a standard title heading.
        /// </summary>
        public static void TextHeading(string text)
        {
            ImGui.TextDisabled(text);
            ImGui.Separator();
        }

        /// <summary>
        ///     Draws a checkbox with an onPress event when interacted with.
        /// </summary>
        /// <param name="label"> The label to show next to the checkbox. </param>
        /// <param name="value"> The reference to the value to change. </param>
        /// <param name="onPress"> The event to trigger when the checkbox is interacted with. </param>
        public static void ToggleCheckbox(string label, ref bool value, Action? onPress = null)
        {
            if (ImGui.Checkbox(label, ref value))
            {
                value = !value;
                if (onPress != null) onPress();
            }
        }

        /// <summary>
        ///     Shows a tooltip when hovering over the last item.
        /// </summary>
        public static void AddTooltip(string text)
        {
            if (ImGui.IsItemHovered()) ImGui.SetTooltip(text);
        }
    }
}