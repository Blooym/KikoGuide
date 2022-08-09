namespace KikoGuide.Managers;

using System;
using Dalamud.Logging;
using Dalamud.Game.Command;
using KikoGuide.Base;

/// <summary> Initializes and manages all commands and command-events for the plugin. </summary>
sealed public class CommandManager : IDisposable
{
    private const string listCommand = "/kikolist";
    private const string settingsCommand = "/kikoconfig";
    private const string editorCommand = "/kikoeditor";
    private const string dutyInfoCommand = "/kikoinfo";


    /// <summary> Initializes the CommandManager and its resources. </summary>
    public CommandManager()
    {
        PluginLog.Debug("CommandManager: Initializing...");

        PluginService.Commands.AddHandler(listCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.DutyListHelp() });
        PluginService.Commands.AddHandler(settingsCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.SettingsHelp() });
        PluginService.Commands.AddHandler(editorCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.EditorHelp() });
        PluginService.Commands.AddHandler(dutyInfoCommand, new CommandInfo(OnCommand) { HelpMessage = TStrings.InfoHelp() });

        PluginLog.Debug("CommandManager: Initialization complete.");
    }


    /// <summary> Dispose of the PluginCommandManager and its resources. </summary>
    public void Dispose()
    {
        PluginLog.Debug("CommandManager: Disposing...");

        PluginService.Commands.RemoveHandler(listCommand);
        PluginService.Commands.RemoveHandler(settingsCommand);
        PluginService.Commands.RemoveHandler(editorCommand);
        PluginService.Commands.RemoveHandler(dutyInfoCommand);

        PluginLog.Debug("CommandManager: Successfully disposed.");
    }


    /// <summary> Event handler for when a command is issued by the user. </summary>
    private void OnCommand(string command, string args)
    {
        switch (command)
        {
            case listCommand:
                PluginService.WindowManager.DutyList.presenter.isVisible = !PluginService.WindowManager.DutyList.presenter.isVisible;
                break;
            case settingsCommand:
                PluginService.WindowManager.Settings.presenter.isVisible = !PluginService.WindowManager.Settings.presenter.isVisible;
                break;
            case editorCommand:
                PluginService.WindowManager.Editor.presenter.isVisible = !PluginService.WindowManager.Editor.presenter.isVisible;
                break;
            case dutyInfoCommand:
                PluginService.WindowManager.DutyInfo.presenter.isVisible = !PluginService.WindowManager.DutyInfo.presenter.isVisible;
                break;
        }
    }
}