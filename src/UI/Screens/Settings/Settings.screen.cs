namespace KikoGuide.UI.Screens.Settings;

using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using ImGuiNET;
using CheapLoc;
using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.Managers;
using KikoGuide.UI.Components;

sealed class SettingsScreen : IDisposable
{
    public SettingsPresenter presenter = new SettingsPresenter();

    /// <summary> Disposes of the settings screen and any resources it uses. </summary>
    public void Dispose() => this.presenter.Dispose();

    /// <summary> Draws all UI elements associated with the settings screen. </summary>
    public void Draw() => DrawSettingsWindow();

    /// <summary> Draws the settings window. </summary>
    private void DrawSettingsWindow()
    {

        if (!presenter.isVisible) return;

        List<int> disabledMechanics = Service.Configuration.hiddenMechanics;
        bool autoOpenDuty = Service.Configuration.autoOpenDuty;
        bool shortenStrategies = Service.Configuration.shortenStrategies;
        bool supportButtonShown = Service.Configuration.supportButtonShown;
        long lastUpdateTime = Service.Configuration.lastResourceUpdate;

        ImGui.SetNextWindowSizeConstraints(new Vector2(410, 250), new Vector2(1000, 1000));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.Settings.Title", "{0} - Settings"), PStrings.pluginName), ref presenter.isVisible))
        {
            // Create tab bar for each settings category
            ImGui.BeginTabBar("settings");

            // General settings go in here.
            if (ImGui.BeginTabItem(Loc.Localize("UI.Settings.TabItem.General", "General")))
            {
                // Auto-open duty setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Settings.AutoOpenDuty", "Open in Duty"), ref autoOpenDuty, () =>
               {
                   Service.Configuration.autoOpenDuty = !autoOpenDuty;
                   Service.Configuration.Save();
               });
                Tooltips.AddTooltip(Loc.Localize("UI.Settings.AutoOpenDuty.Tooltip", "Open the duty guide when entering a duty."));


                // TLDR mode setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Settings.ShortenStrategies", "Shorten Strategies"), ref shortenStrategies, () =>
                {
                    Service.Configuration.shortenStrategies = !shortenStrategies;
                    Service.Configuration.Save();
                });

                Tooltips.AddTooltip(Loc.Localize("UI.Settings.ShortenStrategies.Tooltip", "Shorten duty guide strategies if possible."));


                // Support button setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Settings.ShowSupportButton", "Show Support Button"), ref supportButtonShown, () =>
                {
                    Service.Configuration.supportButtonShown = !supportButtonShown;
                    Service.Configuration.Save();
                });
                Tooltips.AddTooltip(Loc.Localize("UI.Settings.ShowSupportButton.Tooltip", "Show the support Kiko Guide button."));


                // Update resources / localizable button.
                ImGui.Dummy(new Vector2(0, 5));
                Common.TextHeading(Loc.Localize("UI.Settings.UpdateResources.Title", "Resources & Localization"));
                ImGui.TextWrapped(Loc.Localize("UI.Settings.UpdateResources.Text", "Downloads the latest resources, localizations & guides."));
                ImGui.Dummy(new Vector2(0, 5));
                ImGui.BeginDisabled(UpdateManager.updateInProgress);
                if (ImGui.Button(Loc.Localize("UI.Settings.UpdateResources", "Update Resources"))) UpdateManager.UpdateResources();
                ImGui.EndDisabled();

                if (!UpdateManager.updateInProgress && UpdateManager.lastUpdateSuccess == false && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(Loc.Localize("UI.Settings.UpdateLocalization.Failed", "Update Failed."));
                }
                else if (!UpdateManager.updateInProgress && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(String.Format(Loc.Localize("UI.Settings.UpdateLocalization.UpdatedAt", "Last Update: {0}"),
                    DateTimeOffset.FromUnixTimeMilliseconds(lastUpdateTime).ToString("hh:mm tt")));
                }

#if DEBUG
                this.presenter.dialogManager.Draw();
                if (ImGui.Button("Export Localizable")) this.presenter.dialogManager.OpenFolderDialog("Select Export Directory", this.presenter.OnDirectoryPicked);
#endif

                ImGui.EndTabItem();
            }


            // Mechanics settings go in here. 
            if (ImGui.BeginTabItem(Loc.Localize("UI.Settings.TabItem.Mechanics", "Mechanics")))
            {
                // Create a child since we're using columns.
                ImGui.BeginChild("mechanics", new Vector2(0, 0), false);
                ImGui.Columns(2, "mechanics", false);

                // For each mechanic enum, creating a checkbox for it.
                foreach (var mechanic in Enum.GetValues(typeof(Mechanics)).Cast<int>().ToList())
                {
                    // See if the mechanic is enabled by looking at the list for the enum value.
                    var isMechanicDisabled = disabledMechanics.Contains(mechanic);

                    // Create a checkbox for the mechanic.
                    Common.ToggleCheckbox(String.Format(Loc.Localize("UI.Settings.HideMechanic", "Hide {0}"), Enum.GetName(typeof(Mechanics), mechanic)), ref isMechanicDisabled, () =>
                    {
                        switch (isMechanicDisabled)
                        {
                            case false:
                                Service.Configuration.hiddenMechanics.Add(mechanic);
                                break;
                            case true:
                                Service.Configuration.hiddenMechanics.Remove(mechanic);
                                break;
                        }
                        Service.Configuration.Save();
                    });

                    Tooltips.AddTooltip(String.Format(Loc.Localize("UI.Settings.HideMechanicTooltip", "Hide {0} from the duty guide."), Enum.GetName(typeof(Mechanics), mechanic)));

                    ImGui.NextColumn();
                }

                ImGui.EndChild();
                ImGui.EndTabItem();
            }
        }
    }
}