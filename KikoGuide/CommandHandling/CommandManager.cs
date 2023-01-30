using System;
using KikoGuide.CommandHandling.Commands;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;

namespace KikoGuide.CommandHandling
{
    internal sealed class CommandManager : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The list of registered commands.
        /// </summary>
        private ICommand[] commands =
        {
            new KikoListCommand(),
            new KikoCommand(),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandManager"/> class.
        /// </summary>
        private CommandManager()
        {
            foreach (var command in this.commands)
            {
                Services.Commands.AddHandler(command.Name, command.Command);
            }
        }

        /// <summary>
        /// Disposes of the command manager.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                foreach (var command in this.commands)
                {
                    Services.Commands.RemoveHandler(command.Name);
                }
                this.commands = Array.Empty<ICommand>();

                this.disposedValue = true;
            }
        }
    }
}
