namespace KikoGuide.Enums
{
    /// <summary>
    /// Represents a ContentType, mapping from <see cref="ContentFinderCondition.ContentType" />.
    /// </summary>
    // a modified version from Sirensong.
    internal enum ContentTypeModified
    {
        Roulette = 1,
        Dungeons = 2,
        Guildhests = 3,
        Trials = 4,
        Raids = 5,
        PvP = 6,
        QuestBattles = 7,
        Fates = 8,
        TreasureHunt = 9,
        Levequests = 10,
        GrandCompany = 11,
        Companions = 12,
        TribalQuests = 13,
        OverallCompletion = 14,
        PlayerCommendation = 15,
        DisciplesOfTheLand = 16,
        DisciplesOfTheHand = 17,
        RetainerVentures = 18,
        GoldSaucer = 19,
        Unknown1 = 20,
        DeepDungeon = 21,
        Unknown2 = 22,
        Unknown3 = 23,
        DonderousTails = 24,
        CustomDeliveries = 25,
        Eureka = 26,
        Unknown4 = 27,
        SouthernFront = 29,
        VariantCriteonDungeon = 30,

        /// <remarks>
        /// This isn't a ContentType from the game, but its used in <see cref="ContentFinderConditionExtensions.GetDutyType(Lumina.Excel.GeneratedSheets.ContentFinderCondition)" /> 
        /// to determine <see cref="Raids" /> apart from alliance raid.
        /// </remarks>
        AllianceRaid = -1,

        Custom = 999,
    }
}