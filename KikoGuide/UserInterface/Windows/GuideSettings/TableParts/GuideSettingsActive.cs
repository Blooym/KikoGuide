using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.GuideSettings.TableParts
{
    internal static class GuideSettingsActive
    {
        /// <summary>
        /// Draws the selected integration.
        /// </summary>
        /// <param name="logic"></param>
        public static void Draw(GuideSettingsLogic logic)
        {
            if (logic.SelectedGuideSettings == null)
            {
                DrawGuideSettingsAbout(logic);
                return;
            }

            SiGui.Heading(logic.SelectedGuideSettings.Name);
            logic.SelectedGuideSettings.Draw();
        }

        private static void DrawGuideSettingsAbout(GuideSettingsLogic _)
        {
            SiGui.Heading(Strings.UserInterface_GuideSettings_About_Title);
            SiGui.TextWrapped(Strings.UserInterface_GuideSettings_About_Body);
        }
    }
}