namespace KikoGuide;

using System;
using System.IO;
using Dalamud.IoC;
using Dalamud.Plugin;
using KikoGuide.UI.Screens.DutyList;
using KikoGuide.UI.Screens.Editor;
using KikoGuide.UI.Screens.Settings;
using KikoGuide.UI.Screens.DutyInfo;
using KikoGuide.Base;
using KikoGuide.Managers;
using CheapLoc;

internal class KikoPlugin : IDalamudPlugin
{
    public string Name => PStrings.pluginName;
    public static ListScreen listScreen = new ListScreen();
    public static SettingsScreen settingsScreen = new SettingsScreen();
    public static DutyInfoScreen dutyInfoScreen = new DutyInfoScreen();
    public static EditorScreen editorScreen = new EditorScreen();
    public static CommandManager commands = new CommandManager();

    public KikoPlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();
        Service.Initialize(Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration());
        OnLanguageChange(Service.PluginInterface.UiLanguage);
        commands.Initialize();

#if !DEBUG
        // Update resources if not running a debug build.
        UpdateManager.UpdateResources();
#endif

        // Register event handlers
        Service.PluginInterface.UiBuilder.Draw += DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        Service.PluginInterface.LanguageChanged += OnLanguageChange;
        Service.ClientState.Logout += OnLogout;
        UpdateManager.ResourcesUpdated += DutyManager.OnResourceUpdate;
    }

    ///<summary> Handles disposing of all resources used by the plugin. </summary>
    public void Dispose()
    {
        listScreen.Dispose();
        settingsScreen.Dispose();
        dutyInfoScreen.Dispose();
        editorScreen.Dispose();
        commands.Dispose();
        UpdateManager.ResourcesUpdated -= DutyManager.OnResourceUpdate;
        Service.ClientState.Logout -= OnLogout;
        Service.PluginInterface.LanguageChanged -= OnLanguageChange;
        Service.PluginInterface.UiBuilder.Draw -= DrawUI;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
    }

    /// <summary>  Event handler for the client is logging out. </summary>
    public static void OnLogout(object? sender, EventArgs e)
    {
        dutyInfoScreen.presenter.selectedDuty = null;
        listScreen.presenter.isVisible = false;
        dutyInfoScreen.presenter.isVisible = false;
        editorScreen.presenter.isVisible = false;
    }


    /// <summary> Event handler for when the language is changed, reloads the localization strings. </summary>
    public static void OnLanguageChange(string language)
    {
        var uiLang = Service.PluginInterface.UiLanguage;

        DutyManager.OnResourceUpdate();

        try { Loc.Setup(File.ReadAllText($"{PStrings.localizationPath}\\Plugin\\{uiLang}.json")); }
        catch { Loc.SetupWithFallbacks(); }
    }


    /// <summary> Event handler for when the plugin is told to draw the UI. </summary>
    private protected void DrawUI()
    {
        listScreen.Draw();
        settingsScreen.Draw();
        dutyInfoScreen.Draw();
        editorScreen.Draw();
    }

    /// <summary> Event handler for when the UI is told to draw the config UI (Dalamud settings button) </summary>
    private protected void DrawConfigUI() => settingsScreen.presenter.isVisible = !settingsScreen.presenter.isVisible;
}
