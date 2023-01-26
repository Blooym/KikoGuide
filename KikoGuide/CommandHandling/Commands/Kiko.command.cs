using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using KikoGuide.UserInterface.Windows.GuideViewer;

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
                Services.WindowManager.WindowingSystem.GetWindow<GuideViewerWindow>()?.Toggle();
            }
        };
    }
}
