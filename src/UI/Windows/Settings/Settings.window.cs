namespace KikoGuide.UI.Windows.Settings
{
    using System;
    using System.Numerics;
    using System.Linq;
    using Dalamud.Interface.Windowing;
    using ImGuiNET;
    using KikoGuide.IPC;
    using KikoGuide.Types;
    using KikoGuide.Managers;
    using KikoGuide.Localization;
    using KikoGuide.UI.ImGuiBasicComponents;

    public sealed class SettingsWindow : Window, IDisposable
    {
        public SettingsPresenter _presenter;
        public SettingsWindow() : base(WindowManager.SettingsWindowName)
        {
            Size = new Vector2(400, 400);
            SizeCondition = ImGuiCond.FirstUseEver;

            this._presenter = new SettingsPresenter();
        }
        public void Dispose() => this._presenter.Dispose();

        public override void Draw()
        {
            var disabledMechanics = this._presenter.GetConfiguration().Display.DisabledMechanics;
            var autoOpenDuty = this._presenter.GetConfiguration().Display.AutoToggleGuideForDuty;
            var shortenStrategies = this._presenter.GetConfiguration().Accessiblity.ShortenGuideText;
            var supportButtonShown = this._presenter.GetConfiguration().Display.DonateButtonShown;

            if (ImGui.BeginTabBar("##Settings"))
            {
                // General settings go in here.
                if (ImGui.BeginTabItem(TStrings.SettingsGeneral))
                {
                    // Auto-open duty setting.
                    Common.ToggleCheckbox(TStrings.SettingsAutoOpenInDuty, ref autoOpenDuty, () =>
                   {
                       this._presenter.GetConfiguration().Display.AutoToggleGuideForDuty = !autoOpenDuty;
                       this._presenter.GetConfiguration().Save();
                   });
                    Common.AddTooltip(TStrings.SettingsAutoOpenInDutyTooltip);


                    // Short mode setting.
                    Common.ToggleCheckbox(TStrings.SettingsShortMode, ref shortenStrategies, () =>
                    {
                        this._presenter.GetConfiguration().Accessiblity.ShortenGuideText = !shortenStrategies;
                        this._presenter.GetConfiguration().Save();
                    });
                    Common.AddTooltip(TStrings.SettingsShortModeTooltip);


                    // Support button setting.
                    Common.ToggleCheckbox(TStrings.SettingsShowSupportButton, ref supportButtonShown, () =>
                    {
                        this._presenter.GetConfiguration().Display.DonateButtonShown = !supportButtonShown;
                        this._presenter.GetConfiguration().Save();
                    });
                    Common.AddTooltip(TStrings.SettingsShowSupportButtonTooltip);


                    // Update resources / localizable button.
                    ImGui.Dummy(new Vector2(0, 5));
                    Common.TextHeading(TStrings.SettingsResourcesAndLocalization);

#if DEBUG
                    this._presenter.dialogManager.Draw();
                    if (ImGui.Button("Export Localizable")) this._presenter.dialogManager.OpenFolderDialog("Select Export Directory", this._presenter.OnDirectoryPicked);
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
                                    this._presenter.GetConfiguration().Display.DisabledMechanics.Add((DutyMechanics)mechanic);
                                    break;
                                case true:
                                    this._presenter.GetConfiguration().Display.DisabledMechanics.Remove((DutyMechanics)mechanic);
                                    break;
                            }
                            this._presenter.GetConfiguration().Save();
                        });

                        Common.AddTooltip(TStrings.SettingsHideMechanicTooltip(Enum.GetName(typeof(DutyMechanics), mechanic)));

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
                        var isIntegrationDisabled = this._presenter.GetConfiguration().IPC.EnabledIntegrations.Contains((IPCProviders)integration);
                        var name = LoCExtensions.GetLocalizedName((IPCProviders)integration);
                        var tooltip = LoCExtensions.GetLocalizedDescription((IPCProviders)integration);

                        Common.ToggleCheckbox(name, ref isIntegrationDisabled, () =>
                        {
                            switch (isIntegrationDisabled)
                            {
                                case false:
                                    this._presenter.GetConfiguration().IPC.EnabledIntegrations.Add((IPCProviders)integration);
                                    this._presenter.SetIPCProviderEnabled((IPCProviders)integration);
                                    break;
                                case true:
                                    this._presenter.GetConfiguration().IPC.EnabledIntegrations.Remove((IPCProviders)integration);
                                    this._presenter.SetIPCProviderDisabled((IPCProviders)integration);
                                    break;
                            }
                            this._presenter.GetConfiguration().Save();
                        });
                        Common.AddTooltip(tooltip);
                    }

                    ImGui.EndTabItem();
                }
            }

            ImGui.EndTabBar();
        }
    }
}