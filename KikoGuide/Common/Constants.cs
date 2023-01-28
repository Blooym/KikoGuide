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
        internal const string PluginName = "Kiko Guide";
        internal static readonly string NotesDirectory = $@"{Services.PluginInterface.GetPluginConfigDirectory()}\Notes";
        internal static readonly string IntegrationsDirectory = $@"{Services.PluginInterface.GetPluginConfigDirectory()}\Integrations";
        internal static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);
        internal static readonly string GitCommitHash = Assembly.GetExecutingAssembly().GetCustomAttribute<GitHashAttribute>()?.Value ?? "Unknown";
        internal static readonly DateTime GitCommitDate = DateTime.TryParse(Assembly.GetExecutingAssembly().GetCustomAttribute<GitCommitDateAttribute>()?.Value, out var date) ? date : DateTime.MinValue;
        internal static readonly string GitBranch = Assembly.GetExecutingAssembly().GetCustomAttribute<GitBranchAttribute>()?.Value ?? "Unknown";

        internal static class Commands
        {
            internal const string GuideList = "/kikolist";
            internal const string GuideViewer = "/kiko";
        }

        internal static class Links
        {
            internal const string GitHub = "https://github.com/BitsOfAByte/KikoGuide";
            internal const string KoFi = "https://ko-fi.com/BitsOfAByte";
        }

        internal static class Windows
        {
            internal static string SettingsTitle => string.Format(Strings.Settings_Window_Title, PluginName);
            internal static string GuideListTitle => string.Format(Strings.Gudie_List_Window_Title, PluginName);
            internal static string GuideViewerTitle => string.Format(Strings.Guide_Viewer_Window_Title, PluginName);
            internal static string IntegrationSettingsTitle => string.Format("[{0}] Integration Settings", PluginName);
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
