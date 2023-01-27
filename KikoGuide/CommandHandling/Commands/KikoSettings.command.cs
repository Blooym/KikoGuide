using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;

namespace KikoGuide.CommandHandling.Commands
{
    internal sealed class KikoSettingsCommand : ICommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.Settings;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute)
        {
            HelpMessage = Strings.Settings_Command_Help,
            ShowInHelp = true,
        };

        /// <inheritdoc />
        public CommandInfo.HandlerDelegate OnExecute => (command, arguments) =>
        {
            if (command == Constants.Commands.Settings)
            {
                Services.WindowManager.ToggleSettingsWindow();
            }
        };
    }
}
