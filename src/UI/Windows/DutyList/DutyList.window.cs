namespace KikoGuide.UI.Windows.DutyList
{
    using System;
    using System.Numerics;
    using System.Linq;
    using ImGuiNET;
    using KikoGuide.Base;
    using KikoGuide.Localization;
    using KikoGuide.Types;
    using KikoGuide.Managers;
    using KikoGuide.UI.Windows.DutyInfo;
    using Dalamud.Interface.Windowing;
    using Dalamud.Utility;
    using KikoGuide.UI.ImGuiBasicComponents;
    using KikoGuide.UI.ImGuiFullComponents.DutyList;

    sealed public class DutyListWindow : Window, IDisposable
    {
        public DutyListPresenter presenter = new DutyListPresenter();

        public DutyListWindow() : base(WindowManager.DutyListWindowName)
        {
            Flags |= ImGuiWindowFlags.NoScrollbar;
            Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(360, 300),
                MaximumSize = new Vector2(1000, 1000)
            };

            SizeCondition = ImGuiCond.FirstUseEver;
        }

        public void Dispose() => this.presenter.Dispose();

        /// <summary>
        ///     The current input search text.
        /// </summary>
        private string _searchText = "";

        /// <summary>
        ///     Draws the list window.
        /// </summary>
        public override void Draw()
        {
            // Prevent the plugin from crashing when using Window docking.
            if (ImGui.GetWindowSize().X < 100 || ImGui.GetWindowSize().Y < 100) return;

            // If the plugin detects no duties, show a warning message.
            var duties = DutyManager.GetDuties();
            if (duties.Count == 0) Colours.TextWrappedColoured(Colours.Error, TStrings.DutyFinderContentNotFound);

            // If the support button is shown, make the search bar accommodate it, otherwise make it full width.
            if (PluginService.Configuration.Display.SupportButtonShown) ImGui.SetNextItemWidth(-(ImGui.CalcTextSize(TStrings.Support).X + ImGui.GetStyle().FramePadding.X * 2 + ImGui.GetStyle().ItemSpacing.X));
            else ImGui.SetNextItemWidth(-1);
            ImGui.InputTextWithHint("", TStrings.Search, ref this._searchText, 60);

            // If support button shown, add the button next to the search bar.
            if (PluginService.Configuration.Display.SupportButtonShown)
            {
                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0xfa9898);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0xe76262);
                if (ImGui.Button(TStrings.Support)) Util.OpenLink(PluginConstants.supportButtonUrl);
                ImGui.PopStyleColor(2);
            }

            // For each duty type enum, create a tab for it.
            ImGui.BeginTabBar("DutyTypes");
            foreach (var dutyType in Enum.GetValues(typeof(DutyType)).Cast<int>().ToList())
            {
                if (ImGui.BeginTabItem(LoCExtensions.GetLocalizedName((DutyType)dutyType)))
                {
                    ImGui.BeginChild(dutyType.ToString());

                    DutyListComponent.Draw(duties, ((duty) =>
                    {
                        if (PluginService.WindowManager.windowSystem.GetWindow(WindowManager.DutyInfoWindowName) is DutyInfoWindow dutyInfoWindow)
                        {
                            dutyInfoWindow.IsOpen = true;
                            dutyInfoWindow.presenter.selectedDuty = duty;
                        }
                    }), this._searchText, dutyType);

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
            }
        }
    }
}