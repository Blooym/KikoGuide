using System;
using Dalamud.Game.Command;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.UI.Windows.DutyInfo;
using KikoGuide.UI.Windows.DutyList;
using KikoGuide.UI.Windows.Editor;
using KikoGuide.UI.Windows.Settings;

namespace KikoGuide.Managers
{
    /// <summary> 
    ///     Initializes and manages all commands and command-events for the plugin.
    /// </summary>
    public sealed class CommandManager : IDisposable
    {
        private const string listCommand = "/kikolist";
        private const string settingsCommand = "/kikoconfig";
        private const string editorCommand = "/kikoeditor";
        private const string dutyInfoCommand = "/kikoinfo";

        /// <summary>
        ///     Initializes the CommandManager and its resources.
        /// </summary>
        public CommandManager()
        {
            PluginLog.Debug("CommandManager(Constructor): Initializing...");

            _ = PluginService.Commands.AddHandler(listCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.DutyListHelp });
            _ = PluginService.Commands.AddHandler(settingsCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.SettingsHelp });
            _ = PluginService.Commands.AddHandler(editorCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.EditorHelp });
            _ = PluginService.Commands.AddHandler(dutyInfoCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.InfoHelp });

            PluginLog.Debug("CommandManager(Constructor): Initialization complete.");
        }

        /// <summary>
        ///     Dispose of the PluginCommandManager and its resources.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("CommandManager(Dispose): Disposing...");

            _ = PluginService.Commands.RemoveHandler(listCommand);
            _ = PluginService.Commands.RemoveHandler(settingsCommand);
            _ = PluginService.Commands.RemoveHandler(editorCommand);
            _ = PluginService.Commands.RemoveHandler(dutyInfoCommand);

            PluginLog.Debug("CommandManager(Dispose): Successfully disposed.");
        }

        /// <summary>
        ///     Event handler for when a command is issued by the user.
        /// </summary>
        private void OnCommand(string command, string args)
        {
            Dalamud.Interface.Windowing.WindowSystem windowSystem = PluginService.WindowManager.windowSystem;
            switch (command)
            {
                case listCommand:
                    if (windowSystem.GetWindow(WindowManager.DutyListWindowName) is DutyListWindow dutyListWindow)
                    {
                        dutyListWindow.IsOpen = !dutyListWindow.IsOpen;
                    }

                    break;
                case settingsCommand:
                    if (windowSystem.GetWindow(WindowManager.SettingsWindowName) is SettingsWindow settingsWindow)
                    {
                        settingsWindow.IsOpen = !settingsWindow.IsOpen;
                    }

                    break;
                case editorCommand:
                    if (windowSystem.GetWindow(WindowManager.EditorWindowName) is EditorWindow editorWindow)
                    {
                        editorWindow.IsOpen = !editorWindow.IsOpen;
                    }

                    break;
                case dutyInfoCommand:
                    if (windowSystem.GetWindow(WindowManager.DutyInfoWindowName) is DutyInfoWindow dutyInfoScreen)
                    {
                        dutyInfoScreen.IsOpen = !dutyInfoScreen.IsOpen;
                    }

                    break;
                default:
                    break;
            }
        }
    }
}