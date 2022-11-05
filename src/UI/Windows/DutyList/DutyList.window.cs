using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Attributes;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.DutyList;

namespace KikoGuide.UI.Windows.DutyList
{
    public sealed class DutyListWindow : Window, IDisposable
    {
        internal DutyListPresenter Presenter;
        public DutyListWindow() : base(WindowManager.DutyListWindowName)
        {
            this.Flags |= ImGuiWindowFlags.NoScrollbar;
            this.Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(360, 300),
                MaximumSize = new Vector2(1000, 1000)
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;

            this.Presenter = new DutyListPresenter();
        }
        public void Dispose() => this.Presenter.Dispose();

        /// <summary>
        ///     The current search query.
        /// </summary>
        private string searchText = "";

        public override void Draw()
        {
            // Prevent the plugin from crashing when using Window docking.
            // FIXME: Find why this happens in the first place and fix it.
            if (ImGui.GetWindowSize().X < 100 || ImGui.GetWindowSize().Y < 100)
            {
                return;
            }

            // If the plugin detects no duties, show a warning message.
            var duties = DutyListPresenter.GetDuties();
            if (duties.Count == 0)
            {
                Colours.TextWrappedColoured(Colours.Error, TStrings.DutyFinderContentNotFound);
            }

            // If the support button is shown, make the search bar accommodate it, otherwise make it full width.
            var supportButtonShown = DutyListPresenter.GetConfiguration().Display.DonateButtonShown;
            if (supportButtonShown)
            {
                ImGui.SetNextItemWidth(-(ImGui.CalcTextSize(TStrings.Donate).X + (ImGui.GetStyle().FramePadding.X * 2) + ImGui.GetStyle().ItemSpacing.X));
            }
            else
            {
                ImGui.SetNextItemWidth(-1);
            }

            ImGui.InputTextWithHint("", TStrings.Search, ref this.searchText, 60);

            // If support button shown, add the button next to the search bar.
            if (supportButtonShown)
            {
                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0xfa9898);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0xe76262);
                if (ImGui.Button(TStrings.Donate))
                {
                    Util.OpenLink(PluginConstants.DonateButtonUrl);
                }

                ImGui.PopStyleColor(2);
            }

            // For each duty type enum, create a tab for it.
            ImGui.BeginTabBar("##DutyListTabBar");
            foreach (var dutyType in Enum.GetValues(typeof(DutyType)).Cast<int>().ToList())
            {
                if (ImGui.BeginTabItem(AttributeExtensions.GetNameAttribute((DutyType)dutyType)))
                {
                    ImGui.BeginChild(dutyType.ToString());

                    DutyListComponent.Draw(duties, (duty) => DutyListPresenter.OnDutyListSelection(duty), this.searchText, (DutyType)dutyType);

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
            }
        }
    }
}