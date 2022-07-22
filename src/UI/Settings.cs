namespace KikoGuide.UI;


using System;
using System.Numerics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using CheapLoc;
using Dalamud.Logging;
using Dalamud.Interface.Internal.Notifications;
using KikoGuide.Base;
using KikoGuide.Enums;
using KikoGuide.Managers;
using KikoGuide.UI.Components;

internal class Settings : IDisposable
{
    private protected Configuration _configuration;

    /// <summary>
    //      Instantiates a new settings UI window.
    /// </summary>
    public Settings(Configuration configuration) => this._configuration = configuration;



    /// <summary>
    ///     Disposes of the settings UI window and any resources it uses.
    /// </summary>
    public void Dispose() { }


    /// <summary>
    ///     Draws all UI elements associated with the settings UI.
    /// </summary>
    public void Draw() => DrawSettingsWindow();


    /// <summary>
    ///     Draws the settings window.
    /// </summary>
    private void DrawSettingsWindow()
    {

        if (!UIState.settingsVisible) return;

        List<int> disabledMechanics = this._configuration.hiddenMechanics;
        bool autoOpenDuty = this._configuration.autoOpenDuty;
        bool shortenStrategies = this._configuration.shortenStrategies;
        bool supportButtonShown = this._configuration.supportButtonShown;
        long lastUpdateTime = this._configuration.lastResourceUpdate;

#if DEBUG
        string localizableOutputDir = this._configuration.localizableOutputDir;
#endif

        ImGui.SetNextWindowSizeConstraints(new Vector2(400, 240), new Vector2(400, 240));
        if (ImGui.Begin(String.Format(Loc.Localize("UI.Settings.Title", "{0} - Settings"), PStrings.pluginName), ref UIState.settingsVisible, ImGuiWindowFlags.NoResize))
        {
            // Create tab bar for each settings category
            ImGui.BeginTabBar("settings");

            // General settings go in here.
            if (ImGui.BeginTabItem(Loc.Localize("UI.Settings.TabItem.General", "General")))
            {
                // Auto-open duty setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Settings.AutoOpenDuty", "Open in Duty"), ref autoOpenDuty, () =>
               {
                   autoOpenDuty = !autoOpenDuty;
                   this._configuration.autoOpenDuty = autoOpenDuty;
                   this._configuration.Save();
               });
                Tooltips.AddTooltip(Loc.Localize("UI.Settings.AutoOpenDuty.Tooltip", "Open the duty guide when entering a duty."));


                // TLDR mode setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Settings.ShortenStrategies", "Shorten Strategies"), ref shortenStrategies, () =>
                {
                    shortenStrategies = !shortenStrategies;
                    this._configuration.shortenStrategies = shortenStrategies;
                    this._configuration.Save();
                });

                Tooltips.AddTooltip(Loc.Localize("UI.Settings.ShortenStrategies.Tooltip", "Shorten duty guide strategies if possible."));


                // Support button setting.
                Common.ToggleCheckbox(Loc.Localize("UI.Settings.ShowSupportButton", "Show Support Button"), ref supportButtonShown, () =>
                {
                    supportButtonShown = !supportButtonShown;
                    this._configuration.supportButtonShown = supportButtonShown;
                    this._configuration.Save();
                });
                Tooltips.AddTooltip(Loc.Localize("UI.Settings.ShowSupportButton.Tooltip", "Show the support Kiko Guide button."));


                // Update resources / localizable button.
                Common.TextHeading(Loc.Localize("UI.Settings.UpdateResources.Title", "Resources & Localization"));
                ImGui.TextWrapped(Loc.Localize("UI.Settings.UpdateResources.Text", "Downloads the latest resources, localizations & guides."));
                ImGui.Dummy(new Vector2(0, 5));
                ImGui.BeginDisabled(UpdateManager.updateInProgress);
                if (ImGui.Button(Loc.Localize("UI.Settings.UpdateResources", "Update Resources"))) UpdateManager.UpdateResources();
                ImGui.EndDisabled();

                if (!UpdateManager.updateInProgress && UpdateManager.lastUpdateSuccess == false && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(Loc.Localize("UI.Settings.UpdateLocalization.Failed", "Failed to update."));
                }
                else if (!UpdateManager.updateInProgress && lastUpdateTime != 0)
                {
                    ImGui.SameLine();
                    ImGui.TextWrapped(String.Format(Loc.Localize("UI.Settings.UpdateLocalization.UpdatedAt", "Last Update: {0}"),
                    DateTimeOffset.FromUnixTimeMilliseconds(lastUpdateTime).ToString("dd/MM/yy - hh:mm tt")));
                }

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
                                this._configuration.hiddenMechanics.Add(mechanic);
                                break;
                            case true:
                                this._configuration.hiddenMechanics.Remove(mechanic);
                                break;
                        }
                        this._configuration.Save();
                    });

                    Tooltips.AddTooltip(String.Format(Loc.Localize("UI.Settings.HideMechanicTooltip", "Hide {0} from the duty guide."), Enum.GetName(typeof(Mechanics), mechanic)));

                    ImGui.NextColumn();
                }

                ImGui.EndChild();
                ImGui.EndTabItem();
            }


#if DEBUG
            // If this is a debug build, add a tab for some debug information.
            if (ImGui.BeginTabItem("Debug"))
            {
                // only as big as the content inside the child
                ImGui.BeginChild("debug", new Vector2(0, 240), false);
                ImGui.Columns(2, "debug", false);

                // Client debug information.
                Common.TextHeading("Client Information");
                ImGui.TextWrapped($"Language: {Service.PluginInterface.UiLanguage}");
                ImGui.NextColumn();
                ImGui.TextWrapped($"Loaded Duties: {DutyManager.GetDuties().Count}");
                ImGui.NextColumn();
                ImGui.NewLine();

                // Current duty debug information.
                Common.TextHeading("Duty Information");
                ImGui.TextWrapped($"Current Territory: {Service.ClientState.TerritoryType}");
                ImGui.NextColumn();
                ImGui.TextWrapped($"Current Duty: {DutyManager.GetPlayerDuty()?.Name ?? "None"}");
                ImGui.NextColumn();
                ImGui.NewLine();

                // Selected Duty debug information.
                Common.TextHeading("Selected Duty");
                ImGui.TextWrapped($"Name: {UIState.SelectedDuty?.Name ?? "None"}");
                ImGui.NextColumn();
                ImGui.TextWrapped($"Expansion: {Enum.GetName(typeof(Expansion), UIState.SelectedDuty?.Expansion ?? -1)}");
                ImGui.NextColumn();
                ImGui.TextWrapped($"Level: {UIState.SelectedDuty?.Level ?? 0}");
                ImGui.NextColumn();
                ImGui.TextWrapped($"Bosses: {UIState.SelectedDuty?.Bosses?.Count ?? 0}");
                ImGui.NextColumn();
                ImGui.TextWrapped($"QuestUnlockID: {UIState.SelectedDuty?.UnlockQuestID ?? 0}");
                ImGui.NextColumn();
                ImGui.TextWrapped($"TerritoryID: {UIState.SelectedDuty?.TerritoryID ?? 0}");
                ImGui.EndChild();

                Common.TextHeading("Localization");
                if (ImGui.InputTextWithHint("", "Localizable Output Directory", ref localizableOutputDir, 1000))
                {
                    this._configuration.localizableOutputDir = localizableOutputDir;
                    this._configuration.Save();
                }

                ImGui.SameLine();

                if (ImGui.Button("Export"))
                {
                    try
                    {
                        var directory = Directory.GetCurrentDirectory();
                        Directory.SetCurrentDirectory(localizableOutputDir);
                        Loc.ExportLocalizable();
                        File.Copy(Path.Combine(localizableOutputDir, "KikoGuide_Localizable.json"), Path.Combine(localizableOutputDir, "en.json"), true);
                        Directory.SetCurrentDirectory(directory);
                        Service.PluginInterface.UiBuilder.AddNotification("Localization exported successfully.", "KikoGuide", NotificationType.Success);
                    }
                    catch (Exception e)
                    {
                        PluginLog.Error($"Failed to export localization {e.Message}");
                        Service.PluginInterface.UiBuilder.AddNotification("Something went wrong exporting, see /xllog for details.", "KikoGuide", NotificationType.Error);
                    }
                }

                ImGui.EndTabItem();
            }
#endif

            ImGui.EndTabBar();
            ImGui.End();
        }
    }
}