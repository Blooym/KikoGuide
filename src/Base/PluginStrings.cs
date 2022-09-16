namespace KikoGuide.Base;

using System;
using CheapLoc;

/// <summary> 
///     A collection of read-only strings used by the plugin.
/// </summary>
sealed internal class PStrings
{
    /// <summary> 
    ///    This is the name that will be shown in all UI elements, does not change InternalName.
    /// </summary>
    internal static readonly string pluginName = "Kiko Guide";

    /// <summary>
    ///     The repository to linked to this plugin.
    /// </summary>
    internal static readonly string repoUrl = "https://github.com/BitsOfAByte/KikoGuide/";

    /// <summary>
    ///     The production branch of the repository.
    /// </summary>
    internal static readonly string repoBranch = "main";

    /// <summary>
    ///     The resources directory relative to the base of the repository.
    /// </summary>
    internal static readonly string repoResourcesDir = "src/Resources/";

    /// <summary>
    ///     The support button URL.
    /// </summary>
    internal static readonly string supportButtonUrl = "https://github.com/sponsors/BitsOfAByte";

    /// <summary>
    ///     The path to the plugin's resources folder with trailing slashes, relative to the plugin assembly location with trailing slashes.
    /// </summary>
    internal static readonly string pluginResourcesDir = $"{PluginService.PluginInterface.AssemblyLocation.DirectoryName}\\Resources\\";

    /// <summary>
    ///     The path to the plugin's localization folder with trailing slashes.
    /// </summary>
    internal static readonly string pluginlocalizationDir = pluginResourcesDir + "Localization\\";

    /// <summary>
    ///    The fallback language to use if the user's language is not supported for localization (ISO 639-1 code).
    /// </summary>
    internal static readonly string fallbackLanguage = "en";
}

/// <summary> 
///     A collection translatable strings.
/// </summary>
sealed internal class TStrings
{
    // Generics
    internal static string Error => Loc.Localize("Generics.Error", "An error occured.");
    internal static string Support => Loc.Localize("Generics.Support", "Support");
    internal static string OpenFile => Loc.Localize("Generics.OpenFile", "Open File");
    internal static string SaveFile => Loc.Localize("Generics.SaveFile", "Save File");
    internal static string Search => Loc.Localize("Generics.Search", "Search");
    internal static string Duty => Loc.Localize("Generics.Duty", "Duty");
    internal static string InDuty => Loc.Localize("Generics.InDuty", "In Duty");
    internal static string Mechanic => Loc.Localize("Generics.Mechanic", "Mechanic");
    internal static string Description => Loc.Localize("Generics.Description", "Description");
    internal static string Level => Loc.Localize("Generics.Level", "Level");
    internal static string Type => Loc.Localize("Generics.Type", "Type");


    // Editor Strings
    internal static string EditorTitle => String.Format(Loc.Localize("DutyEditor.Title", "{0} - Duty Editor"), PStrings.pluginName);
    internal static string EditorFormat => Loc.Localize("Editor.Format", "Format");
    internal static string EditorClear => Loc.Localize("Editor.Clear", "Clear");
    internal static string EditorPreview => Loc.Localize("Editor.Preview", "Preview");
    internal static string EditorMetadata => Loc.Localize("Editor.Metadata", "Metadata");
    internal static string EditorContributingGuide => Loc.Localize("Editor.ContributingGuide", "Contributing Guide");
    internal static string EditorProblems => Loc.Localize("Editor.Problems", "Problems");
    internal static string EditorProblemUnsupported => Loc.Localize("Editor.Unsupported", "Duty contains invalid IDs and/or is unsupported by this plugin version.");
    internal static string EditorNoProblems => Loc.Localize("Editor.NoProblemsDetected", "No problems detected.");
    internal static string EditorFileTooLarge => Loc.Localize("Editor.FileTooBig", "Your file is too big to be loaded into the editor.");
    internal static string EditorFileSuccessfullyLoaded => Loc.Localize("Editor.FileLoaded", "Your file was successfully loaded.");
    internal static string EditorFileSuccessfullySaved => Loc.Localize("Editor.FileSaved", "Your file was successfully saved.");


    // Settings Strings
    internal static string SettingsTitle => String.Format(Loc.Localize("Settings.Title", "{0} - Settings"), PStrings.pluginName);
    internal static string SettingsGeneral => Loc.Localize("Settings.General", "General");
    internal static string SettingsAutoOpenInDuty => Loc.Localize("Settings.AutoOpenInDuty", "Auto Open In Duty");
    internal static string SettingsAutoOpenInDutyTooltip => Loc.Localize("Settings.AutoOpenDuty.Tooltip", "Open the duty guide when entering a duty.");
    internal static string SettingsShortMode => Loc.Localize("Settings.ShortMode", "Short Mode");
    internal static string SettingsShortModeTooltip => Loc.Localize("Settings.ShortMode.Tooltip", "Shorten strategies & descriptions if possible.");
    internal static string SettingsShowSupportButton => Loc.Localize("Settings.ShowSupportButton", "Show Support Button");
    internal static string SettingsShowSupportButtonTooltip => Loc.Localize("Settings.ShowSupportButton.Tooltip", "Show the support Kiko Guide button");
    internal static string SettingsResourcesAndLocalization => Loc.Localize("Settings.ResourcesAndLocalization", "Resources & Localization");
    internal static string SettingsResourcesAndLocalizationDesc => Loc.Localize("Settings.ResourcesAndLocalization.Tooltip", "Downloads the latest resources, localizations & guides.");
    internal static string SettingsUpdateResources => Loc.Localize("Settings.UpdateResources", "Update Resources");
    internal static string SettingsUpdateFailed => Loc.Localize("Settings.UpdateFailed", "Update Failed");
    internal static string SettingsLastUpdate(string time) => String.Format(Loc.Localize("Settings.LastUpdate", "Last Update: {0}"), time);
    internal static string SettingsMechanics => Loc.Localize("Settings.Mechanics", "Mechanics");
    internal static string SettingsHideMechanic(string? mechanicName) => String.Format(Loc.Localize("Settings.HideMechanic", "Hide: {0}"), mechanicName);
    internal static string SettingsHideMechanicTooltip(string? mechanicName) => String.Format(Loc.Localize("Settings.HideMechanicTooltip", "Hide {0} from duty guides."), mechanicName);
    internal static string SettingsIntegrations => Loc.Localize("Settings.Integrations", "Integrations");
    internal static string SettingsAvailableIntegrations => Loc.Localize("Settings.Integrations.Available", "Available Integrations");
    internal static string SettingsIntegrationsDesc => Loc.Localize("Settings.Integrations.Tooltip", "You can enable or disable integrations below, changes will take effect next plugin load if the integration plugin is present.");


    // Duty Finder
    internal static string DutyFinderTitle => String.Format(Loc.Localize("DutyFinder.Title", "{0} - Duty Finder"), PStrings.pluginName);
    internal static string DutyFinderContentNotFound => Loc.Localize("DutyFinder.NoFiles", "No duty files detected! Please try Settings -> Update Resources.");


    // Duty Info
    internal static string DutyInfoTitle => String.Format(Loc.Localize("DutyInfo.Title", "{0} - Duty Info"), PStrings.pluginName);
    internal static string DutyInfoNoneSelected => Loc.Localize("DutyInfo.NoDuty", "No duty selected, use /kikolist to see all available duties.");
    internal static string DutyInfoNotUnlocked => Loc.Localize("DutyInfo.NotUnlocked", "You cannot view the guide for this duty as you have not unlocked it yet.");


    // Duty List Component
    internal static string DutyListNoneFound => Loc.Localize("DutyList.NoneFound", "No duties found.");
    internal static string DutyListNoGuide(string dutyName) => String.Format(Loc.Localize("DutyList.NoGuide", "No guide available for {0}."), dutyName);
    internal static string DutyListNeedsUpdate => String.Format(Loc.Localize("DutyList.NeedsUpdate", "Cannot display duty as it was not made for this plugin version."));


    // Duty Heading Component
    internal static string DutyHeadingTitle(string dutyName) => String.Format(Loc.Localize("DutyHeading.Title", "Duty: {0}"), dutyName);


    // Commands
    internal static string DutyListHelp => Loc.Localize("Commands.DutyList.Help", "Toggles the duty list");
    internal static string SettingsHelp => Loc.Localize("Commands.Settings.Help", "Toggles the settings menu");
    internal static string EditorHelp => Loc.Localize("Commands.Editor.Help", "Toggles the duty editor");
    internal static string InfoHelp => Loc.Localize("Commands.Info.Help", "Toggles the duty info window if a duty is loaded");

    // Types
    internal static string TypeDutyUnnamed = Loc.Localize("Types.Duty.Name.None", "Unnamed Duty");
    internal static string TypeDutySectionStrategyNone = Loc.Localize("Types.Duty.Section.Strategy.None", "No description available.");
}
