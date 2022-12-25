using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Internal.Notifications;
using Dalamud.Utility;
using KikoGuide.Base;
using KikoGuide.Localization;
using KikoGuide.Types;
using KikoGuide.Utils;

namespace KikoGuide.UI.Windows.Editor
{
    internal sealed class EditorPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Pulls the configuration from the plugin service.
        /// </summary>
        internal static Configuration Configuration => PluginService.Configuration;

        /// <summary>
        ///     Opens the contributing guide.
        /// </summary>
        internal static void OpenContributingGuide() => Util.OpenLink($"{PluginConstants.RepoUrl}blob/main/CONTRIBUTING.md#guide-contribution");

        /// <summary>
        ///     Gets the players current territory.
        /// </summary>
        internal static uint GetPlayerTerritory => PluginService.ClientState.TerritoryType;

        /// <summary>
        ///     An instance of the FileDialogManager for loading/saving guides.
        /// </summary>
        internal FileDialogManager DialogManager = new();

        /// <summary>
        ///     The character limit for the input text fields, applies to the UI and file loading.
        /// </summary>
        internal uint CharacterLimit = 35000;

        /// <summary>
        ///     Handles the file select event
        /// </summary>
        /// <param name="cancelled"> Whether the file selection was cancelled or not. </param>
        /// <param name="file"> The file that was selected. </param>
        /// <param name="text"> The text that was loaded from the file. </param>
        internal string OnFileSelect(bool cancelled, string file, string text)
        {
            if (!cancelled)
            {
                return text;
            }

            var fileText = File.ReadAllText(file);

            // If the length was zero, it likely means they cancelled the dialog or the file was empty.
            if (fileText.Length == 0)
            {
                return text;
            }

            // Reject loading if the file length is beyond the character limit.
            if (fileText.Length > this.CharacterLimit)
            {
                Notifications.ShowToast(message: TEditor.FileTooLarge, type: NotificationType.Error);
                return text;
            }

            Notifications.ShowToast(message: TEditor.FileSuccessfullyLoaded, type: NotificationType.Success);
            return fileText;
        }

        /// <summary>
        ///     Handles the file save event.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="file"></param>
        /// <param name="text"></param>
        internal static void OnFileSave(bool success, string file, string text)
        {
            if (!success)
            {
                return;
            }

            text = OnFormat(text);
            File.WriteAllText(file, text);
            Notifications.ShowToast(message: TEditor.FileSuccessfullySaved, type: NotificationType.Success);
        }

        /// <summary>
        ///     Formats the given text into a better layout.
        /// </summary>
        /// <param name="text"></param>
        internal static string OnFormat(string text)
        {
            try
            {
                List<string> newLines = new();
                foreach (var line in text.Split('\n'))
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
        ///     The last parse result from this.ParseGuide()
        /// </summary>
        private Tuple<Guide?, Exception?>? lastParseResult;

        /// <summary>
        ///     The last parsed guideText for this.ParseGuide(), used to prevent consistently deserializing.
        /// </summary>
        private string parsedGuideText = "";

        /// <summary>
        ///     Parses the given guideText into a Guide object or returns an Exception.
        /// </summary>
        /// <param name="guideText"></param>
        internal Tuple<Guide?, Exception?> ParseGuide(string guideText)
        {
            if (guideText == this.parsedGuideText && this.lastParseResult != null)
            {
                return this.lastParseResult;
            }

            this.parsedGuideText = guideText;

            try
            {
                this.lastParseResult = new Tuple<Guide?, Exception?>(Guide.FromJson(guideText), null);
                return this.lastParseResult;
            }
            catch (Exception e)
            {
                this.lastParseResult = new Tuple<Guide?, Exception?>(null, e);
                return this.lastParseResult;
            }
        }
    }
}
