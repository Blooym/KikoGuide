using System;
using KikoGuide.CommandHandling.Commands;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;

namespace KikoGuide.CommandHandling
{
    public sealed class CommandManager : IDisposable
    {
        /// <summary>
        ///     The singleton instance of <see cref="CommandManager"/>.
        /// </summary>
        public static CommandManager Instance { get; } = new();

        /// <summary>
        ///     The list of registered commands.
        /// </summary>
        private ICommand[] commands =
        {
            new KikoList(),
            new KikoViewer(),
            new KikoSettings(),
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandManager"/> class.
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
            foreach (var command in this.commands)
            {
                Services.Commands.RemoveHandler(command.Name);
            }
            this.commands = Array.Empty<ICommand>();
            GC.SuppressFinalize(this);
        }
    }
}
