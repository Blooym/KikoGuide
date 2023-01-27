using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.Game.UI;

namespace KikoGuide.CommandHandling.Commands
{
    internal sealed class KikoCommand : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideViewer;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute)
        {
            HelpMessage = Strings.Guide_Viewer_Command_Help,
            ShowInHelp = true,
        };

        /// <inheritdoc />
        public CommandInfo.HandlerDelegate OnExecute => (command, arguments) =>
        {
            if (command == Constants.Commands.GuideViewer)
            {
                if (Services.GuideManager.SelectedGuide == null)
                {
                    GameChat.PrintError($"Cannot open the guide viewer without a guide selected.");
                    return;
                }
                Services.WindowManager.ToggleGuideViewerWindow();
            }
        };
    }
}
