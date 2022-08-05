namespace KikoGuide.UI.Screens.Settings;

using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using ImGuiNET;
using CheapLoc;
using KikoGuide.Base;
using KikoGuide.Types;
using KikoGuide.UI.Components;
using KikoGuide.Interfaces;

sealed public class SettingsScreen : IScreen
{
    public SettingsPresenter presenter = new SettingsPresenter();

    public void Draw() => DrawSettingsWindow();
    public void Dispose() => this.presenter.Dispose();
    public void Show() => this.presenter.isVisible = true;
    public void Hide() => this.presenter.isVisible = false;

    /// <summary> Draws the settings window. </summary>
    private void DrawSettingsWindow()
    {

        if (!presenter.isVisible) return;

        List<int> disabledMechanics = PluginService.Configuration.hiddenMechanics;
        bool autoOpenDuty = PluginService.Configuration.autoOpenDuty;
        bool shortenStrategies = PluginService.Configuration.shortenStrategies;
        bool supportButtonShown = PluginService.Configuration.supportButtonShown;
        long lastUpdateTime = PluginService.Configuration.lastResourceUpdate;

        ImGui.SetNextWindowSizeConstraints(new Vector2(410, 250), new Vector2(1000, 1000));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.Screens.Settings.Title", "{0} - Settings"), PluginStrings.pluginName), ref presenter.isVisible))
        {
            // Create tab bar for each settings category
            ImGui.BeginTabBar("settings");

            // General settings go in here.
            if (ImGui.BeginTabItem(Loc.Localize("UI.Screens.Settings.TabItem.General", "General")))
            {
                // Auto-open duty setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Screens.Settings.AutoOpenDuty", "Open in Duty"), ref autoOpenDuty, () =>
               {
                   PluginService.Configuration.autoOpenDuty = !autoOpenDuty;
                   PluginService.Configuration.Save();
               });
                Tooltips.AddTooltip(Loc.Localize("UI.Screens.Settings.AutoOpenDuty.Tooltip", "Open the duty guide when entering a duty."));


                // TLDR mode setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Screens.Settings.ShortenStrategies", "Shorten Strategies"), ref shortenStrategies, () =>
                {
                    PluginService.Configuration.shortenStrategies = !shortenStrategies;
                    PluginService.Configuration.Save();
                });

                Tooltips.AddTooltip(Loc.Localize("UI.Screens.Settings.ShortenStrategies.Tooltip", "Shorten duty guide strategies if possible."));


                // Support button setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Screens.Settings.ShowSupportButton", "Show Support Button"), ref supportButtonShown, () =>
                {
                    PluginService.Configuration.supportButtonShown = !supportButtonShown;
                    PluginService.Configuration.Save();
                });
                Tooltips.AddTooltip(Loc.Localize("UI.Screens.Settings.ShowSupportButton.Tooltip", "Show the support Kiko Guide button."));


                // Update resources / localizable button.
                ImGui.Dummy(new Vector2(0, 5));
                Common.TextHeading(Loc.Localize("UI.Screens.Settings.UpdateResources.Title", "Resources & Localization"));
                ImGui.TextWrapped(Loc.Localize("UI.Screens.Settings.UpdateResources.Text", "Downloads the latest resources, localizations & guides."));
                ImGui.Dummy(new Vector2(0, 5));
                ImGui.BeginDisabled(PluginResourceManager.updateInProgress);
                if (ImGui.Button(Loc.Localize("UI.Screens.Settings.UpdateResources", "Update Resources"))) PluginResourceManager.Update();
                ImGui.EndDisabled();

                if (!PluginResourceManager.updateInProgress && PluginResourceManager.lastUpdateSuccess == false && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(Loc.Localize("UI.Screens.Settings.UpdateLocalization.Failed", "Update Failed."));
                }
                else if (!PluginResourceManager.updateInProgress && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(String.Format(Loc.Localize("UI.Screens.Settings.UpdateLocalization.UpdatedAt", "Last Update: {0}"),
                    DateTimeOffset.FromUnixTimeMilliseconds(lastUpdateTime).ToString("hh:mm tt")));
                }

#if DEBUG
                this.presenter.dialogManager.Draw();
                if (ImGui.Button("Export Localizable")) this.presenter.dialogManager.OpenFolderDialog("Select Export Directory", this.presenter.OnDirectoryPicked);
#endif

                ImGui.EndTabItem();
            }


            // Mechanics settings go in here. 
            if (ImGui.BeginTabItem(Loc.Localize("UI.Screens.Settings.TabItem.Mechanics", "Mechanics")))
            {
                // Create a child since we're using columns.
                ImGui.BeginChild("mechanics", new Vector2(0, 0), false);
                ImGui.Columns(2, "mechanics", false);

                // For each mechanic enum, creating a checkbox for it.
                foreach (var mechanic in Enum.GetValues(typeof(DutyMechanics)).Cast<int>().ToList())
                {
                    // See if the mechanic is enabled by looking at the list for the enum value.
                    var isMechanicDisabled = disabledMechanics.Contains(mechanic);

                    // Create a checkbox for the mechanic.
                    Common.ToggleCheckbox(String.Format(Loc.Localize("UI.Screens.Settings.HideMechanic", "Hide {0}"), Enum.GetName(typeof(DutyMechanics), mechanic)), ref isMechanicDisabled, () =>
                    {
                        switch (isMechanicDisabled)
                        {
                            case false:
                                PluginService.Configuration.hiddenMechanics.Add(mechanic);
                                break;
                            case true:
                                PluginService.Configuration.hiddenMechanics.Remove(mechanic);
                                break;
                        }
                        PluginService.Configuration.Save();
                    });

                    Tooltips.AddTooltip(String.Format(Loc.Localize("UI.Screens.Settings.HideMechanicTooltip", "Hide {0} from the duty guide."), Enum.GetName(typeof(DutyMechanics), mechanic)));

                    ImGui.NextColumn();
                }

                ImGui.EndChild();
                ImGui.EndTabItem();
            }
        }
    }
}