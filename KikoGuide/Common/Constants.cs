using System;
using System.Reflection;
using CheapLoc;

namespace KikoGuide.Common
{
    /// <summary>
    /// Compile-time constants, readonly values, and localization strings.
    /// </summary>
    internal static class Constants
    {
        public const string PluginName = "Kiko Guide";
        public static readonly string NotesDirectory = $@"{Services.PluginInterface.GetPluginConfigDirectory()}\Notes";
        public static readonly string CustomGuidesDirectory = $@"{Services.PluginInterface.GetPluginConfigDirectory()}\Guides";
        public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);
        public static readonly string GitCommitHash = Assembly.GetExecutingAssembly().GetCustomAttribute<GitHashAttribute>()?.Value ?? "Unknown";
        public static readonly DateTime GitCommitDate = DateTime.TryParse(Assembly.GetExecutingAssembly().GetCustomAttribute<GitCommitDateAttribute>()?.Value, out var date) ? date : DateTime.MinValue;
        public static readonly string GitBranch = Assembly.GetExecutingAssembly().GetCustomAttribute<GitBranchAttribute>()?.Value ?? "Unknown";

        internal static class Commands
        {
            public const string GuideList = "/kikolist";
            public static string GuideListHelp => Loc.Localize("Commands.GuideList.Help", "Toggle a list of all available guides.");
            public const string GuideViewer = "/kiko";
            public static string GuideViewerHelp => Loc.Localize("Commands.GuideViewer.Help", "Toggle the guide viewer window.");
            public const string GuideEditor = "/kikoeditor";
            public static string GuideEditorHelp => Loc.Localize("Commands.GuideEditor.Help", "Toggle the guide editor window.");
            public const string Settings = "/kikosettings";
            public static string SettingsHelp => Loc.Localize("Commands.Settings.Help", "Toggle the settings window.");
        }

        internal static class ExceptionMessages
        {
            public const string InvalidDutyOrUnlockQuest = "The given duty ID or unlock quest ID is invalid and could not be found in the game data.";
        }

        internal static class Links
        {
            public const string GitHub = "https://github.com/BitsOfAByte/KikoGuide";
            public const string KoFi = "https://ko-fi.com/BitsOfAByte";
        }

        internal static class Windows
        {
            public static string SettingsTitle => string.Format(Loc.Localize("Windows.Settings", "{0} - Settings"), PluginName);
            public static string GuideListTitle => string.Format(Loc.Localize("Windows.GuideList", "{0} - Guide List"), PluginName);
            public static string GuideViewerTitle => string.Format(Loc.Localize("Windows.GuideViewer", "{0} - Guide Viewer"), PluginName);
            public static string GuideEditorTitle => string.Format(Loc.Localize("Windows.GuideEditor", "{0} - Guide Editor"), PluginName);
        }

        internal static class Common
        {

        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    internal sealed class GitHashAttribute : Attribute
    {
        public string Value { get; set; }
        public GitHashAttribute(string value) => this.Value = value;
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    internal sealed class GitCommitDateAttribute : Attribute
    {
        public string Value { get; set; }
        public GitCommitDateAttribute(string value) => this.Value = value;
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    internal sealed class GitBranchAttribute : Attribute
    {
        public string Value { get; set; }
        public GitBranchAttribute(string value) => this.Value = value;
    }
}
