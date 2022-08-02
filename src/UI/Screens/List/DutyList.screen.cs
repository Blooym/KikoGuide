namespace KikoGuide.UI.Screens.DutyList;

using System;
using System.Numerics;
using System.Linq;
using ImGuiNET;
using CheapLoc;
using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.Managers;
using KikoGuide.UI.Components;

sealed class ListScreen : IDisposable
{
    public DutyListPresenter presenter = new DutyListPresenter();

    /// <summary> Disposes of the DutyList UI window and any resources it uses. </summary>
    public void Dispose() => this.presenter.Dispose();

    /// <summary> Draws all UI elements associated with the List UI. </summary>
    public void Draw() => DrawListWindow();

    /// <summary> The current input search text. </summary>
    private string _searchText = "";

    /// <summary> Draws the list window. </summary>
    private void DrawListWindow()
    {
        if (!presenter.isVisible) return;

        ImGui.SetNextWindowSizeConstraints(new Vector2(350, 280), new Vector2(1000, 1000));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.List.Title", "{0} - Duty Finder"), PStrings.pluginName), ref presenter.isVisible))
        {

            // Prevent the plugin from crashing when using Window docking.
            if (ImGui.GetWindowSize().X < 100 || ImGui.GetWindowSize().Y < 100) return;

            // If the plugin detects no duties, show a warning message.
            var duties = DutyManager.GetDuties();
            if (duties.Count == 0) Colours.TextWrappedColoured(Colours.Error, Loc.Localize("UI.Screens.DutyList.NoFiles", "No duty files detected! Please try Settings -> Update Resources."));

            // Support button
            var supportButtonText = Loc.Localize("Generics.Support", "Support (GitHub)");
            var supportButtonShown = Service.Configuration.supportButtonShown;
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

                    Components.Duty.DutyListComponent.Draw(duties, ((duty) =>
                    {
                        KikoPlugin.dutyInfoScreen.presenter.selectedDuty = duty;
                        KikoPlugin.dutyInfoScreen.presenter.isVisible = true;
                    }), this._searchText, dutyType);

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
            }
            ImGui.End();
        }
    }
}
