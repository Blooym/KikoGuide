namespace KikoGuide.Managers;

using System;
using CheapLoc;
using KikoGuide.Base;
using Dalamud.Game.Command;

sealed internal class CommandManager : IDisposable
{
    private const string listCommand = "/kikolist";
    private const string settingsCommand = "/kikoconfig";
    private const string editorCommand = "/kikoeditor";
    private const string dutyInfoCommand = "/kikoinfo";

    /// <summary>
    ///     Initializes the plugin commands.
    /// </summary>
    public void Initialize()
    {
        Service.CommandManager.AddHandler(listCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.List.Help", "Toggles the duty list"),
        });

        Service.CommandManager.AddHandler(settingsCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Settings.Help", "Toggles the settings menu"),
        });

        Service.CommandManager.AddHandler(editorCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Editor.Help", "Toggles the duty editor"),
        });

        Service.CommandManager.AddHandler(dutyInfoCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Info.Help", "Toggles the duty info window if a duty is loaded"),
        });
    }

    /// <summary>
    ///     Dispose of all PluginCommand handlers.
    /// </summary>
    public void Dispose()
    {
        Service.CommandManager.RemoveHandler(listCommand);
        Service.CommandManager.RemoveHandler(settingsCommand);
        Service.CommandManager.RemoveHandler(editorCommand);
        Service.CommandManager.RemoveHandler(dutyInfoCommand);
    }

    /// <summary>
    ///     Event handler for when a command is issued by the user.
    /// </summary>
    private void OnCommand(string command, string args)
    {
        switch (command)
        {
            case listCommand:
                KikoPlugin.listScreen.presenter.isVisible = !KikoPlugin.listScreen.presenter.isVisible;
                break;
            case settingsCommand:
                KikoPlugin.settingsScreen.presenter.isVisible = !KikoPlugin.settingsScreen.presenter.isVisible;
                break;
            case editorCommand:
                KikoPlugin.editorScreen.presenter.isVisible = !KikoPlugin.editorScreen.presenter.isVisible;
                break;
            case dutyInfoCommand:
                KikoPlugin.editorScreen.presenter.isVisible = !KikoPlugin.editorScreen.presenter.isVisible;
                break;
        }
    }
}