using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using KikoGuide.UserInterface.Windows;

namespace KikoGuide.CommandHandling.Commands
{
    public sealed class KikoList : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideList;

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
                Services.WindowManager.WindowingSystem.GetWindow<GuideListWindow>()?.Toggle();
            }
        };
    }
}
