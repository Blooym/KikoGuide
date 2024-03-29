using Dalamud.Game.Command;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;

namespace KikoGuide.CommandHandling.Commands
{
    internal sealed class KikoListDalamudCommand : IDalamudCommand
    {
        /// <inheritdoc />
        public string Name => Constants.Commands.GuideList;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute)
        {
            HelpMessage = Strings.Commands_GuideList_Help, ShowInHelp = true,
        };

        /// <inheritdoc />
        public CommandInfo.HandlerDelegate OnExecute => (command, arguments) =>
        {
            if (command == Constants.Commands.GuideList)
            {
                Services.WindowManager.ToggleGuideListWindow();
            }
        };
    }
}
