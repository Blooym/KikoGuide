namespace KikoGuide.Enums;

/// <summary>
///     Map DutyTypes to IDs, which can then be implemented by Duty files to determine their type.
///     Any DutyType added will be added to the UI as a new tab category.
/// </summary>
enum DutyType
{
    Dungeon = 0,
    Trial = 1,
    AllianceRaid = 2
}


/// <summary>
///     Map DutyDifficulty to IDs, which can then be implemented by Duty files to determine their difficulty.
/// </summary>
enum DutyDifficulty
{
    Normal = 0,
    Hard = 1,
    Extreme = 2,
    Savage = 3,
    Ultimate = 4,
    Unreal = 5
}


/// <summary>
///     Map game expansions to IDs, which can then be implemented by 
///     Duty files to determine what expansion they belong to.
/// </summary>
enum Expansion
{
    ARealmReborn = 0,
    Heavensward = 1,
    Stormblood = 2,
    Shadowbringers = 3,
    Endwalker = 4
}


/// <summary>
///     Map Mechanics to IDs, which can then be implemented by duty files (bosses, etc) to determine the mechanic type.
///     Any mechanic within this enum will be added to the configurable hidden mechanics list.
/// </summary>
enum Mechanics
{
    Tankbuster = 0,
    Enrage = 1,
    AOE = 2,
    Stackmarker = 3,
    Raidwide = 4,
    Invulnerablity = 5,
    Targetted = 6,
    AddSpawn = 7,
    DPSCheck = 8,
    Cleave = 9,
    Other = 10,
}