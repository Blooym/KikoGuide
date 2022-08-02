namespace KikoGuide.UI.Editor;

using System.IO;
using System;
using System.Collections.Generic;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Internal.Notifications;
using KikoGuide.Managers;
using KikoGuide.Base;

class EditorPresenter : IDisposable
{
    public EditorPresenter() { }
    public void Dispose() { }

    public bool isVisible = false;
    public FileDialogManager dialogManager = new FileDialogManager();

    /// <summary>
    ///     Handles the file select event
    /// </summary>
    public string OnFileSelect(bool success, string file, string text)
    {
        if (!success) return text;
        var fileText = File.ReadAllText(file);
        if (fileText.Length == 0) return text;
        Service.PluginInterface.UiBuilder.AddNotification("File loaded successfully.", PStrings.pluginName, NotificationType.Success);
        return fileText;
    }


    /// <summary>
    ///     Handles the file save event.
    /// </summary>
    public void OnFileSave(bool success, string file, string text)
    {
        if (!success) return;
        File.WriteAllText(file, text);
        Service.PluginInterface.UiBuilder.AddNotification("File saved successfully.", PStrings.pluginName, NotificationType.Success);
    }


    /// <summary>
    ///     Formats the given text into an acceptable format for submission.
    /// </summary>
    public string OnFormat(string text)
    {
        try
        {
            var newLines = new List<string>();
            foreach (var line in text.Split('\n'))
            {
                if (line.Trim().Length > 0)
                {
                    newLines.Add(line);
                }
            }
            return string.Join("\n", newLines).Trim();

        }
        catch { return text; }
    }


    /// <summary> The last parse result from this.ParseDuty() </summary>
    private Tuple<Duty?, Exception?>? _lastParseResult;

    /// <summary> THe last parsed dutyText for this.ParseDuty(), used to prevent consistently deserializing. </summary>
    private string _parsedDutyText = "";

    /// <summary>
    ///     Parses the given dutyText into a Duty object or returns an Exception.
    /// </summary>
    public Tuple<Duty?, Exception?> ParseDuty(string dutyText)
    {
        if (dutyText == this._parsedDutyText && this._lastParseResult != null) return this._lastParseResult;
        this._parsedDutyText = dutyText;

        try
        {
            this._lastParseResult = new Tuple<Duty?, Exception?>(Newtonsoft.Json.JsonConvert.DeserializeObject<Duty>(dutyText), null);
            return this._lastParseResult;
        }

        catch (Exception e)
        {
            this._lastParseResult = new Tuple<Duty?, Exception?>(null, e);
            return this._lastParseResult;
        }
    }
}