using System;
using Dalamud.Game.Command;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.UI.Windows.Editor;
using KikoGuide.UI.Windows.GuideList;
using KikoGuide.UI.Windows.GuideViewer;
using KikoGuide.UI.Windows.Settings;

namespace KikoGuide.Managers
{
    /// <summary>
    ///     Initializes and manages all commands and command-events for the plugin.
    /// </summary>
    public sealed class CommandManager : IDisposable
    {
        private const string ListCommand = "/kikolist";
        private const string SettingsCommand = "/kikoconfig";
        private const string EditorCommand = "/kikoeditor";
        private const string GuideViewerCommand = "/kikoinfo";

        /// <summary>
        ///     Initializes the CommandManager and its resources.
        /// </summary>
        public CommandManager()
        {
            PluginLog.Debug("CommandManager(Constructor): Initializing...");

            PluginService.Commands.AddHandler(ListCommand, new CommandInfo(this.OnCommand) { HelpMessage = TCommands.GuideListHelp });
            PluginService.Commands.AddHandler(SettingsCommand, new CommandInfo(this.OnCommand) { HelpMessage = TCommands.SettingsHelp });
            PluginService.Commands.AddHandler(EditorCommand, new CommandInfo(this.OnCommand) { HelpMessage = TCommands.EditorHelp });
            PluginService.Commands.AddHandler(GuideViewerCommand, new CommandInfo(this.OnCommand) { HelpMessage = TCommands.InfoHelp });

            PluginLog.Debug("CommandManager(Constructor): Initialization complete.");
        }

        /// <summary>
        ///     Dispose of the PluginCommandManager and its resources.
        /// </summary>
        public void Dispose()
        {
            PluginService.Commands.RemoveHandler(ListCommand);
            PluginService.Commands.RemoveHandler(SettingsCommand);
            PluginService.Commands.RemoveHandler(EditorCommand);
            PluginService.Commands.RemoveHandler(GuideViewerCommand);

            PluginLog.Debug("CommandManager(Dispose): Successfully disposed.");
        }

        /// <summary>
        ///     Event handler for when a command is issued by the user.
        /// </summary>
        /// <param name="command">The command that was issued.</param>
        /// <param name="args">The arguments that were passed with the command.</param>
        private void OnCommand(string command, string args)
        {
            var windowManager = PluginService.WindowManager;
            switch (command)
            {
                case ListCommand:
                    if (windowManager.GetWindow(TWindowNames.GuideList) is GuideListWindow guideListWindow)
                    {
                        guideListWindow.IsOpen = !guideListWindow.IsOpen;
                    }

                    break;
                case SettingsCommand:
                    if (windowManager.GetWindow(TWindowNames.Settings) is SettingsWindow settingsWindow)
                    {
                        settingsWindow.IsOpen = !settingsWindow.IsOpen;
                    }

                    break;
                case EditorCommand:
                    if (windowManager.GetWindow(TWindowNames.GuideEditor) is EditorWindow editorWindow)
                    {
                        editorWindow.IsOpen = !editorWindow.IsOpen;
                    }

                    break;
                case GuideViewerCommand:
                    if (windowManager.GetWindow(TWindowNames.GuideViewer) is GuideViewerWindow guideViewerScreen)
                    {
                        guideViewerScreen.IsOpen = !guideViewerScreen.IsOpen;
                    }
                    break;
            }
        }
    }
}
