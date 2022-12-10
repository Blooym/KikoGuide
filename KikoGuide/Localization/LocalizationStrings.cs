using CheapLoc;
using KikoGuide.Base;

namespace KikoGuide.Localization
{
    /// <summary>
    ///     A collection translatable Command Help strings.
    /// </summary>
    internal sealed class TCommands
    {
        internal static string GuideListHelp => Loc.Localize("Commands.DutyList.Help", "Toggles the guide list");
        internal static string SettingsHelp => Loc.Localize("Commands.Settings.Help", "Toggles the settings menu");
        internal static string EditorHelp => Loc.Localize("Commands.Editor.Help", "Toggles the guide editor");
        internal static string InfoHelp => Loc.Localize("Commands.Info.Help", "Toggles the guide viewer window");
    }

    /// <summary>
    ///     A collection of translatable window strings.
    /// </summary>
    internal sealed class TWindowNames
    {
        internal static string Settings => string.Format(Loc.Localize("Window.Settings", "{0} - Settings"), PluginConstants.PluginName);
        internal static string GuideList => string.Format(Loc.Localize("Window.GuideList", "{0} - Guide List"), PluginConstants.PluginName);
        internal static string GuideViewer => string.Format(Loc.Localize("Window.GuideViewer", "{0} - Guide Viewer"), PluginConstants.PluginName);
        internal static string GuideEditor => string.Format(Loc.Localize("Window.GuideEditor", "{0} - Guide Editor"), PluginConstants.PluginName);
    }

    /// <summary>
    ///     Translation strings used in the plugin.
    /// </summary>
    internal static class TGenerics
    {
        internal static string Donate => Loc.Localize("Generics.Donate", "Donate");
        internal static string OpenFile => Loc.Localize("Generics.OpenFile", "Open File");
        internal static string SaveFile => Loc.Localize("Generics.SaveFile", "Save File");
        internal static string Search => Loc.Localize("Generics.Search", "Search");
        internal static string Guide => Loc.Localize("Generics.Guide", "Guide");
        internal static string InTerritory => Loc.Localize("Generics.InTerritory", "In Territory");
        internal static string Mechanic => Loc.Localize("Generics.Mechanic", "Mechanic");
        internal static string Mechanics => Loc.Localize("Generics.Mechanics", "Mechanics");
        internal static string Strategy => Loc.Localize("Generics.Strategy", "Strategy");
        internal static string Note => Loc.Localize("Generics.Note", "Note");
        internal static string Tips => Loc.Localize("Generics.Tips", "Tips");
        internal static string Description => Loc.Localize("Generics.Description", "Description");
        internal static string Level => Loc.Localize("Generics.Level", "Level");
        internal static string Type => Loc.Localize("Generics.Type", "Type");
        internal static string Enabled => Loc.Localize("Generics.Enabled", "Enabled");
        internal static string Disabled => Loc.Localize("Generics.Disabled", "Disabled");
        internal static string Unknown => Loc.Localize("Generics.Unknown", "Unknown");
        internal static string Unspecified => Loc.Localize("Generics.Unspecified", "Unspecified");
        internal static string None => Loc.Localize("Generics.None", "None");
    }

    /// <summary>
    ///    Translation strings used in the Editor.
    /// </summary>
    internal static class TEditor
    {
        internal static string Problems => Loc.Localize("Editor.Problems", "Problems");
        internal static string Preview => Loc.Localize("Editor.Preview", "Preview");
        internal static string Clear => Loc.Localize("Editor.Clear", "Clear");
        internal static string Format => Loc.Localize("Editor.Format", "Format");
        internal static string Metadata => Loc.Localize("Editor.Metadata", "Metadata");
        internal static string EnumList => Loc.Localize("Editor.EnumList", "Enum List");
        internal static string CannotParseNoPreview => Loc.Localize("Editor.CannotParse", "Cannot parse file, unable to preview.");
        internal static string ContributingGuide => Loc.Localize("Editor.ContributingGuide", "Contributing Guide");
        internal static string ProblemUnsupported => Loc.Localize("Editor.Problem.Unsupported", "Guide contains invalid IDs and/or is unsupported by this plugin version.");
        internal static string NoProblems => Loc.Localize("Editor.Problem.None", "No problems detected.");
        internal static string FileTooLarge => Loc.Localize("Editor.FileTooBig", "Your file is too big to be loaded into the editor.");
        internal static string FileSuccessfullyLoaded => Loc.Localize("Editor.FileLoaded", "Your file was successfully loaded.");
        internal static string FileSuccessfullySaved => Loc.Localize("Editor.FileSaved", "Your file was successfully saved.");
    }

    /// <summary>
    ///     Translation strings used in the Settings.
    /// </summary>
    internal static class TSettings
    {
        internal static string Configuration => Loc.Localize("Settings.Configuration", "Configuration");
        internal static string AutoOpenGuideForDuty => Loc.Localize("Settings.AutOpenGuideForDuty", "Auto Open Guide for Duty");
        internal static string SettingsAutoOpenInDutyTooltip => Loc.Localize("Settings.AutoOpenDuty.Tooltip", "Automatically open/hide the guide for the given duty when you enter/leave it.");
        internal static string ShortenGuideText => Loc.Localize("Settings.ShortedGuideText", "Shorten Guide Text");
        internal static string ShortenGuideTextTooltip => Loc.Localize("Settings.ShortenGuideText.Tooltip", "Shorten text in all guides when available to help make them more accessible and save space.");
        internal static string ShowDonateButton => Loc.Localize("Settings.ShowDonateButton", "Show Donate Button");
        internal static string ShowDonateButtonTooltip => Loc.Localize("Settings.ShowDonateButton.Tooltip", "Show donate buttons in the plugin to help support the plugin and its development.");
        internal static string HideLockedGuides => Loc.Localize("Settings.HideLockedGuides", "Hide Locked Guides");
        internal static string HideLockedGuidesTooltip => Loc.Localize("Settings.HideLockedGuides.Tooltip", "Hide guides that you have not unlocked yet from the guide list and prevent opening them in the guide viewer.\nWARNING: Turning this off will show you spoilers for guides you have not unlocked yet.");
        internal static string HiddenMechanics => Loc.Localize("Settings.HiddenMechanics", "Hidden Mechanics");
        internal static string HiddenMechanicsTooltip => Loc.Localize("Settings.HiddenMechanics.Tooltip", "Hide mechanic types, preventing them from being shown in any guides.");
        internal static string EnabledIntegrations => Loc.Localize("Settings.EnabledIntegrations", "Enabled Integrations");
        internal static string EnabledIntegrationsTooltip => Loc.Localize("Settings.EnabledIntegrations.Tooltip", "Enable or disable integrations with other plugins. Please note that the plugin(s) must be installed for the integration to work.");
    }

    /// <summary>
    ///     Translation strings used in the Guide viewer window.
    /// </summary>
    internal static class TGuideViewer
    {
        internal static string NoGuideInfoAvailable => Loc.Localize("GuideViewer.NoGuideInfoAvailable", "There is no information available for this guide right now or the guide is not supported by this plugin version.");
        internal static string NoPhaseInfoAvailable => Loc.Localize("GuideViewer.NoPhaseInfoAvailable", "There is no information available for this phase right now.");
        internal static string ReportIssueWithGuide => Loc.Localize("GuideViewer.ReportIssueWithGuide", "Report issue with guide");
        internal static string UnlockWindowMovement => Loc.Localize("GuideViewer.UnlockWindowMovement", "Unlock window");
        internal static string LockWindowMovement => Loc.Localize("GuideViewer.LockWindowMovement", "Lock window");
        internal static string ToggleSettingsWindow => Loc.Localize("GuideViewer.ToggleSettingsWindow", "Toggle settings window");
        internal static string GuideHeading(string guideName) => string.Format(Loc.Localize("GuideViewer.GuideHeading", "Guide for {0}"), guideName);
        internal static string NoGuideSelected => Loc.Localize("GuideViewer.NoGuideSelected", "No guide selected, use /kikolist to see all available guides.");
        internal static string GuideNotUnlocked => Loc.Localize("GuideViewer.GuideInfoNotUnlocked", "You cannot view the guide for this duty as you have not unlocked it yet.");
        internal static string GuideAvailableForDuty => Loc.Localize("GuideViewer.GuideAvailableForDuty", "A guide is available for this duty, use /kikoinfo to view it.");
        internal static string Lore => Loc.Localize("GuideViewer.Lore", "Lore");
    }

    /// <summary>
    ///     Translation strings used in the guide list window.
    /// </summary>
    internal static class TGuideListTable
    {
        internal static string NoneFoundForType => Loc.Localize("GuideListTable.NoneFoundForType", "No guides found for this duty type.");
        internal static string NoGuidesUnlocked => Loc.Localize("GuideListTable.NoGuidesUnlocked", "You have not unlocked any guides for this type.");
        internal static string NoGuidesFoundForSearch => Loc.Localize("GuideListTable.NoGuidesFoundForSearch", "No guides found for your search.");
        internal static string UnsupportedGuide(string guideName) => string.Format(Loc.Localize("GuideListTable.UnsupportedGuide", "The guide for {0} is for a different version of the plugin and cannot be loaded yet"), guideName);
        internal static string NoGuideData(string guideName) => string.Format(Loc.Localize("GuideListTable.NoGuideData", "There is no data for {0} yet."), guideName);
        internal static string NoGuidesFilesDetected => Loc.Localize("GuideListTable.NoGuidesFilesDetected", "No guide files were detected or loaded, check /xllog for errors or try reinstalling the plugin.");
    }
}
