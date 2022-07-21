namespace KikoGuide;

using CheapLoc;
using System.IO;
using Dalamud.Game.Command;
using Dalamud.Logging;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.UI;
using KikoGuide.Base;
using KikoGuide.Managers;

internal class KikoPlugin : IDalamudPlugin
{
    public string Name => PStrings.pluginName;
    private protected const string listCommand = "/kikolist";
    private protected const string settingsCommand = "/kikoconfig";
    private protected Configuration _configuration { get; init; }

    private protected List _kikoUIList { get; init; }
    private protected Settings _kikoUISettings { get; init; }
    private protected DutyInfo _kikoUIFrame { get; init; }

    public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        // Inject/Create all services
        pluginInterface.Create<Service>();

        // Load the configuration file
        this._configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        // Initialize everything & load trigger any events
        FFXIVClientStructs.Resolver.Initialize(Service.Scanner.SearchBase);
        Service.Initialize(_configuration);
        OnLanguageChange(Service.PluginInterface.UiLanguage);
        UpdateManager.UpdateResources();

        // Create UI Instances
        this._kikoUIList = new List(this._configuration);
        this._kikoUISettings = new Settings(this._configuration);
        this._kikoUIFrame = new DutyInfo(this._configuration);

        // Register event handlers
        Service.PluginInterface.UiBuilder.Draw += DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        Service.PluginInterface.LanguageChanged += OnLanguageChange;
        Service.ClientState.TerritoryChanged += UIState.OnTerritoryChange;
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
    }


    ///<summary>
    ///     Handles disposing of all resources used by the plugin.
    /// </summary>
    public void Dispose()
    {
        this._kikoUIList.Dispose();
        this._kikoUISettings.Dispose();
        this._kikoUIFrame.Dispose();
        Service.Commands.RemoveHandler(listCommand);
        Service.Commands.RemoveHandler(settingsCommand);
        Service.ClientState.TerritoryChanged -= UIState.OnTerritoryChange;
        UpdateManager.ResourcesUpdated -= DutyManager.OnResourceUpdate;
        Service.PluginInterface.LanguageChanged -= OnLanguageChange;
        Service.PluginInterface.UiBuilder.Draw -= DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
    }


    /// <summary>
    ///     Event handler for when a command is issued by the user.
    /// </summary>
    private protected void OnCommand(string command, string args)
    {
        switch (command)
        {
            case listCommand:
                UIState.listVisible = !UIState.listVisible;
                break;
            case settingsCommand:
                UIState.settingsVisible = !UIState.settingsVisible;
                break;
        }
    }


    /// <summary>
    ///     Event handler for when the plugin is told to draw the UI.
    /// </summary>
    private protected void DrawUI()
    {
        this._kikoUIList.Draw();
        this._kikoUISettings.Draw();
        this._kikoUIFrame.Draw();
    }


    /// <summary>
    ///     Event handler for when the UI is told to draw the config UI (Dalamud settings button)
    /// </summary>
    private protected void DrawConfigUI() => UIState.settingsVisible = !UIState.settingsVisible;


    /// <summary>
    ///    Event handler for when the language is changed, reloads the localization strings.
    /// </summary>
    internal static void OnLanguageChange(string language)
    {
        var uiLang = Service.PluginInterface.UiLanguage;
        PluginLog.Debug("Trying to set up Loc for culture {0}", uiLang);

        // Tell the duty manager to refresh its resources for the new language
        DutyManager.OnResourceUpdate();

        try { Loc.Setup(File.ReadAllText($"{PStrings.localizationPath}\\Plugin\\{uiLang}.json")); }
        catch { Loc.SetupWithFallbacks(); }
    }
}
