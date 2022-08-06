namespace KikoGuide.UI.Screens.DutyList;

using System;
using System.Numerics;
using System.Linq;
using ImGuiNET;
using CheapLoc;
using KikoGuide.Base;
using KikoGuide.Types;
using KikoGuide.Managers;
using KikoGuide.Interfaces;
using Components.Duty;
using KikoGuide.UI.Components;

sealed public class DutyListScreen : IScreen
{
    public DutyListPresenter presenter = new DutyListPresenter();

    public void Draw() => DrawListWindow();
    public void Dispose() => this.presenter.Dispose();
    public void Show() => this.presenter.isVisible = true;
    public void Hide() => this.presenter.isVisible = false;

    /// <summary> The current input search text. </summary>
    private string _searchText = "";

    /// <summary> Draws the list window. </summary>
    private void DrawListWindow()
    {
        if (!presenter.isVisible) return;

        ImGui.SetNextWindowSizeConstraints(new Vector2(350, 280), new Vector2(1000, 1000));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.Screens.DutyList.Title", "{0} - Duty Finder"), PluginStrings.pluginName), ref presenter.isVisible))
        {

            // Prevent the plugin from crashing when using Window docking.
            if (ImGui.GetWindowSize().X < 100 || ImGui.GetWindowSize().Y < 100) return;

            // If the plugin detects no duties, show a warning message.
            var duties = DutyManager.GetDuties();
            if (duties.Count == 0) Colours.TextWrappedColoured(Colours.Error, Loc.Localize("UI.Screens.DutyList.NoFiles", "No duty files detected! Please try Settings -> Update Resources."));

            // Support button
            var supportButtonText = Loc.Localize("Generics.Support", "Support (GitHub)");
            var supportButtonShown = PluginService.Configuration.supportButtonShown;
            if (supportButtonShown) ImGui.SetNextItemWidth(-(ImGui.CalcTextSize(supportButtonText).X + ImGui.GetStyle().FramePadding.X * 2 + ImGui.GetStyle().ItemSpacing.X));
            else ImGui.SetNextItemWidth(-1);
            ImGui.InputTextWithHint("", Loc.Localize("Generics.Search", "Search"), ref this._searchText, 60);
            if (supportButtonShown)
            {
                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0xfa9898);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0xe76262);
                if (ImGui.Button(supportButtonText)) Utils.Common.OpenBrowser("https://github.com/sponsors/BitsOfAByte");
                ImGui.PopStyleColor(2);
            }

            // For each duty type enum, create a tab for it.
            ImGui.BeginTabBar("DutyTypes", ImGuiTabBarFlags.Reorderable);
            foreach (var dutyType in Enum.GetValues(typeof(DutyType)).Cast<int>().ToList())
            {
                if (ImGui.BeginTabItem(Enum.GetName(typeof(DutyType), dutyType)))
                {
                    ImGui.BeginChild(dutyType.ToString());

                    DutyListComponent.Draw(duties, ((duty) =>
                    {
                        PluginService.WindowManager.DutyInfo.presenter.selectedDuty = duty;
                        PluginService.WindowManager.DutyInfo.Show();
                    }), this._searchText, dutyType);

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
            }
            ImGui.End();
        }
    }
}
