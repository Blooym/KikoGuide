using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using KikoGuide.Attributes;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.GuideListTable;

namespace KikoGuide.UI.Windows.GuideList
{
    public sealed class GuideListWindow : Window, IDisposable
    {
        internal GuideListPresenter Presenter;
        public GuideListWindow() : base(TWindowNames.GuideList)
        {
            this.Flags |= ImGuiWindowFlags.NoScrollbar;
            this.Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(360, 300),
                MaximumSize = new Vector2(1000, 1000)
            };
            this.SizeCondition = ImGuiCond.FirstUseEver;

            this.Presenter = new GuideListPresenter();
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

            // If the plugin detects no gudies, show a warning message.
            var guides = GuideListPresenter.GetGuides();
            if (guides.Count == 0)
            {
                Colours.TextWrappedColoured(Colours.Error, TGuideListTable.NoGuidesFilesDetected);
            }

            // If the support button is shown, make the search bar accommodate it, otherwise make it full width.
            var supportButtonShown = GuideListPresenter.GetConfiguration().Display.DonateButtonShown;
            if (supportButtonShown)
            {
                ImGui.SetNextItemWidth(-(ImGui.CalcTextSize(TGenerics.Donate).X + (ImGui.GetStyle().FramePadding.X * 2) + ImGui.GetStyle().ItemSpacing.X));
            }
            else
            {
                ImGui.SetNextItemWidth(-1);
            }

            ImGui.InputTextWithHint("", TGenerics.Search, ref this.searchText, 60);

            // If support button shown, add the button next to the search bar.
            if (supportButtonShown)
            {
                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0xfa9898);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0xe76262);
                if (ImGui.Button(TGenerics.Donate))
                {
                    Util.OpenLink(PluginConstants.DonateButtonUrl);
                }

                ImGui.PopStyleColor(2);
            }

            // For each duty type enum, create a tab for it.
            ImGui.BeginTabBar("##DutyListTabBar");
            foreach (var dutyType in Enum.GetValues(typeof(DutyType)).Cast<int>().ToList())
            {
                if (ImGui.BeginTabItem(((DutyType)dutyType).GetNameAttribute()))
                {
                    ImGui.BeginChild(dutyType.ToString());

                    GuideListTableComponent.Draw(guides, (guide) => GuideListPresenter.OnGuideListSelection(guide), this.searchText, (DutyType)dutyType);

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
            }
        }
    }
}
