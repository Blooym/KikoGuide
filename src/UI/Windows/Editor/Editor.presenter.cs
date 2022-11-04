using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Internal.Notifications;
using Dalamud.Utility;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Types;
using Newtonsoft.Json;

namespace KikoGuide.UI.Windows.Editor
{
    public sealed class EditorPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration Configuration => PluginService.Configuration;

        /// <summary>
        ///     Opens the contributing guide.
        /// </summary>
        public static void OpenContributingGuide()
        {
            Util.OpenLink($"{PluginConstants.repoUrl}blob/main/CONTRIBUTING.md#guide-contribution");
        }

        /// <summary>
        ///     Gets the players current territory.
        /// </summary>
        public static uint GetPlayerTerritory => PluginService.ClientState.TerritoryType;

        /// <summary>
        ///     An instance of the FileDialogManager for loading/saving duties.
        /// </summary>
        public FileDialogManager dialogManager = new();

        /// <summary> 
        ///     The character limit for the input text fields, applies to the UI and file loading.
        /// </summary>
        public uint characterLimit = 35000;

        /// <summary> 
        ///     Handles the file select event
        /// </summary>
        public string OnFileSelect(bool success, string file, string text)
        {
            if (!success)
            {
                return text;
            }

            string fileText = File.ReadAllText(file);

            // If the length was zero, it likely means they cancelled the dialog or the file was empty.
            if (fileText.Length == 0)
            {
                return text;
            }

            // Reject loading if the file length is beyond the character limit.
            if (fileText.Length > characterLimit)
            {
                PluginService.PluginInterface.UiBuilder.AddNotification(TStrings.EditorFileTooLarge, PluginConstants.pluginName, NotificationType.Error);
                return text;
            }

            PluginService.PluginInterface.UiBuilder.AddNotification(TStrings.EditorFileSuccessfullyLoaded, PluginConstants.pluginName, NotificationType.Success);
            return fileText;
        }

        /// <summary>
        ///     Handles the file save event.
        /// </summary>
        public static void OnFileSave(bool success, string file, string text)
        {
            if (!success)
            {
                return;
            }

            text = OnFormat(text);
            File.WriteAllText(file, text);
            PluginService.PluginInterface.UiBuilder.AddNotification(TStrings.EditorFileSuccessfullySaved, PluginConstants.pluginName, NotificationType.Success);
        }

        /// <summary>
        ///     Formats the given text into a better layout.
        /// </summary>
        public static string OnFormat(string text)
        {
            try
            {
                List<string> newLines = new();
                foreach (string line in text.Split('\n'))
                {
                    if (line.Trim().Length > 0)
                    {
                        newLines.Add(line);
                    }
                }

                return string.Join("\n", newLines).Trim().Replace("\t", "    ");
            }
            catch { return text; }
        }

        /// <summary>
        ///     The last parse result from this.ParseDuty()
        /// </summary>
        private Tuple<Duty?, Exception?>? _lastParseResult;

        /// <summary> 
        ///     The last parsed dutyText for this.ParseDuty(), used to prevent consistently deserializing. 
        /// </summary>
        private string _parsedDutyText = "";

        /// <summary> 
        ///     Parses the given dutyText into a Duty object or returns an Exception.
        /// </summary>
        public Tuple<Duty?, Exception?> ParseDuty(string dutyText)
        {
            if (dutyText == _parsedDutyText && _lastParseResult != null)
            {
                return _lastParseResult;
            }

            _parsedDutyText = dutyText;

            try
            {
                _lastParseResult = new Tuple<Duty?, Exception?>(JsonConvert.DeserializeObject<Duty>(dutyText), null);
                return _lastParseResult;
            }

            catch (Exception e)
            {
                _lastParseResult = new Tuple<Duty?, Exception?>(null, e);
                return _lastParseResult;
            }
        }
    }
}