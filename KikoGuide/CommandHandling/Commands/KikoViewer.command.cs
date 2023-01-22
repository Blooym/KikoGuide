using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using Sirensong.Game.UI;

namespace KikoGuide.CommandHandling.Commands
{
    public class KikoViewer : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideViewer;

        /// <inheritdoc />
        public bool Enabled { get; set; } = true;

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
                GameChat.Print("Opening viewer window.");
            }
        };
    }
}