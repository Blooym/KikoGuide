using System;
using KikoGuide.CommandHandling.Commands;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;

namespace KikoGuide.CommandHandling
{
    /// <summary>
    ///     Handles the lifespan and execution of all commands.
    /// </summary>
    internal sealed class CommandManager : IDisposable
    {
        /// <summary>
        ///     All  commands to register with the <see cref="Dalamud.Game.Command.CommandManager" />, holds all references.
        /// </summary>
        private IDalamudCommand[] commands =
        {
            new KikoListDalamudCommand(), new KikoDalamudCommand(),
        };

        private bool disposedValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandManager" /> class.
        /// </summary>
        private CommandManager()
        {
            foreach (var command in this.commands)
            {
                Services.Commands.AddHandler(command.Name, command.Command);
            }
        }

        /// <summary>
        ///     Disposes of the command manager.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                foreach (var command in this.commands)
                {
                    Services.Commands.RemoveHandler(command.Name);
                }
                this.commands = Array.Empty<IDalamudCommand>();

                this.disposedValue = true;
            }
        }
    }
}
