namespace KikoGuide.UI.Components;

using System;
using ImGuiNET;

static class Common
{
    /// <summary> Draws a standard title heading </summary>
    public static void TextHeading(string text)
    {
        ImGui.TextDisabled(text);
        ImGui.Separator();
    }

    /// <summary> Draws a checkbox with an onPress event when interacted with. </summary>
    public static void ToggleCheckbox(string label, ref bool value, Action? onPress = null)
    {
        if (ImGui.Checkbox(label, ref value))
        {
            value = !value;
            if (onPress != null) onPress();
        }
    }
}