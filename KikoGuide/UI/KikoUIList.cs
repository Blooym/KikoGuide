namespace KikoGuide.UI;

using System;
using System.Numerics;
using System.Linq;

using ImGuiNET;
using CheapLoc;

using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.Utils;
using KikoGuide.Managers;
using KikoGuide.UI.Components;

internal class KikoUIList : IDisposable
{

    // Instance of the global configuration.
    private protected Configuration _configuration;
    private protected string searchText = "";


    // <summary>
    // Instantiates a new KikoUIList.
    // </summary>
    public KikoUIList(Configuration configuration)
    {
        this._configuration = configuration;
    }


    // <summary>
    // Disposes of the KikoUIList and any resources it uses.
    // </summary>
    public void Dispose()
    { }


    // <summary>
    // Draws all UI elements.
    // </summary>
    public void Draw() => DrawListWindow();


    // <summary>
    // Draws the list window.
    // </summary>
    private void DrawListWindow()
    {

        if (!KikoUIState.listVisible) return;

        ImGui.SetNextWindowSizeConstraints(new Vector2(390, 280), new Vector2(600, 600));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.List.Title", "{0} - Duty Finder"), PStrings.PluginName), ref KikoUIState.listVisible))
        {

            var duties = DutyManager.GetDuties();


            if (duties.Count == 0) ImGui.TextColored(Colours.Error, Loc.Localize("UI.List.NoDutyFiles", "No duty files detected! Please try Settings -> Update Resources."));


            var supportButtonText = Loc.Localize("UI.List.SupportButton", "Support (GitHub)");
            var supportButtonShown = _configuration.supportButtonShown;

            // Create donate button 
            if (supportButtonShown) ImGui.SetNextItemWidth(-(ImGui.CalcTextSize(supportButtonText).X + ImGui.GetStyle().FramePadding.X * 2 + ImGui.GetStyle().ItemSpacing.X));
            else ImGui.SetNextItemWidth(-1);

            ImGui.InputTextWithHint("", Loc.Localize("UI.List.Search", "Search"), ref this.searchText, 60);

            if (supportButtonShown)
            {
                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0xfa9898);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0xe76262);
                if (ImGui.Button(supportButtonText)) FS.OpenBrowser("https://github.com/sponsors/BitsOfAByte");
                ImGui.PopStyleColor(2);
            }

            // For each duty type enum, create a tab for it.
            ImGui.BeginTabBar("DutyTypes", ImGuiTabBarFlags.Reorderable);
            foreach (var dutyType in Enum.GetValues(typeof(DutyType)).Cast<int>().ToList())
            {
                if (ImGui.BeginTabItem(Enum.GetName(typeof(DutyType), dutyType)))
                {
                    ImGui.BeginChild(dutyType.ToString(), new Vector2(0, 0), true);

                    // If there are no duties for this duty type, don't draw anything 
                    if (duties.Where(x => x.Type == (int)dutyType).Count() == 0)
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
                    foreach (var duty in duties.Where(x => x.Type == dutyType).ToList())
                    {
                        // If there is a search query, skip the duty if it doesn't match.
                        if (!DutyManager.IsDutyUnlocked(duty)) continue;
                        if (!duty.Name.ToLower().Contains(this.searchText.ToLower())) continue;

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
                        else if (duty.UpdateRequired)
                        {
                            ImGui.TextDisabled(name);
                            Badges.Questionmark(Loc.Localize("UI.List.UpdateRequired", "Cannot display duty as it is not supported on this version."));
                            continue;
                        }

                        // Create a selectable text for the duty name that will open the duty info page.
                        if (ImGui.Selectable(name, false, ImGuiSelectableFlags.AllowDoubleClick))
                        {
                            KikoUIState.SelectedDuty = duty;
                            KikoUIState.dutyInfoVisible = true;
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
