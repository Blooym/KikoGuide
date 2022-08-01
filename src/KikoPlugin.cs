namespace KikoGuide;

using System;
using System.IO;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.UI.DutyList;
using KikoGuide.UI.Editor;
using KikoGuide.UI.Settings;
using KikoGuide.UI.DutyInfo;
using KikoGuide.Base;
using KikoGuide.Managers;
using CheapLoc;

internal class KikoPlugin : IDalamudPlugin
{
    public string Name => PStrings.pluginName;
    private const string listCommand = "/kikolist";
    private const string settingsCommand = "/kikoconfig";
    private const string editorCommand = "/kikoeditor";

    public static ListScreen listScreen = new ListScreen();
    public static SettingsScreen settingsScreen = new SettingsScreen();
    public static DutyInfoScreen dutyInfoScreen = new DutyInfoScreen();
    public static EditorScreen editorScreen = new EditorScreen();

    public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        // Inject/Create all services
        pluginInterface.Create<Service>();

        // Initialize everything & load trigger any events
        Service.Initialize(Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration());
        OnLanguageChange(Service.PluginInterface.UiLanguage);
        UpdateManager.UpdateResources();

        // Register event handlers
        Service.PluginInterface.UiBuilder.Draw += DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        Service.PluginInterface.LanguageChanged += OnLanguageChange;
        Service.ClientState.Logout += OnLogout;
        UpdateManager.ResourcesUpdated += DutyManager.OnResourceUpdate;

        // Register all command handlers
        Service.Commands.AddHandler(listCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.List.Help", "Opens the duty list"),
        });

        Service.Commands.AddHandler(settingsCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Settings.Help", "Opens the settings menu"),
        });

        Service.Commands.AddHandler(editorCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Editor.Help", "Opens the duty editor"),
        });
    }


    ///<summary>
    ///     Handles disposing of all resources used by the plugin.
    /// </summary>
    public void Dispose()
    {
        listScreen.Dispose();
        settingsScreen.Dispose();
        dutyInfoScreen.Dispose();
        editorScreen.Dispose();
        Service.Commands.RemoveHandler(listCommand);
        Service.Commands.RemoveHandler(settingsCommand);
        Service.Commands.RemoveHandler(editorCommand);
        UpdateManager.ResourcesUpdated -= DutyManager.OnResourceUpdate;
        Service.ClientState.Logout -= OnLogout;
        Service.PluginInterface.LanguageChanged -= OnLanguageChange;
        Service.PluginInterface.UiBuilder.Draw -= DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
    }

    /// <summary> 
    ///     Event handler for the client is logging out.
    /// </summary>
    // consume the onLogout event and handle it
    public static void OnLogout(object? sender, EventArgs e)
    {
        dutyInfoScreen.presenter.selectedDuty = null;
        listScreen.presenter.isVisible = false;
        dutyInfoScreen.presenter.isVisible = false;
        editorScreen.presenter.isVisible = false;
    }


    /// <summary>
    ///    Event handler for when the language is changed, reloads the localization strings.
    /// </summary>
    public static void OnLanguageChange(string language)
    {
        var uiLang = Service.PluginInterface.UiLanguage;

        DutyManager.OnResourceUpdate();

        try { Loc.Setup(File.ReadAllText($"{PStrings.localizationPath}\\Plugin\\{uiLang}.json")); }
        catch { Loc.SetupWithFallbacks(); }
    }


    /// <summary>
    ///     Event handler for when a command is issued by the user.
    /// </summary>
    private protected void OnCommand(string command, string args)
    {
        switch (command)
        {
            case listCommand:
                listScreen.presenter.isVisible = !listScreen.presenter.isVisible;
                break;
            case settingsCommand:
                settingsScreen.presenter.isVisible = !settingsScreen.presenter.isVisible;
                break;
            case editorCommand:
                editorScreen.presenter.isVisible = !editorScreen.presenter.isVisible;
                break;
        }
    }


    /// <summary>
    ///     Event handler for when the plugin is told to draw the UI.
    /// </summary>
    private protected void DrawUI()
    {
        listScreen.Draw();
        settingsScreen.Draw();
        dutyInfoScreen.Draw();
        editorScreen.Draw();
    }


    /// <summary>
    ///     Event handler for when the UI is told to draw the config UI (Dalamud settings button)
    /// </summary>
    private protected void DrawConfigUI() => settingsScreen.presenter.isVisible = !settingsScreen.presenter.isVisible;

}
