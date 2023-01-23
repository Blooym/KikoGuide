using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows;

namespace KikoGuide.CommandHandling.Commands
{
    public sealed class KikoViewer : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideViewer;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute)
        {
            HelpMessage = Constants.Commands.GuideViewerHelp,
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
