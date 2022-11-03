namespace KikoGuide.UI.ImGuiFullComponents.DutyInfo
{
    using System;
    using ImGuiNET;
    using KikoGuide.Types;
    using KikoGuide.UI.ImGuiBasicComponents;

    /// <summary>
    ///     A component for displaying duty information.
    /// </summary>
    static class DutyInfoComponent
    {
        private static DutyInfoPresenter _presenter = new DutyInfoPresenter();

        /// <summary> 
        ///     Draws the duty info window.
        /// </summary>
        /// <param name="duty"> The duty to draw information for. </param>
        public static void Draw(Duty duty)
        {
            try
            {
                ImGui.TextWrapped("To be implemented :)");
            }
            catch (Exception e) { ImGui.TextColored(Colours.Error, $"Component Exception: {e.Message}"); }
        }
    }
}