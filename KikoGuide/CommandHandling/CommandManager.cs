using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KikoGuide.CommandHandling.Interfaces;
using KikoGuide.Common;

namespace KikoGuide.CommandHandling
{
    public class CommandManager : IDisposable
    {
        /// <summary>
        ///     The singleton instance of <see cref="CommandManager"/>.
        /// </summary>
        private static CommandManager? instance;

        /// <summary>
        ///     Gets the singleton instance of <see cref="CommandManager"/>.
        /// </summary>
        public static CommandManager Instance => instance ??= new();

        /// <summary>
        ///     The list of registered commands.
        /// </summary>
        private readonly List<ICommand> registeredCommands = new();

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandManager"/> class.
        /// </summary>
        private CommandManager()
        {
            foreach (var command in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand))))
            {
                var commandInstance = (ICommand?)command.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                if (commandInstance == null || !commandInstance.Enabled)
                {
                    continue;
                }
                Services.Commands.AddHandler(commandInstance.Name, commandInstance.Command);
                this.registeredCommands.Add(commandInstance);
            }
        }

        /// <summary>
        ///     Disposes of the command manager.
        /// </summary>
        public void Dispose()
        {
            foreach (var command in this.registeredCommands)
            {
                Services.Commands.RemoveHandler(command.Name);
            }

            this.registeredCommands.Clear();
            GC.SuppressFinalize(this);
        }
    }
}