namespace KikoGuide.UI.Windows.Settings
{
    using System;
    using System.Numerics;
    using System.Linq;
    using Dalamud.Interface.Windowing;
    using ImGuiNET;
    using KikoGuide.Base;
    using KikoGuide.Types;
    using KikoGuide.IPC;
    using KikoGuide.Managers;
    using KikoGuide.UI.Components;

    sealed public class SettingsWindow : Window, IDisposable
    {
        /// <summary>
        ///     The presenter associated with the window, handles all logic and data.
        /// </summary>
        public SettingsPresenter presenter = new SettingsPresenter();

        /// <summary>
        ///     Instantiate a new settings window.
        /// </summary>
        public SettingsWindow() : base(WindowManager.SettingsWindowName)
        {
            Size = new Vector2(400, 400);
            SizeCondition = ImGuiCond.FirstUseEver;
        }

        /// <summary>
        ///     Dispose of the window and all associated resources.
        /// </summary>
        public void Dispose() => this.presenter.Dispose();

        /// <summary>
        ///     Draws the settings window.
        /// </summary>
        public override void Draw()
        {

            var disabledMechanics = PluginService.Configuration.Display.DisabledMechanics;
            var autoOpenDuty = PluginService.Configuration.Display.AutoToggleGuideForDuty;
            var shortenStrategies = PluginService.Configuration.Accessiblity.ShortenGuideText;
            var supportButtonShown = PluginService.Configuration.Display.SupportButtonShown;

            if (ImGui.BeginTabBar("##Settings"))
            {

                // General settings go in here.
                if (ImGui.BeginTabItem(TStrings.SettingsGeneral))
                {
                    // Auto-open duty setting.
                    Common.ToggleCheckbox(TStrings.SettingsAutoOpenInDuty, ref autoOpenDuty, () =>
                   {
                       PluginService.Configuration.Display.AutoToggleGuideForDuty = !autoOpenDuty;
                       PluginService.Configuration.Save();
                   });
                    Tooltips.AddTooltip(TStrings.SettingsAutoOpenInDutyTooltip);


                    // Short mode setting.
                    Common.ToggleCheckbox(TStrings.SettingsShortMode, ref shortenStrategies, () =>
                    {
                        PluginService.Configuration.Accessiblity.ShortenGuideText = !shortenStrategies;
                        PluginService.Configuration.Save();
                    });
                    Tooltips.AddTooltip(TStrings.SettingsShortModeTooltip);


                    // Support button setting.
                    Common.ToggleCheckbox(TStrings.SettingsShowSupportButton, ref supportButtonShown, () =>
                    {
                        PluginService.Configuration.Display.SupportButtonShown = !supportButtonShown;
                        PluginService.Configuration.Save();
                    });
                    Tooltips.AddTooltip(TStrings.SettingsShowSupportButtonTooltip);


                    // Update resources / localizable button.
                    ImGui.Dummy(new Vector2(0, 5));
                    Common.TextHeading(TStrings.SettingsResourcesAndLocalization);

#if DEBUG
                    this.presenter.dialogManager.Draw();
                    if (ImGui.Button("Export Localizable")) this.presenter.dialogManager.OpenFolderDialog("Select Export Directory", this.presenter.OnDirectoryPicked);
#endif

                    ImGui.EndTabItem();
                }


                // Mechanics settings go in here. 
                if (ImGui.BeginTabItem(TStrings.SettingsMechanics))
                {
                    // Create a child since we're using columns.
                    ImGui.BeginChild("##Mechanics", new Vector2(0, 0), false);
                    ImGui.Columns(2, "##Mechanics", false);

                    // For each mechanic enum, creating a checkbox for it.
                    foreach (var mechanic in Enum.GetValues(typeof(DutyMechanics)).Cast<int>().ToList())
                    {
                        // See if the mechanic is enabled by looking at the list for the enum value.
                        var isMechanicDisabled = disabledMechanics.Contains((DutyMechanics)mechanic);

                        // Create a checkbox for the mechanic.
                        Common.ToggleCheckbox(TStrings.SettingsHideMechanic(Enum.GetName(typeof(DutyMechanics), mechanic)), ref isMechanicDisabled, () =>
                        {
                            switch (isMechanicDisabled)
                            {
                                case false:
                                    PluginService.Configuration.Display.DisabledMechanics.Add((DutyMechanics)mechanic);
                                    break;
                                case true:
                                    PluginService.Configuration.Display.DisabledMechanics.Remove((DutyMechanics)mechanic);
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
                if (ImGui.BeginTabItem(TStrings.SettingsIntegrations))
                {
                    ImGui.TextWrapped(TStrings.SettingsIntegrationsDesc);
                    ImGui.Dummy(new Vector2(0, 10));
                    Common.TextHeading(TStrings.SettingsAvailableIntegrations);

                    // For each mechanic enum, creating a checkbox for it.
                    foreach (var integration in Enum.GetValues(typeof(IPCProviders)))
                    {
                        var isIntegrationDisabled = PluginService.Configuration.IPC.EnabledIntegrations.Contains((IPCProviders)integration);
                        var name = IPCProvider.GetName((IPCProviders)integration);
                        var tooltip = IPCProvider.GetDescription((IPCProviders)integration);

                        Common.ToggleCheckbox(name, ref isIntegrationDisabled, () =>
                        {
                            switch (isIntegrationDisabled)
                            {
                                case false:
                                    PluginService.Configuration.IPC.EnabledIntegrations.Add((IPCProviders)integration);
                                    PluginService.IPC.EnableProvider((IPCProviders)integration);
                                    break;
                                case true:
                                    PluginService.Configuration.IPC.EnabledIntegrations.Remove((IPCProviders)integration);
                                    PluginService.IPC.DisableProvider((IPCProviders)integration);
                                    break;
                            }
                            PluginService.Configuration.Save();
                        });
                        Tooltips.AddTooltip(tooltip);
                    }

                    ImGui.EndTabItem();
                }
            }

            ImGui.EndTabBar();
        }
    }
}