namespace KikoGuide.Base;

using System;
using Dalamud.Logging;
using KikoGuide.UI.Screens.DutyInfo;
using KikoGuide.UI.Screens.DutyList;
using KikoGuide.UI.Screens.Editor;
using KikoGuide.UI.Screens.Settings;

/// <summary> Initializes and manages all windows and window-events for the plugin. </summary>
public static class PluginWindowManager
{
    public static readonly DutyInfoScreen DutyInfo = new DutyInfoScreen();
    public static readonly DutyListScreen DutyList = new DutyListScreen();
    public static readonly EditorScreen Editor = new EditorScreen();
    public static readonly SettingsScreen Settings = new SettingsScreen();


    /// <summary> Handles the ClientState.Logout event by hiding all screens </summary>
    private static void OnLogout(object? sender, EventArgs e) => HideAll();


    /// <summary> Draws all windows for the draw event. </summary>
    private static void OnDraw()
    {
        DutyInfo.Draw();
        DutyList.Draw();
        Editor.Draw();
        Settings.Draw();
    }


    /// <summary> Opens/Closes the plugin configuration screen. </summary> 
    private static void OnOpenConfigUI() => Settings.presenter.isVisible = !Settings.presenter.isVisible;


    /// <summary> Initializes the PluginWindowManager and associated resources. </summary>
    public static void Initialize()
    {
        PluginLog.Debug("PluginWindowManager: Initializing...");

        PluginService.PluginInterface.UiBuilder.Draw += OnDraw;
        PluginService.PluginInterface.UiBuilder.OpenConfigUi += OnOpenConfigUI;
        PluginService.ClientState.Logout += OnLogout;

        PluginLog.Debug("PluginWindowManager: Successfully initialized.");
    }


    /// <summary> Disposes of the PluginWindowManager and associated resources. </summary>
    public static void Dispose()
    {
        PluginLog.Debug("PluginWindowManager: Disposing...");

        PluginService.PluginInterface.UiBuilder.Draw -= OnDraw;
        PluginService.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUI;
        PluginService.ClientState.Logout -= OnLogout;

        DutyInfo.Dispose();
        DutyList.Dispose();
        Editor.Dispose();
        Settings.Dispose();

        PluginLog.Debug("PluginCommandManager: Successfully disposed.");
    }


    /// <summary> Hides all screens. </summary>
    public static void HideAll()
    {
        DutyInfo.Hide();
        DutyList.Hide();
        Editor.Hide();
        Settings.Hide();
    }
}