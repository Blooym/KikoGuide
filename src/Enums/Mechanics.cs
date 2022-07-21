namespace KikoGuide.Enums;

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