namespace KikoGuide.Guides.Guides.Dungeons
{
    public class Sastasha : Guide
    {
        public override uint DutyId { get; } = 4;
        public override DutyDifficulty Difficulty { get; } = DutyDifficulty.Normal;
        public override uint UnlockQuestId { get; } = 66211;
        public override GuideContent Content { get; protected set; } = new()
        {
            Sections = new[]
            {
                new GuideContent.ContentSection
                {
                    Title = new()
                    {
                       EN = "Chopper",
                    },
                    SubSections = new[]
                    {
                        new GuideContent.ContentSection.SubSection
                        {
                            Content = new()
                            {
                                EN = $"Avoid being hit by \"Charged Whisker\", which is an AoE attack that inflicts {Statuses.Paralysis.Name}.",
                            },
                            Mechanics = new GuideContent.ContentSection.SubSection.Mechanic[]
                            {
                                new GuideContent.ContentSection.SubSection.Mechanic
                                {
                                    Name = new()
                                    {
                                        EN = "Charged Whisker",
                                    },
                                    Description = new()
                                    {
                                        EN = $"Inflicts {Statuses.Paralysis.Name} to all players hit.",
                                    },
                                }
                            },
                        },
                    }
                },
                new GuideContent.ContentSection
                {
                    Title = new()
                    {
                        EN = "Captain Madison",
                    },
                    SubSections = new[]
                    {
                        new GuideContent.ContentSection.SubSection
                        {
                            Content = new()
                            {
                                EN = "You must defeat Captain Madison twice. In the first encounter, kill adds first and then focus on the boss until they run away. In the second encounter they will summon guard dogs at 50% HP, kill these and attack until they flees.",
                            },
                        },
                    }
                },
                new GuideContent.ContentSection
                {
                    Title = new()
                    {
                        EN = "Denn the Orcatoothed",
                    },
                    SubSections = new[]
                    {
                        new GuideContent.ContentSection.SubSection
                        {
                            Content = new()
                            {
                                EN = "During the boss fight, adds will spawn from the bubbling water, interact with the grate when it's bubbling to prevent spawning. These can also be safely ignored if enough damage is being dealt to the boss.",
                            },
                        },
                    }
                },
            }
        };
    }
}
