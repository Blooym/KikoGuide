using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.IPC;
using KikoGuide.Localization;
using KikoGuide.Managers;
using KikoGuide.Types;
using KikoGuide.UI.ImGuiBasicComponents;

namespace KikoGuide.UI.Windows.Settings
{
    public sealed class SettingsWindow : Window, IDisposable
    {
        public SettingsPresenter _presenter;
        public SettingsWindow() : base(WindowManager.SettingsWindowName)
        {
            Size = new Vector2(400, 400);
            SizeCondition = ImGuiCond.FirstUseEver;

            _presenter = new SettingsPresenter();
        }
        public void Dispose()
        {
            _presenter.Dispose();
        }

        public override void Draw()
        {
            System.Collections.Generic.List<DutyMechanics> disabledMechanics = SettingsPresenter.GetConfiguration().Display.DisabledMechanics;
            bool autoOpenDuty = SettingsPresenter.GetConfiguration().Display.AutoToggleGuideForDuty;
            bool shortenStrategies = SettingsPresenter.GetConfiguration().Accessiblity.ShortenGuideText;
            bool supportButtonShown = SettingsPresenter.GetConfiguration().Display.DonateButtonShown;

            if (ImGui.BeginTabBar("##Settings"))
            {
                // General settings go in here.
                if (ImGui.BeginTabItem(TStrings.SettingsGeneral))
                {
                    // Auto-open duty setting.
                    Common.ToggleCheckbox(TStrings.SettingsAutoOpenInDuty, ref autoOpenDuty, () =>
                   {
                       SettingsPresenter.GetConfiguration().Display.AutoToggleGuideForDuty = !autoOpenDuty;
                       SettingsPresenter.GetConfiguration().Save();
                   });
                    Common.AddTooltip(TStrings.SettingsAutoOpenInDutyTooltip);


                    // Short mode setting.
                    Common.ToggleCheckbox(TStrings.SettingsShortMode, ref shortenStrategies, () =>
                    {
                        SettingsPresenter.GetConfiguration().Accessiblity.ShortenGuideText = !shortenStrategies;
                        SettingsPresenter.GetConfiguration().Save();
                    });
                    Common.AddTooltip(TStrings.SettingsShortModeTooltip);


                    // Support button setting.
                    Common.ToggleCheckbox(TStrings.SettingsShowSupportButton, ref supportButtonShown, () =>
                    {
                        SettingsPresenter.GetConfiguration().Display.DonateButtonShown = !supportButtonShown;
                        SettingsPresenter.GetConfiguration().Save();
                    });
                    Common.AddTooltip(TStrings.SettingsShowSupportButtonTooltip);


                    // Update resources / localizable button.
                    ImGui.Dummy(new Vector2(0, 5));
                    Common.TextHeading(TStrings.SettingsResourcesAndLocalization);

#if DEBUG
                    _presenter.dialogManager.Draw();
                    if (ImGui.Button("Export Localizable"))
                    {
                        _presenter.dialogManager.OpenFolderDialog("Select Export Directory", SettingsPresenter.OnDirectoryPicked);
                    }
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
                    foreach (int mechanic in Enum.GetValues(typeof(DutyMechanics)).Cast<int>().ToList())
                    {
                        // See if the mechanic is enabled by looking at the list for the enum value.
                        bool isMechanicDisabled = disabledMechanics.Contains((DutyMechanics)mechanic);

                        // Create a checkbox for the mechanic.
                        Common.ToggleCheckbox(TStrings.SettingsHideMechanic(Enum.GetName(typeof(DutyMechanics), mechanic)), ref isMechanicDisabled, () =>
                        {
                            switch (isMechanicDisabled)
                            {
                                case false:
                                    SettingsPresenter.GetConfiguration().Display.DisabledMechanics.Add((DutyMechanics)mechanic);
                                    break;
                                case true:
                                    SettingsPresenter.GetConfiguration().Display.DisabledMechanics.Remove((DutyMechanics)mechanic);
                                    break;
                                default:
                            }
                            SettingsPresenter.GetConfiguration().Save();
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
                    foreach (object? integration in Enum.GetValues(typeof(IPCProviders)))
                    {
                        bool isIntegrationDisabled = SettingsPresenter.GetConfiguration().IPC.EnabledIntegrations.Contains((IPCProviders)integration);
                        string name = LoCExtensions.GetLocalizedName((IPCProviders)integration);
                        string tooltip = LoCExtensions.GetLocalizedDescription((IPCProviders)integration);

                        Common.ToggleCheckbox(name, ref isIntegrationDisabled, () =>
                        {
                            switch (isIntegrationDisabled)
                            {
                                case false:
                                    SettingsPresenter.GetConfiguration().IPC.EnabledIntegrations.Add((IPCProviders)integration);
                                    SettingsPresenter.SetIPCProviderEnabled((IPCProviders)integration);
                                    break;
                                case true:
                                    SettingsPresenter.GetConfiguration().IPC.EnabledIntegrations.Remove((IPCProviders)integration);
                                    SettingsPresenter.SetIPCProviderDisabled((IPCProviders)integration);
                                    break;
                                default:
                            }
                            SettingsPresenter.GetConfiguration().Save();
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