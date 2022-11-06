using CheapLoc;

namespace KikoGuide.Localization
{
    /// <summary>
    ///     A collection translatable strings.
    /// </summary>
    internal sealed class TStrings
    {
        // Guide List
        internal static string GuideListContentNotFound => Loc.Localize("GuideList.NoFiles", "No guide files detected! Please try Settings -> Update Resources.");


        // Guide viewer
        internal static string GuideInfoNoneSelected => Loc.Localize("DutyInfo.NoDuty", "No guide selected, use /kikolist to see all available guides.");
        internal static string GuideInfoNotUnlocked => Loc.Localize("DutyInfo.NotUnlocked", "You cannot view the guide for this duty as you have not unlocked it yet.");

        // Commands
        internal static string GuideListHelp => Loc.Localize("Commands.DutyList.Help", "Toggles the guide list");
        internal static string SettingsHelp => Loc.Localize("Commands.Settings.Help", "Toggles the settings menu");
        internal static string EditorHelp => Loc.Localize("Commands.Editor.Help", "Toggles the guide editor");
        internal static string InfoHelp => Loc.Localize("Commands.Info.Help", "Toggles the guide viewer window");

        // Types
        internal static string TypeDutyUnnamed = Loc.Localize("Types.Duty.Name.None", "Unnamed Guide");
        internal static string TypeDutySectionStrategyNone = Loc.Localize("Types.Guide.Section.Strategy.None", "No description available.");
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
        internal static string InDuty => Loc.Localize("Generics.InDuty", "In Duty");
        internal static string Mechanic => Loc.Localize("Generics.Mechanic", "Mechanic");
        internal static string Description => Loc.Localize("Generics.Description", "Description");
        internal static string Level => Loc.Localize("Generics.Level", "Level");
        internal static string Type => Loc.Localize("Generics.Type", "Type");
        internal static string Enabled => Loc.Localize("Generics.Enabled", "Enabled");
        internal static string Disabled => Loc.Localize("Generics.Disabled", "Disabled");
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
        internal static string NoInfoAvailable => Loc.Localize("GuideViewer.NoInfoAvailable", "There is no information available for this guide right now.");
        internal static string UnlockWindowMovement => Loc.Localize("GuideViewer.UnlockWindowMovement", "Unlock Window Movement");
        internal static string LockWindowMovement => Loc.Localize("GuideViewer.LockWindowMovement", "Lock Window Movement");
        internal static string UnlockWindowResize => Loc.Localize("GuideViewer.UnlockWindowResize", "Unlock Window Resize");
        internal static string LockWindowResize => Loc.Localize("GuideViewer.LockWindowResize", "Lock Window Resize");
        internal static string ToggleSettingsWindow => Loc.Localize("GuideViewer.ToggleSettingsWindow", "Toggle Settings Window");
        internal static string GuideHeading(string guideName) => string.Format(Loc.Localize("GuideViewer.GuideHeading", "Guide for {0}"), guideName);
    }

    /// <summary>
    ///     Translation strings used in the guide list window.
    /// </summary>
    internal static class TGuideListTable
    {
        internal static string NoneFoundForType => Loc.Localize("GuideListTable.NoneFoundForType", "No guides found for this duty type.");
        internal static string NoGuidesUnlocked => Loc.Localize("GuideListTable.NoGuidesUnlocked", "You have not unlocked any guides for this type.");
        internal static string NoGuidesFoundForSearch => Loc.Localize("GuideListTable.NoGuidesFoundForSearch", "No guides found for your search.");
        internal static string UnsupportedGuide(string guideName) => string.Format(Loc.Localize("GuideListTable.UnsupportedGuide", "The guide for {0} is for a different version of the plugin and cannot be loaded."), guideName);
        internal static string NoGuideData(string guideName) => string.Format(Loc.Localize("GuideListTable.NoGuideData", "There is no data for {0} yet."), guideName);
    }
}
