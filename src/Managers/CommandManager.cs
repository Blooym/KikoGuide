namespace KikoGuide.Managers
{
    using System;
    using Dalamud.Logging;
    using Dalamud.Game.Command;
    using KikoGuide.Base;
    using KikoGuide.UI.Windows.DutyList;
    using KikoGuide.UI.Windows.DutyInfo;
    using KikoGuide.UI.Windows.Editor;
    using KikoGuide.UI.Windows.Settings;

    /// <summary> 
    ///     Initializes and manages all commands and command-events for the plugin.
    /// </summary>
    sealed public class CommandManager : IDisposable
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
            PluginLog.Debug("CommandManager(CommandManager): Initializing...");

            PluginService.Commands.AddHandler(listCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.DutyListHelp });
            PluginService.Commands.AddHandler(settingsCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.SettingsHelp });
            PluginService.Commands.AddHandler(editorCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.EditorHelp });
            PluginService.Commands.AddHandler(dutyInfoCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.InfoHelp });

            PluginLog.Debug("CommandManager(CommandManager): Initialization complete.");
        }


        /// <summary>
        ///     Dispose of the PluginCommandManager and its resources.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("CommandManager(Dispose): Disposing...");

            PluginService.Commands.RemoveHandler(listCommand);
            PluginService.Commands.RemoveHandler(settingsCommand);
            PluginService.Commands.RemoveHandler(editorCommand);
            PluginService.Commands.RemoveHandler(dutyInfoCommand);

            PluginLog.Debug("CommandManager(Dispose): Successfully disposed.");
        }


        /// <summary>
        ///     Event handler for when a command is issued by the user.
        /// </summary>
        private void OnCommand(string command, string args)
        {
            var windowSystem = PluginService.WindowManager.windowSystem;
            switch (command)
            {
                case listCommand:
                    if (windowSystem.GetWindow("List") is DutyListWindow dutyListWindow)
                        dutyListWindow.IsOpen = !dutyListWindow.IsOpen;
                    break;
                case settingsCommand:
                    if (windowSystem.GetWindow("Settings") is SettingsWindow settingsWindow)
                        settingsWindow.IsOpen = !settingsWindow.IsOpen;
                    break;
                case editorCommand:
                    if (windowSystem.GetWindow("Editor") is EditorWindow editorWindow)
                        editorWindow.IsOpen = !editorWindow.IsOpen;
                    break;
                case dutyInfoCommand:
                    if (windowSystem.GetWindow("Info") is DutyInfoWindow dutyInfoScreen)
                        dutyInfoScreen.IsOpen = !dutyInfoScreen.IsOpen;
                    break;
            }
        }
    }
}