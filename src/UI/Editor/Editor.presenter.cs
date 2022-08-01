namespace KikoGuide.UI.Editor;

using Dalamud.Interface.ImGuiFileDialog;
using System.IO;
using System.Collections.Generic;

class EditorPresenter
{
    public bool isVisible = false;
    public FileDialogManager dialogManager = new FileDialogManager();
    public string previewDutyText = "";


    /// <summary>
    ///     Handles the file select event
    /// </summary>
    public void OnFileSelect(bool success, string file)
    {
        if (!success) return;

        var loadedText = File.ReadAllText(file);
        previewDutyText = loadedText;
    }


    /// <summary>
    ///     Handles the file save event.
    /// </summary>
    public void OnFileSave(bool success, string file)
    {
        if (!success) return;
        File.WriteAllText(file, previewDutyText);
    }


    /// <summary>
    ///     Formats the previewDutyText into an acceptable format for submission.
    /// </summary>
    public void OnFormat()
    {
        // Remove all empty rows.
        var newLines = new List<string>();
        foreach (var line in previewDutyText.Split('\n'))
        {
            if (line.Trim().Length > 0)
            {
                newLines.Add(line);
            }
        }
        previewDutyText = string.Join("\n", newLines);
    }
}