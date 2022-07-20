namespace KikoGuide;

using CheapLoc;
using Dalamud.Game.Command;
using Dalamud.Logging;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using KikoGuide.UI;
using KikoGuide.Utils;
using KikoGuide.Base;

internal class KikoPlugin : IDalamudPlugin
{
    // Initialize some plugin settings & commands
    public string Name => PStrings.PluginName;

    private protected const string listCommand = "/kikolist";
    private protected const string settingsCommand = "/kikoconfig";
    private protected Configuration _configuration { get; init; }
    private protected DalamudPluginInterface _pluginInterface { get; init; }

    // Initialize an instance of each user interface so it can be drawn.
    private protected KikoUIList _kikoUIList { get; init; }
    private protected KikoUISettings _kikoUISettings { get; init; }
    private protected KikoUIDutyInfo _kikoUIFrame { get; init; }

    public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();
        this._pluginInterface = pluginInterface;

        this._configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        FFXIVClientStructs.Resolver.Initialize(Service.Scanner.SearchBase);
        Service.Initialize(_configuration);
        OnLanguageChange(Service.PluginInterface.UiLanguage);

        // UI instances
        this._kikoUIList = new KikoUIList(this._configuration);
        this._kikoUISettings = new KikoUISettings(this._configuration);
        this._kikoUIFrame = new KikoUIDutyInfo(this._configuration);

        // Event handlers.
        Service.PluginInterface.UiBuilder.Draw += DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        Service.PluginInterface.LanguageChanged += OnLanguageChange;
        Service.ClientState.TerritoryChanged += KikoUIState.OnTerritoryChange;

        // Register commands.
        Service.Commands.AddHandler(listCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.List.Help", "Opens the duty list"),
        });

        Service.Commands.AddHandler(settingsCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = Loc.Localize("Commands.Settings.Help", "Opens the settings menu"),
        });
    }

    // <summary>
    // Handles disposing of all resources used by the plugin.
    // </summary>
    public void Dispose()
    {
        this._kikoUIList.Dispose();
        this._kikoUISettings.Dispose();
        this._kikoUIFrame.Dispose();
        Service.Commands.RemoveHandler(listCommand);
        Service.Commands.RemoveHandler(settingsCommand);
        Service.ClientState.TerritoryChanged -= KikoUIState.OnTerritoryChange;
        Service.PluginInterface.LanguageChanged -= OnLanguageChange;
        Service.PluginInterface.UiBuilder.Draw -= DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
    }


    // <summary>
    // This method executes when a command is issued by the user (within chat).
    // </summary>
    private protected void OnCommand(string command, string args)
    {
        switch (command)
        {
            case listCommand:
                KikoUIState.listVisible = !KikoUIState.listVisible;
                break;
            case settingsCommand:
                KikoUIState.settingsVisible = !KikoUIState.settingsVisible;
                break;
        }
    }


    // <summary>
    // Draws UI is responsible for handling the drawing of top-level UI elements.
    // </summary>
    private protected void DrawUI()
    {
        this._kikoUIList.Draw();
        this._kikoUISettings.Draw();
        this._kikoUIFrame.Draw();
    }


    // <summary>
    // This method handles drawing the configuration UI when the dalamud settings button is pressed.
    // </summary>
    private protected void DrawConfigUI()
    {
        KikoUIState.settingsVisible = !KikoUIState.settingsVisible;
    }

    internal static void OnLanguageChange(string language)
    {

        var uiLang = Service.PluginInterface.UiLanguage;
        PluginLog.Debug("Trying to set up Loc for culture {0}", uiLang);

        try { Loc.Setup(File.ReadAllText($"{FS.resourcePath}\\Localization\\Plugin\\{uiLang}.json")); }
        catch { Loc.SetupWithFallbacks(); }
    }
}
