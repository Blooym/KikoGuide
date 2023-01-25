namespace KikoGuide.GuideHandling.Guides.Dungeons.ARR
{
    internal sealed class Sastaasha : GuideBase
    {
        public override uint DutyId { get; } = 90000;
        public override uint UnlockQuestId { get; } = 90000;
        protected override bool UseUnsafeNoGuideLink => true;
        public override string[] Authors { get; } = new[] { "Kiko" };
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
                                EN = "Avoid being hit by \"Charged Whisker\", which is an AoE attack that inflicts Paralysis.",
                            },
                            Mechanics = new GuideContent.ContentSection.SubSection.TableRow[]
                            {
                                new GuideContent.ContentSection.SubSection.TableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Charged Whisker",
                                    },
                                    Description = new()
                                    {
                                        EN = "Inflicts Paralysis to all players hit.",
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
