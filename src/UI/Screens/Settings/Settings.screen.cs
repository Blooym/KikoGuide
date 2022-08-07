namespace KikoGuide.UI.Screens.Settings;

using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using ImGuiNET;
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
        if (ImGui.Begin(TStrings.SettingsTitle(), ref presenter.isVisible))
        {
            // Create tab bar for each settings category
            ImGui.BeginTabBar("##Settings");

            // General settings go in here.
            if (ImGui.BeginTabItem(TStrings.SettingsGeneral()))
            {
                // Auto-open duty setting.
                Common.ToggleCheckbox(TStrings.SettingsAutoOpenInDuty(), ref autoOpenDuty, () =>
               {
                   PluginService.Configuration.autoOpenDuty = !autoOpenDuty;
                   PluginService.Configuration.Save();
               });
                Tooltips.AddTooltip(TStrings.SettingsAutoOpenInDutyTooltip());


                // TLDR mode setting.
                Common.ToggleCheckbox(TStrings.SettingsShortMode(), ref shortenStrategies, () =>
                {
                    PluginService.Configuration.shortenStrategies = !shortenStrategies;
                    PluginService.Configuration.Save();
                });
                Tooltips.AddTooltip(TStrings.SettingsShortModeTooltip());


                // Support button setting.
                Common.ToggleCheckbox(TStrings.SettingsShowSupportButton(), ref supportButtonShown, () =>
                {
                    PluginService.Configuration.supportButtonShown = !supportButtonShown;
                    PluginService.Configuration.Save();
                });
                Tooltips.AddTooltip(TStrings.SettingsShowSupportButtonTooltip());


                // Update resources / localizable button.
                ImGui.Dummy(new Vector2(0, 5));
                Common.TextHeading(TStrings.SettingsResourcesAndLocalization());
                ImGui.TextWrapped(TStrings.SettingsResourcesAndLocalizationDesc());
                ImGui.Dummy(new Vector2(0, 5));
                ImGui.BeginDisabled(PluginService.ResourceManager.updateInProgress);
                if (ImGui.Button(TStrings.SettingsUpdateResources())) PluginService.ResourceManager.Update();
                ImGui.EndDisabled();

                if (!PluginService.ResourceManager.updateInProgress && PluginService.ResourceManager.lastUpdateSuccess == false && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(TStrings.SettingsUpdateFailed());
                }
                else if (!PluginService.ResourceManager.updateInProgress && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(TStrings.SettingsLastUpdate(DateTimeOffset.FromUnixTimeMilliseconds(lastUpdateTime).ToLocalTime().ToString()));
                }

#if DEBUG
                this.presenter.dialogManager.Draw();
                if (ImGui.Button("Export Localizable")) this.presenter.dialogManager.OpenFolderDialog("Select Export Directory", this.presenter.OnDirectoryPicked);
#endif

                ImGui.EndTabItem();
            }


            // Mechanics settings go in here. 
            if (ImGui.BeginTabItem(TStrings.SettingsMechanics()))
            {
                // Create a child since we're using columns.
                ImGui.BeginChild("##Mechanics", new Vector2(0, 0), false);
                ImGui.Columns(2, "##Mechanics", false);

                // For each mechanic enum, creating a checkbox for it.
                foreach (var mechanic in Enum.GetValues(typeof(DutyMechanics)).Cast<int>().ToList())
                {
                    // See if the mechanic is enabled by looking at the list for the enum value.
                    var isMechanicDisabled = disabledMechanics.Contains(mechanic);

                    // Create a checkbox for the mechanic.
                    Common.ToggleCheckbox(TStrings.SettingsHideMechanic(Enum.GetName(typeof(DutyMechanics), mechanic)), ref isMechanicDisabled, () =>
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

                    Tooltips.AddTooltip(TStrings.SettingsHideMechanicTooltip(Enum.GetName(typeof(DutyMechanics), mechanic)));

                    ImGui.NextColumn();
                }

                ImGui.EndChild();
                ImGui.EndTabItem();
            }

            // Integrations settings go in here. 
            if (ImGui.BeginTabItem(TStrings.SettingsIntegrations()))
            {
                ImGui.TextWrapped(TStrings.SettingsIntegrationsDesc());
                ImGui.Dummy(new Vector2(0, 10));
                Common.TextHeading(TStrings.SettingsAvailableIntegrations());

                // For each mechanic enum, creating a checkbox for it.
                foreach (var integration in Enum.GetValues(typeof(Managers.IPC.IPCProviders)))
                {
                    var isIntegrationDisabled = PluginService.Configuration.enabledIntegrations.Contains((Managers.IPC.IPCProviders)integration);

                    Common.ToggleCheckbox(integration.ToString() ?? "Integration", ref isIntegrationDisabled, () =>
                    {
                        switch (isIntegrationDisabled)
                        {
                            case false:
                                PluginService.Configuration.enabledIntegrations.Add((Managers.IPC.IPCProviders)integration);
                                break;
                            case true:
                                PluginService.Configuration.enabledIntegrations.Remove((Managers.IPC.IPCProviders)integration);
                                break;
                        }
                        PluginService.Configuration.Save();
                    });
                }

                ImGui.EndTabItem();
            }
        }
    }
}