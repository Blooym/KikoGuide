namespace KikoGuide.Enums;

/// <summary>
///     Map DutyTypes to IDs, which can then be implemented by Duty files to determine their type.
///     Any DutyType added will be added to the UI as a new tab category.
/// </summary>
enum DutyType
{
    Dungeon = 0,
    Trial = 1,
    AllianceRaid = 2,
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
    Unreal = 5,
}
