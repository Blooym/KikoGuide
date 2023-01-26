using System;
using System.Reflection;
using KikoGuide.Resources.Localization;

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
            public const string GuideViewer = "/kiko";
            public const string GuideEditor = "/kikoeditor";
            public const string Settings = "/kikosettings";
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
            public static string SettingsTitle => string.Format(Strings.Settings_Window_Title, PluginName);
            public static string GuideListTitle => string.Format(Strings.Gudie_List_Window_Title, PluginName);
            public static string GuideViewerTitle => string.Format(Strings.Guide_Viewer_Window_Title, PluginName);
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
