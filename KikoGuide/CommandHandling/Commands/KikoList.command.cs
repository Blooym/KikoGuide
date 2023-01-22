using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using Sirensong.Game.UI;

namespace KikoGuide.CommandHandling.Commands
{
    public class KikoList : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideList;

        /// <inheritdoc />
        public bool Enabled { get; set; } = true;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute)
        {
            HelpMessage = Constants.Commands.GuideListHelp,
            ShowInHelp = true,
        };

        /// <inheritdoc />
        public CommandInfo.HandlerDelegate OnExecute => (command, arguments) =>
        {
            if (command == Constants.Commands.GuideList)
            {
                GameChat.Print("Opening list window.");
            }
        };
    }
}