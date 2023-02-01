using System;
using System.Reflection;
using KikoGuide.Resources.Localization;

namespace KikoGuide.Common
{
    /// <summary>
    /// Constants and other static data.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        internal const string PluginName = "Kiko Guide";

        /// <summary>
        /// Build information.
        /// </summary>
        internal static class Build
        {
            internal static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);
            internal static readonly string GitCommitHash = Assembly.GetExecutingAssembly().GetCustomAttribute<GitHashAttribute>()?.Value ?? "Unknown";
            internal static readonly DateTime GitCommitDate = DateTime.TryParse(Assembly.GetExecutingAssembly().GetCustomAttribute<GitCommitDateAttribute>()?.Value, out var date) ? date : DateTime.MinValue;
            internal static readonly string GitBranch = Assembly.GetExecutingAssembly().GetCustomAttribute<GitBranchAttribute>()?.Value ?? "Unknown";
        }

        /// <summary>
        /// Directory paths.
        /// </summary>
        internal static class Directory
        {
            internal static readonly string Notes = $@"{Services.PluginInterface.GetPluginConfigDirectory()}\Notes";
            internal static readonly string Integrations = $@"{Services.PluginInterface.GetPluginConfigDirectory()}\IntegrationConfigs";
            internal static readonly string Guides = $@"{Services.PluginInterface.GetPluginConfigDirectory()}\GuideConfigs";
        }

        /// <summary>
        /// Commands that are registered in the <see cref="CommandHandling.CommandManager"/>
        /// </summary>
        internal static class Commands
        {
            internal const string GuideList = "/kiko";
            internal const string GuideViewer = "/kikoview";
        }

        /// <summary>
        /// Common links used in multiple places.
        /// </summary>
        internal static class Links
        {
            internal const string GitHub = "https://github.com/BitsOfAByte/KikoGuide";
            internal const string Crowdin = "https://crowdin.com/project/kikoguide";
            internal const string KoFi = "https://ko-fi.com/BitsOfAByte";
        }

        /// <summary>
        /// Strings used in for windows.
        /// </summary>
        internal static class WindowTitles
        {
            internal static string Settings => string.Format(Strings.Windows_PluginSettings_Title, PluginName);
            internal static string GuideList => string.Format(Strings.Windows_GuideList_Title, PluginName);
            internal static string GuideViewer => string.Format(Strings.Windows_GuideViewer_Title, PluginName);
            internal static string IntegrationsTitle => string.Format(Strings.Windows_Integrations_Title, PluginName);
            internal static string GuideTypeSettings => string.Format(Strings.Windows_GuideSettings_Title, PluginName);
        }
    }

    /// <summary>
    /// The Git commit hash of the build.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    internal sealed class GitHashAttribute : Attribute
    {
        public string Value { get; set; }
        public GitHashAttribute(string value) => this.Value = value;
    }

    /// <summary>
    /// The Git commit date of the build.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    internal sealed class GitCommitDateAttribute : Attribute
    {
        public string Value { get; set; }
        public GitCommitDateAttribute(string value) => this.Value = value;
    }

    /// <summary>
    /// The Git branch of the build.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    internal sealed class GitBranchAttribute : Attribute
    {
        public string Value { get; set; }
        public GitBranchAttribute(string value) => this.Value = value;
    }
}