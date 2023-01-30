using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.Game;

namespace KikoGuide.CommandHandling.Commands
{
    internal sealed class KikoCommand : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideViewer;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute)
        {
            HelpMessage = Strings.Commands_GuideViewer_Help,
            ShowInHelp = true,
        };

        /// <inheritdoc />
        public CommandInfo.HandlerDelegate OnExecute => (command, arguments) =>
        {
            if (command == Constants.Commands.GuideViewer)
            {
                if (Services.GuideManager.SelectedGuide == null)
                {
                    GameChat.PrintError(Strings.Commands_GuideViewer_NoGuide);
                    return;
                }
                Services.WindowManager.ToggleGuideViewerWindow();
            }
        };
    }
}
