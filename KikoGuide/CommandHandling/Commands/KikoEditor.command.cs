using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using Sirensong.Game.UI;

namespace KikoGuide.CommandHandling.Commands
{
    public class KikoEditor : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideEditor;

        /// <inheritdoc />
        public bool Enabled { get; set; } = true;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute)
        {
            HelpMessage = Constants.Commands.GuideEditorHelp,
            ShowInHelp = true,
        };

        /// <inheritdoc />
        public CommandInfo.HandlerDelegate OnExecute => (command, arguments) =>
        {
            if (command == Constants.Commands.GuideEditor)
            {
                GameChat.Print("Opening viewer window.");
            }
        };
    }
}