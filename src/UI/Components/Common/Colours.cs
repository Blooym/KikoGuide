namespace KikoGuide.UI.Components;

using System.Numerics;
using ImGuiNET;

static class Colours
{
    public static Vector4 Error = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    public static Vector4 Warning = new Vector4(1.0f, 0.5f, 0.0f, 1.0f);
    public static Vector4 Success = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);

    public static Vector4 Red = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    public static Vector4 Blue = new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
    public static Vector4 Green = new Vector4(0.0f, 0.5f, 0.0f, 1.0f);
    public static Vector4 Grey = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);

    public static void TextWrappedColoured(Vector4 colour, string text)
    {
        ImGui.PushStyleColor(ImGuiCol.Text, colour);
        ImGui.TextWrapped(text);
        ImGui.PopStyleColor();
    }
}