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

#### Duty Localizations
Duty localizations are not currently handled through Crowdin due to the sheer amount of duties that have not been written yet. They will eventually be added to Crowdin once the project has matured a bit more and the localization code of the plugin has been updated to make it as easy as possible to add new localizations.

For now, if you wish to add a duty localization in a supported language, please make a new folder in `Resources/Localization/Duty/<2 letter country code>/` and make sure it has all the same paths as the source duty files. Do not rename the file name itself, just the content within it.

---

## Code Contribution
When making any code changes to the plugin, the most important thing to do is make sure that all your changes comply with the [Dalamud Rules](https://goatcorp.github.io/faq/development#q-what-am-i-allowed-to-do-in-my-plugin) for what is allowed in a plugin. Any changes that do not meet this requirement will not be merged as the plugin is listed in the official repository.

#### Building
When setting up a development environment it is recommended to use the provided [dev container](./.devcontainer) setup, as it will automatically handle installing and configuring the development environment for you. If you want to develop on your local machine instead of a container then you can find a guide [here](https://goatcorp.github.io/faq/development). 

Please note if you are using Linux you must set the `DALAMUD_HOME` environment variable to wherever your Dalamud development folder is located. (this is done for you if using the container environment).

#### Code Standards 
It is highly preferred that you are using a linter and formatting your code cleanly to ensure that it is easy to read and extend in the future. Most code editors should provide support for this out of the box or with an additional extension.

As for the code style in general, there are no strict rules for how code should be written, but following standard conventions for CSharp is highly recommended.

#### Functionality 
Please ensure that all code is properly separated based on its functionality and work towards achieving proper encapsulation where possible. For example, UI elements should not contain any logic that will not be used for drawing itself or displaying information, fetching the information it is displaying should be fetched from a separate class in a different file.

#### Documentation & Comments
Please write `<summary>` comments for functions, methods and classes to help understand the intentions behind the code, and to help others understand how to use it when working with the plugin.

#### Update Manager 
Please avoid making changes to the UpdateManager class as it is critical to avoid constantly pushing public updates, and since it fetches external data it is preferred to not change it. However, if you believe that you can make optimizations to it, you are welcome to do so.

---
 
## Guide Contribution
Guide contributions are highly appreciated due to the amount of duties that are in the game. All files for duty's have already been created so far, and new ones will be made when new duties release. Please note that this repository may not checked around major patches to avoid spoilers.

#### Editing Guides
Guides are stored as `.json` files inside of the `Resources/Localization/Duty/<lang>/` folder, and are named their exact English in-game name no matter the language. 

When changing these files, it is important to run it through a JSON validator before committing back to the repository, otherwise it may be disabled on load due to being unparsable.

It is highly recommended to use the in-game editor to edit guides, as it will provide a live preview of all your changes, as well as real-time validation and formatting, you can access the editor by using `/kikoeditor` with the plugin installed. 

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
- `TerritoryIDs`: The territory ID in-game when inside of the duty
- `Bosses`: The list of bosses in the duty
    - `Name`: The name of the boss
    - `Strategy` (preferred): A longer-form guide on fighting the boss
    - `StrategyShort` (optional): A short-form guide on fighting the boss
    - `Mechanics` (optional): A list of all boss mechanics
      - `Name`: The name of the mechanic
      - `Description`: The description of the mechanic
      - `Type`: The type ID of the mechanic

An example duty to refer to for help when writing or editing a guide can be found [here](src/Resources/Localization/Duty/en/A%20Realm%20Reborn/Dungeons/CopperbellMines.json).

When writing descriptions of mechanics or bosses, please try and minimize the language used to keep it down to just key information. For example, you do not need to write something like "Deals a small amount of damage to all party members hit" for an AoE as that is self-explanatory.

#### Formatting 
When writing the guides, you can use `\n` to move to a new line as JSON does not support this. You can also use `\t` to indent the text by 1 tab space when needed. If you want to use a percentage sign, you will have to do `%%` to display a single percent sign. 

#### Internal IDs (DutyType, MechanicType, Difficulty, etc)
You can find all internal IDs used for identifying duty data inside of the [enums folder](src/Enums/) of the repository or inside of the in-game editor (`/kikoeditor`)

#### Game IDs (Quest, Territory, etc)
You can find all game IDs through the csv files available from the FFXIV-Datamining repository [here](https://github.com/xivapi/ffxiv-datamining)

If for whatever reason you are struggling to locate any of these IDs, feel free to make the pull request without them and clarify that in the request and they will be added for you.

--- 
###### See Also
- [Commit Convention](COMMIT_CONVENTION.md)
- [FFXIV Wiki](https://ffxiv.consolegameswiki.com)
- [FFXIV-Datamining](https://github.com/xivapi/ffxiv-datamining)
