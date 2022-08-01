namespace KikoGuide.UI.DutyList;

using System;
using System.Numerics;
using System.Linq;
using ImGuiNET;
using CheapLoc;
using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.Managers;
using KikoGuide.UI.Components;

internal class ListScreen : IDisposable
{
    private string _searchText = "";
    public DutyListPresenter presenter = new DutyListPresenter();


    /// <summary>
    ///     Disposes of the List UI window and any resources it uses.
    /// </summary>
    public void Dispose() { }


    /// <summary>
    ///      Draws all UI elements associated with the List UI.
    /// </summary>
    public void Draw() => DrawListWindow();


    /// <summary>
    ///      Draws the list window.
    /// </summary>
    private void DrawListWindow()
    {

        if (!presenter.isVisible) return;

        ImGui.SetNextWindowSizeConstraints(new Vector2(350, 280), new Vector2(1000, 1000));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.List.Title", "{0} - Duty Finder"), PStrings.pluginName), ref presenter.isVisible))
        {

            // Prevent the plugin from crashing (on table draw, due to fixed width) 
            // when the user turns on Window docking and tries to size the window too small.
            // If there a better solution, please implement it.
            if (ImGui.GetWindowSize().X < 100 || ImGui.GetWindowSize().Y < 100) return;

            var duties = DutyManager.GetDuties();

            if (duties.Count == 0) ImGui.TextColored(Colours.Error, Loc.Localize("UI.List.NoDutyFiles", "No duty files detected! Please try Settings -> Update Resources."));

            var supportButtonText = Loc.Localize("UI.List.SupportButton", "Support (GitHub)");
            var supportButtonShown = Service.Configuration.supportButtonShown;

            // Create support button 
            if (supportButtonShown) ImGui.SetNextItemWidth(-(ImGui.CalcTextSize(supportButtonText).X + ImGui.GetStyle().FramePadding.X * 2 + ImGui.GetStyle().ItemSpacing.X));
            else ImGui.SetNextItemWidth(-1);

            ImGui.InputTextWithHint("", Loc.Localize("UI.List.Search", "Search"), ref this._searchText, 60);

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
                    ImGui.BeginChild(dutyType.ToString(), new Vector2(0, 0), true);

                    var dutyList = duties.Where(duty => duty.Type == dutyType).ToList();

                    // If there are no duties for this duty type, don't draw anything 
                    if (dutyList.Count() == 0)
                    {
                        ImGui.TextDisabled(Loc.Localize("UI.List.NoDutysForType", "No duties for this type detected."));
                        ImGui.EndChild();
                        ImGui.EndTabItem();
                        continue;
                    }

                    // Create a table for each duty, containing its level and name.
                    ImGui.BeginTable("DutyList", 2);
                    ImGui.TableSetupColumn(Loc.Localize("UI.List.Level", "Level"), ImGuiTableColumnFlags.WidthFixed, 45);
                    ImGui.TableSetupColumn(Loc.Localize("UI.List.Duty", "Duty"));
                    ImGui.TableHeadersRow();

                    // Fetch all duties for this duty type and draw them.
                    foreach (var duty in dutyList)
                    {
                        // If there is a search query, skip the duty if it doesn't match.
                        if (!duty.IsUnlocked()) continue;
                        if (!duty.Name.ToLower().Contains(this._searchText.ToLower())) continue;

                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();

                        // Add the duty level to the list
                        ImGui.Text(duty.Level.ToString());
                        ImGui.TableNextColumn();

                        // Set the duty name to be just the duty name, or the duty name and difficulty if its not normal difficulty.
                        var name = duty.Name;
                        if (duty.Difficulty != (int)DutyDifficulty.Normal) name = $"{name} ({Enum.GetName(typeof(DutyDifficulty), duty.Difficulty)})";

                        // if a duty has no boss data, draw the duty name as red as there is nothing to show.
                        if (duty.Bosses == null || duty.Bosses.Count == 0)
                        {
                            ImGui.TextColored(Colours.Red, name);
                            Badges.Questionmark(String.Format(Loc.Localize("UI.List.NoBossData", "No guide available for {0}."), name));
                            continue;
                        }

                        // If this duty requires an update to view, draw it as blue.
                        else if (!duty.IsSupported())
                        {
                            ImGui.TextDisabled(name);
                            Badges.Questionmark(Loc.Localize("UI.List.UpdateRequired", "Cannot display duty as it is not supported on this version."));
                            continue;
                        }

                        // Create a selectable text for the duty name that will open the duty info page.
                        if (ImGui.Selectable(name, false, ImGuiSelectableFlags.AllowDoubleClick))
                        {
                            KikoPlugin.dutyInfoScreen.presenter.selectedDuty = duty;
                            KikoPlugin.dutyInfoScreen.presenter.isVisible = true;
                        }

                        if (duty == DutyManager.GetPlayerDuty()) Badges.Custom(Colours.Green, Loc.Localize("UI.List.InDuty", "In Duty"));
                    }

                    ImGui.EndTable();
                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
            }
            ImGui.End();
        }
    }
}
