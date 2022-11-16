using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using KikoGuide.Attributes;
using KikoGuide.IPC;
using KikoGuide.Localization;
using KikoGuide.UI.ImGuiBasicComponents;

namespace KikoGuide.UI.ImGuiFullComponents.IPCProviderCombo
{
    public static class IPCProviderComboComponent
    {
        private static string searchFilter = string.Empty;
        public static void Draw()
        {
            var enabledIntegrations = IPCProviderComboPresenter.Configuration.IPC.EnabledIntegrations;
            if (ImGui.BeginCombo("##IPCProviderCombo", $"Enabled Integrations: {enabledIntegrations.Count}"))
            {
                ImGui.SetNextItemWidth(-1);
                ImGui.InputTextWithHint("##IPCProviderComboSearch", TGenerics.Search, ref searchFilter, 100);
                ImGui.Separator();
                foreach (var provider in Enum.GetValues(typeof(IPCProviders)).Cast<IPCProviders>())
                {
                    if (searchFilter != string.Empty && !provider.ToString().Contains(searchFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (ImGui.Selectable(provider.GetNameAttribute(), enabledIntegrations?.Contains(provider) ?? false, ImGuiSelectableFlags.DontClosePopups))
                    {
                        if (enabledIntegrations?.Contains(provider) ?? false)
                        {
                            enabledIntegrations = enabledIntegrations.Where(t => t != provider).ToList();
                            IPCProviderComboPresenter.Configuration.IPC.EnabledIntegrations = enabledIntegrations;
                            IPCProviderComboPresenter.Configuration.Save();
                            IPCProviderComboPresenter.IPC.DisableProvider(provider);
                        }
                        else
                        {
                            enabledIntegrations = enabledIntegrations?.Append(provider).ToList() ?? new List<IPCProviders>() { provider };
                            IPCProviderComboPresenter.Configuration.IPC.EnabledIntegrations = enabledIntegrations;
                            IPCProviderComboPresenter.Configuration.Save();
                            IPCProviderComboPresenter.IPC.EnableProvider(provider);
                        }
                    }
                    Common.AddTooltip(provider.GetDescriptionAttribute());
                }
                ImGui.EndCombo();
            }
        }
    }
}
