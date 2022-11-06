<div align="center">

<img src="./.assets/icon.png" alt="Kiko Guide Logo" width="15%">
  
# Contributing Guide
  
</div>

### Table of Contents

- [Localization Contribution](#localization-contribution)
- [Code Contribution](#code-contribution)
- [Guide Contribution](#guide-contribution)

---

## Localization Contribution
Localization support & contributions are still being worked on being better supported for future releases, but any changes are appreciated nonetheless.

#### Plugin Strings
Localizations to plugin strings are handled through [Crowdin](https://crowdin.com/project/KikoGuide).

#### Guide Localizations
Guide localizations are not currently handled through Crowdin due to the sheer amount of Guide that need to be written. They will most likely be listed on Crowdin in the future when the groundwork has been done to support it better.

For now, if you wish to add a guide in a supported language, please make a new folder in `Resources/Localization/Duty/<version>/<2 letter country code>/` and make sure it has all the same paths as the source duty files. Do not rename the filename itself, just the content within it.

---

## Code Contribution
When making any code changes to the plugin, the most important thing to do is make sure that all your changes comply with the [Dalamud Plugin Rules](https://goatcorp.github.io/faq/development#q-what-am-i-allowed-to-do-in-my-plugin) for what is allowed in a plugin. Any changes that do not meet this requirement will not be merged as the plugin is listed in the official repository. You should make an issue first if you are unsure if your changes will be accepted.

#### Building
When setting up a development environment it is recommended to use the provided [dev container](./.devcontainer) setup, as it will automatically handle installing and configuring the development environment for you. If you want to develop on your local machine instead of a container then you can find a guide [here](https://goatcorp.github.io/faq/development). 

Please note if you are using Linux you must set the `DALAMUD_HOME` environment variable to wherever your Dalamud development folder is located. (this is done for you if using the container environment).

#### Code Standards 
It is highly preferred that you are using a linter and formatting your code cleanly to ensure that it is easy to read and extend in the future. Most code editors should provide support for this out of the box or with an additional extension.

#### Functionality 
Please ensure that all code is properly separated based on its functionality and work towards achieving proper encapsulation where possible. For example, UI elements should not contain any logic that will not be used for drawing itself or displaying information, fetching the information it is displaying should be fetched from a separate class in a different file, like a presenter or service.

#### Documentation & Comments
Please write `<summary>` comments for functions, methods and classes to help understand the intentions behind the code, and to help others understand how to use it when working with the plugin.

---
 
## Guide Contribution
Guide contributions are highly appreciated due to the amount of duties that are in the game. All files for supported duty types already exist in the repository, and new ones will be made when new duties release. Please note that this repository may not checked around major patches to avoid spoiling new content, so a guide may not be made available until a few days after a patch.

#### Editing Guides
Guides are stored as `.json` files inside of the `Resources/Localization/Duty/<version>/<lang>/` folder, and are named their exact English in-game name no matter the language. 

When changing these files, it is important to run it through a JSON validator before committing back to the repository, otherwise it may be disabled on load due to being unparsable. You should also consider running a spell/grammar checker on the file.

It is highly recommended to use the in-game duty editor to edit guides, as it will provide a live preview of all your changes, as well as real-time validation and formatting, you can access the editor by using `/kikoeditor` with the plugin installed. 

##### Editor Preview
![Editor Preview](./.assets/editor.png)

##### Keys for JSON files
- `Version`: The duty file version, do not change this manually.
- `Name`: The name of the duty
- `Difficulty`: The difficulty ID of the duty
- `Type`: The type ID of the duty
- `Expansion`: The expansion ID of the duty
- `Level`: The level of the duty in-game
- `UnlockQuestID`: The [quest ID](https://github.com/xivapi/ffxiv-datamining/blob/master/csv/Quest.csv) that unlocks the duty
- `TerritoryIDs`: The territory ID(s) to automatically open the guide for
- `Sections`: The sections of the guide
  - `Type`: The type of section
  - `Name`: The name of the section (usually the name of the boss)
  - `Phases`: The phases of the section
    - `TitleOverride`: The title of the phase (optional, only set in special cases)
    - `Strategy`: The strategy for the phase
    - `StrategyShort`: The short strategy for the phase used when enabled in the settings
    - `Mechanics`:
      - `Name`: The name of the mechanic
      - `Description`: The description of the mechanic
      - `ShortDescription`: The short description of the mechanic used when enabled in the settings
      - `Type`: The type of mechanic

An example guide to refer to for help when writing or editing can be found [here](src/Resources/Localization/v1/Guide/en/A%20Realm%20Reborn/Dungeons/CopperbellMines.json).

When writing descriptions of mechanics or sections, please try and minimize the language used to keep it down to just key information, and avoid lesser-known terminology. Keep the level/expansion of the duty in mind, as some players may not have experienced certain mechanics/strategies before.

#### Formatting 
When writing the guides, you can use `\n` to move to a new line as JSON does not support this. You can also use `\t` to indent the text by 1 tab space when needed. If you want to use a percentage sign, you will have to do `%%` to display a single percent sign. 

#### Internal IDs (DutyType, MechanicTypes, DutyDifficulty, etc)
You can find all internal IDs used for identifying duty data inside of the [Guide Type](src/Types/Guide.cs) of the repository or inside of the in-game editor (`/kikoeditor`)

#### Game IDs (Quest, Territory, etc)
You can find all game IDs through the csv files available from the FFXIV-Datamining repository [here](https://github.com/xivapi/ffxiv-datamining)

If for whatever reason you are struggling to locate any of these IDs, feel free to make the pull request without them and clarify that in the request and they will be added for you.

--- 
###### See Also
- [Commit Convention](COMMIT_CONVENTION.md)
- [FFXIV Wiki](https://ffxiv.consolegameswiki.com)
- [FFXIV-Datamining](https://github.com/xivapi/ffxiv-datamining)
