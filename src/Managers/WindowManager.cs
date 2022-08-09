namespace KikoGuide.Managers;

using System;
using Dalamud.Logging;
using KikoGuide.Base;
using KikoGuide.UI.Screens.DutyInfo;
using KikoGuide.UI.Screens.DutyList;
using KikoGuide.UI.Screens.Editor;
using KikoGuide.UI.Screens.Settings;

/// <summary> Initializes and manages all windows and window-events for the plugin. </summary>
sealed public class WindowManager : IDisposable
{
    public readonly DutyInfoScreen DutyInfo = new DutyInfoScreen();
    public readonly DutyListScreen DutyList = new DutyListScreen();
    public readonly EditorScreen Editor = new EditorScreen();
    public readonly SettingsScreen Settings = new SettingsScreen();


    /// <summary> Handles the ClientState.Logout event by hiding all screens </summary>
    private void OnLogout(object? sender, EventArgs e) => HideAll();


    /// <summary> Draws all windows for the draw event. </summary>
    private void OnDraw()
    {
        DutyInfo.Draw();
        DutyList.Draw();
        Editor.Draw();
        Settings.Draw();
    }


    /// <summary> Opens/Closes the plugin configuration screen. </summary> 
    private void OnOpenConfigUI() => Settings.presenter.isVisible = !Settings.presenter.isVisible;


    /// <summary> Initializes the WindowManager and associated resources. </summary>
    public WindowManager()
    {
        PluginLog.Debug("WindowManager: Initializing...");

        PluginService.PluginInterface.UiBuilder.Draw += OnDraw;
        PluginService.PluginInterface.UiBuilder.OpenConfigUi += OnOpenConfigUI;
        PluginService.ClientState.Logout += OnLogout;

        PluginLog.Debug("WindowManager: Initialization complete.");
    }


    /// <summary> Disposes of the WindowManager and associated resources. </summary>
    public void Dispose()
    {
        PluginLog.Debug("WindowManager: Disposing...");

        PluginService.PluginInterface.UiBuilder.Draw -= OnDraw;
        PluginService.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUI;
        PluginService.ClientState.Logout -= OnLogout;

        DutyInfo.Dispose();
        DutyList.Dispose();
        Editor.Dispose();
        Settings.Dispose();

        PluginLog.Debug("WindowManager: Successfully disposed.");
    }


    /// <summary> Hides all screens. </summary>
    public void HideAll()
    {
        DutyInfo.Hide();
        DutyList.Hide();
        Editor.Hide();
        Settings.Hide();
    }
}