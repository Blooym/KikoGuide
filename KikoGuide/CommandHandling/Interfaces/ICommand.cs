using Dalamud.Game.Command;

namespace KikoGuide.CommandHandling.Interfaces
{
    /// <summary>
    ///     Represents a command.
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        ///     The name of the command (including the /)
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The command info.
        /// </summary>
        CommandInfo Command { get; }

        /// <summary>
        ///     The command's execution handler.
        /// </summary>
        CommandInfo.HandlerDelegate OnExecute { get; }
    }
}
