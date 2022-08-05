namespace KikoGuide.Base;

using CheapLoc;
using Dalamud.Logging;
using Dalamud.Game.Command;

/// <summary> Initializes and manages all commands and command-events for the plugin. </summary>
public static class PluginCommandManager
{
    private const string listCommand = "/kikolist";
    private const string settingsCommand = "/kikoconfig";
    private const string editorCommand = "/kikoeditor";
    private const string dutyInfoCommand = "/kikoinfo";


    /// <summary> Initializes the PluginCommandManager and its resources. </summary>
    public static void Initialize()
    {
        PluginLog.Debug("PluginCommandManager: Initializing...");

        PluginService.Commands.AddHandler(listCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.List.Help", "Toggles the duty list"),
        });

        PluginService.Commands.AddHandler(settingsCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Settings.Help", "Toggles the settings menu"),
        });

        PluginService.Commands.AddHandler(editorCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Editor.Help", "Toggles the duty editor"),
        });

        PluginService.Commands.AddHandler(dutyInfoCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Info.Help", "Toggles the duty info window if a duty is loaded"),
        });

        PluginLog.Debug("PluginCommandManager: Successfully initialized.");
    }


    /// <summary> Dispose of the PluginCommandManager and its resources. </summary>
    public static void Dispose()
    {
        PluginLog.Debug("PluginCommandManager: Disposing...");

        PluginService.Commands.RemoveHandler(listCommand);
        PluginService.Commands.RemoveHandler(settingsCommand);
        PluginService.Commands.RemoveHandler(editorCommand);
        PluginService.Commands.RemoveHandler(dutyInfoCommand);

        PluginLog.Debug("PluginCommandManager: Successfully disposed.");
    }


    /// <summary> Event handler for when a command is issued by the user. </summary>
    private static void OnCommand(string command, string args)
    {
        switch (command)
        {
            case listCommand:
                PluginWindowManager.DutyList.presenter.isVisible = !PluginWindowManager.DutyList.presenter.isVisible;
                break;
            case settingsCommand:
                PluginWindowManager.Settings.presenter.isVisible = !PluginWindowManager.Settings.presenter.isVisible;
                break;
            case editorCommand:
                PluginWindowManager.Editor.presenter.isVisible = !PluginWindowManager.Editor.presenter.isVisible;
                break;
            case dutyInfoCommand:
                PluginWindowManager.DutyInfo.presenter.isVisible = !PluginWindowManager.DutyInfo.presenter.isVisible;
                break;
        }
    }
}