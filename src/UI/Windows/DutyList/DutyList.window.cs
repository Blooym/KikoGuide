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
    using Dalamud.Interface.Windowing;
    using Dalamud.Utility;
    using KikoGuide.UI.ImGuiBasicComponents;
    using KikoGuide.UI.ImGuiFullComponents.DutyList;

    sealed public class DutyListWindow : Window, IDisposable
    {
        public DutyListPresenter _presenter;
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

            _presenter = new DutyListPresenter();
        }
        public void Dispose() => this._presenter.Dispose();

        /// <summary>
        ///     The current search query.
        /// </summary>
        private string _searchText = "";

        public override void Draw()
        {
            // Prevent the plugin from crashing when using Window docking.
            // FIXME: Find why this happens in the first place and fix it.
            if (ImGui.GetWindowSize().X < 100 || ImGui.GetWindowSize().Y < 100) return;

            // If the plugin detects no duties, show a warning message.
            var duties = this._presenter.GetDuties();
            if (duties.Count == 0) Colours.TextWrappedColoured(Colours.Error, TStrings.DutyFinderContentNotFound);

            // If the support button is shown, make the search bar accommodate it, otherwise make it full width.
            var supportButtonShown = this._presenter.GetConfiguration().Display.DonateButtonShown;
            if (supportButtonShown) ImGui.SetNextItemWidth(-(ImGui.CalcTextSize(TStrings.Donate).X + ImGui.GetStyle().FramePadding.X * 2 + ImGui.GetStyle().ItemSpacing.X));
            else ImGui.SetNextItemWidth(-1);
            ImGui.InputTextWithHint("", TStrings.Search, ref this._searchText, 60);

            // If support button shown, add the button next to the search bar.
            if (supportButtonShown)
            {
                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0xfa9898);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0xe76262);
                if (ImGui.Button(TStrings.Donate)) Util.OpenLink(PluginConstants.donateButtonUrl);
                ImGui.PopStyleColor(2);
            }

            // For each duty type enum, create a tab for it.
            ImGui.BeginTabBar("##DutyListTabBar");
            foreach (var dutyType in Enum.GetValues(typeof(DutyType)).Cast<int>().ToList())
            {
                if (ImGui.BeginTabItem(LoCExtensions.GetLocalizedName((DutyType)dutyType)))
                {
                    ImGui.BeginChild(dutyType.ToString());

                    DutyListComponent.Draw(duties, ((duty) =>
                    {
                        this._presenter.OnDutyListSelection(duty);
                    }), this._searchText, (DutyType)dutyType);

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
            }
        }
    }
}