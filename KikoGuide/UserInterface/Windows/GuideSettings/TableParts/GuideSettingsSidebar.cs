using System;
using System.Collections.Generic;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.UserInterface.Windows.GuideSettings.TableParts
{
    internal static class GuideSettingsSidebar
    {
        /// <summary>
        ///     Draws the integrations sidebar.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(GuideSettingsLogic logic) => DrawGuideConfigsList(logic);

        /// <summary>
        ///     Draws a list of integrations.
        /// </summary>
        /// <param name="logic"></param>
        private static void DrawGuideConfigsList(GuideSettingsLogic logic)
        {
            if (ImGui.Selectable(Strings.UserInterface_GuideSettings_About_Title, logic.SelectedGuideSettings == null))
            {
                logic.SelectedGuideSettings = null;
            }
            ImGui.Dummy(Spacing.SidebarSectionSpacing);

            SiGui.Heading(Strings.UserInterface_GuideSettings_Title);

            var seenAbstractTypes = new HashSet<Type>();
            foreach (var type in Services.GuideManager.GetGuides())
            {
                var baseType = type.GetType()?.BaseType;
                if (baseType == null || !baseType.IsAbstract || seenAbstractTypes.Contains(baseType))
                {
                    continue;
                }

                seenAbstractTypes.Add(baseType);

                if (ImGui.Selectable(type.Configuration.Name, logic.SelectedGuideSettings == type.Configuration))
                {
                    logic.SelectedGuideSettings = type.Configuration;
                }
            }
        }
    }
}
