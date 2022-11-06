using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Localization;
using KikoGuide.Managers;
using KikoGuide.UI.ImGuiBasicComponents;
using KikoGuide.UI.ImGuiFullComponents.IPCProviderCombo;
using KikoGuide.UI.ImGuiFullComponents.MechanicHiderCombo;

namespace KikoGuide.UI.Windows.Settings
{
    public sealed class SettingsWindow : Window, IDisposable
    {
        internal SettingsPresenter Presenter;
        public SettingsWindow() : base(WindowManager.SettingsWindowName)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(400, 250),
                MaximumSize = new Vector2(600, 250)
            };

            this.SizeCondition = ImGuiCond.FirstUseEver;

            this.Presenter = new SettingsPresenter();
        }
        public void Dispose() => this.Presenter.Dispose();

        public override void Draw()
        {
            var autoOpenGuideForDuty = SettingsPresenter.Configuration.Display.AutoToggleGuideForDuty;
            var shortenGuideText = SettingsPresenter.Configuration.Accessiblity.ShortenGuideText;
            var supportButtonShown = SettingsPresenter.Configuration.Display.DonateButtonShown;
            var hideLockedGuides = SettingsPresenter.Configuration.Display.HideLockedGuides;

            if (ImGui.BeginTabBar("##SettingsTabs"))
            {
                // General settings go in here.
                if (ImGui.BeginTabItem(TSettings.Configuration))
                {
                    ImGui.BeginChild("##SettingsGeneralTabContent");
                    if (ImGui.BeginTable("##SettingsGeneralTable", 2))
                    {
                        ImGui.TableNextRow();


                        // Auto open in duty setting
                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextWrapped(TSettings.AutoOpenGuideForDuty);
                        ImGui.TableSetColumnIndex(1);
                        ImGui.SetNextItemWidth(-1);
                        if (ImGui.BeginCombo("##AutoOpenGuideForDuty", autoOpenGuideForDuty ? TGenerics.Enabled : TGenerics.Disabled))
                        {
                            if (ImGui.Selectable(TGenerics.Enabled, autoOpenGuideForDuty))
                            {
                                SettingsPresenter.Configuration.Display.AutoToggleGuideForDuty = true;
                                SettingsPresenter.Configuration.Save();
                            }
                            if (ImGui.Selectable(TGenerics.Disabled, !autoOpenGuideForDuty))
                            {
                                SettingsPresenter.Configuration.Display.AutoToggleGuideForDuty = false;
                                SettingsPresenter.Configuration.Save();
                            }
                            ImGui.EndCombo();
                        }
                        Common.AddTooltip(TSettings.SettingsAutoOpenInDutyTooltip);
                        ImGui.TableNextRow();


                        // Donate/support button shown setting
                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextWrapped(TSettings.ShowDonateButton);
                        ImGui.TableSetColumnIndex(1);
                        ImGui.SetNextItemWidth(-1);
                        if (ImGui.BeginCombo("##DonateButtonShown", supportButtonShown ? TGenerics.Enabled : TGenerics.Disabled))
                        {
                            if (ImGui.Selectable(TGenerics.Enabled, supportButtonShown))
                            {
                                SettingsPresenter.Configuration.Display.DonateButtonShown = true;
                                SettingsPresenter.Configuration.Save();
                            }
                            if (ImGui.Selectable(TGenerics.Disabled, !supportButtonShown))
                            {
                                SettingsPresenter.Configuration.Display.DonateButtonShown = false;
                                SettingsPresenter.Configuration.Save();
                            }
                            ImGui.EndCombo();
                        }
                        Common.AddTooltip(TSettings.ShowDonateButtonTooltip);
                        ImGui.TableNextRow();


                        // Shorten guide text setting
                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextWrapped(TSettings.ShortenGuideText);
                        ImGui.TableSetColumnIndex(1);
                        ImGui.SetNextItemWidth(-1);
                        if (ImGui.BeginCombo("##ShortenGuideText", shortenGuideText ? TGenerics.Enabled : TGenerics.Disabled))
                        {
                            if (ImGui.Selectable(TGenerics.Enabled, shortenGuideText))
                            {
                                SettingsPresenter.Configuration.Accessiblity.ShortenGuideText = true;
                                SettingsPresenter.Configuration.Save();
                            }
                            if (ImGui.Selectable(TGenerics.Disabled, !shortenGuideText))
                            {
                                SettingsPresenter.Configuration.Accessiblity.ShortenGuideText = false;
                                SettingsPresenter.Configuration.Save();
                            }
                            ImGui.EndCombo();
                        }
                        Common.AddTooltip(TSettings.ShortenGuideTextTooltip);
                        ImGui.TableNextRow();


                        // Hide locked gudies setting
                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextWrapped(TSettings.HideLockedGuides);
                        ImGui.TableSetColumnIndex(1);
                        ImGui.SetNextItemWidth(-1);
                        if (ImGui.BeginCombo("##HideLockedGuides", hideLockedGuides ? TGenerics.Enabled : TGenerics.Disabled))
                        {
                            if (ImGui.Selectable(TGenerics.Enabled, hideLockedGuides))
                            {
                                SettingsPresenter.Configuration.Display.HideLockedGuides = true;
                                SettingsPresenter.Configuration.Save();
                            }
                            if (ImGui.Selectable(TGenerics.Disabled, !hideLockedGuides))
                            {
                                SettingsPresenter.Configuration.Display.HideLockedGuides = false;
                                SettingsPresenter.Configuration.Save();
                            }
                            ImGui.EndCombo();
                        }
                        Common.AddTooltip(TSettings.HideLockedGuidesTooltip);
                        ImGui.TableNextRow();


                        // Hidden mechanics setting
                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextWrapped(TSettings.HiddenMechanics);
                        ImGui.TableSetColumnIndex(1);
                        ImGui.SetNextItemWidth(-1);
                        MechanicHiderComboComponent.Draw();
                        Common.AddTooltip(TSettings.HiddenMechanicsTooltip);
                        ImGui.TableNextRow();


                        // IPC Providers enabled setting
                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextWrapped(TSettings.EnabledIntegrations);
                        ImGui.TableSetColumnIndex(1);
                        ImGui.SetNextItemWidth(-1);
                        IPCProviderComboComponent.Draw();
                        Common.AddTooltip(TSettings.EnabledIntegrationsTooltip);

#if DEBUG
                        ImGui.TableNextRow();
                        ImGui.TableSetColumnIndex(0);
                        ImGui.TextWrapped("Export Localization");
                        ImGui.TableSetColumnIndex(1);
                        this.Presenter.DialogManager.Draw();
                        if (ImGui.Button("Export"))
                        {
                            this.Presenter.DialogManager.OpenFolderDialog("Export Localization", SettingsPresenter.OnDirectoryPicked);
                        }
#endif


                        ImGui.EndTable();
                        ImGui.EndChild();
                        ImGui.EndTabItem();
                    }
                }
            }
        }
    }
}