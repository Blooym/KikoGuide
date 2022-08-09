namespace KikoGuide.UI.Screens.Editor;

using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Internal.Notifications;
using KikoGuide.Base;
using KikoGuide.Types;

sealed public class EditorPresenter : IDisposable
{
    public EditorPresenter() { }
    public void Dispose() { }

    /// <summary> If the associated screen should be drawn. </summary>
    public bool isVisible = false;


    /// <summary> An instance of the FileDialogManager for loading/saving duties. </summary>
    public FileDialogManager dialogManager = new FileDialogManager();

    /// <summary> The character limit for the input text fields, applies to the UI and file loading. </summary>
    public uint characterLimit = 35000;

    /// <summary> Handles the file select event </summary>
    public string OnFileSelect(bool success, string file, string text)
    {
        if (!success) return text;
        var fileText = File.ReadAllText(file);

        // If the length was zero, it likely means they cancelled the dialog or the file was empty.
        if (fileText.Length == 0) return text;

        // Reject loading if the file length is beyond the character limit.
        if (fileText.Length > this.characterLimit)
        {
            PluginService.PluginInterface.UiBuilder.AddNotification(TStrings.EditorFileTooLarge(), PStrings.pluginName, NotificationType.Error);
            return text;
        }

        PluginService.PluginInterface.UiBuilder.AddNotification(TStrings.EditorFileSuccessfullyLoaded(), PStrings.pluginName, NotificationType.Success);
        return fileText;
    }

    /// <summary> Handles the file save event. </summary>
    public void OnFileSave(bool success, string file, string text)
    {
        if (!success) return;
        text = this.OnFormat(text);
        File.WriteAllText(file, text);
        PluginService.PluginInterface.UiBuilder.AddNotification(TStrings.EditorFileSuccessfullySaved(), PStrings.pluginName, NotificationType.Success);
    }

    /// <summary> Formats the given text into a better layout. </summary>
    public string OnFormat(string text)
    {
        try
        {
            var newLines = new List<string>();
            foreach (var line in text.Split('\n')) if (line.Trim().Length > 0) newLines.Add(line);
            return string.Join("\n", newLines).Trim().Replace("\t", "    ");
        }
        catch { return text; }
    }


    /// <summary> The last parse result from this.ParseDuty() </summary>
    private Tuple<Duty?, Exception?>? _lastParseResult;

    /// <summary> THe last parsed dutyText for this.ParseDuty(), used to prevent consistently deserializing. </summary>
    private string _parsedDutyText = "";

    /// <summary> Parses the given dutyText into a Duty object or returns an Exception. </summary>
    public Tuple<Duty?, Exception?> ParseDuty(string dutyText)
    {
        if (dutyText == this._parsedDutyText && this._lastParseResult != null) return this._lastParseResult;
        this._parsedDutyText = dutyText;

        try
        {
            this._lastParseResult = new Tuple<Duty?, Exception?>(JsonConvert.DeserializeObject<Duty>(dutyText), null);
            return this._lastParseResult;
        }

        catch (Exception e)
        {
            this._lastParseResult = new Tuple<Duty?, Exception?>(null, e);
            return this._lastParseResult;
        }
    }
}