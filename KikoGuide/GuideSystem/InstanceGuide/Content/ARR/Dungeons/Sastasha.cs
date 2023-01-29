namespace KikoGuide.GuideSystem.InstanceGuide.Content.ARR.Dungeons
{
    internal sealed class Sastasha : InstanceGuideBase
    {
        protected override uint DutyId { get; } = 4;
        protected override uint UnlockQuestId { get; } = 66211;
        public override InstanceGuideContent Content { get; } = new()
        {
            Sections = new[]
            {
                new InstanceGuideContent.Section
                {
                    Title = new()
                    {
                       EN = "Chopper",
                    },

                    Subsections = new[]
                    {
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new()
                            {
                                EN = "Phase 1",
                            },

                            Content = new()
                            {
                                EN = "Avoid being hit by \"Charged Whisker\", which is an AoE attack that inflicts Paralysis.",
                            },

                            Mechanics = new[]
                            {
                                new InstanceGuideContent.Section.Subsection.MechanicsTableRow
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

                new InstanceGuideContent.Section
                {
                    Title = new()
                    {
                        EN = "Captain Madison",
                    },

                    Subsections = new[]
                    {
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new()
                            {
                                EN = "Phase 1",
                            },

                            Content = new()
                            {
                                EN = "You must defeat Captain Madison twice. In the first encounter, kill adds first and then focus on the boss until they run away. In the second encounter they will summon guard dogs at 50% HP, kill these and attack until they flees.",
                            },
                        },
                    }
                },

                new InstanceGuideContent.Section
                {
                    Title = new()
                    {
                        EN = "Denn the Orcatoothed",
                    },

                    Subsections = new[]
                    {
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new()
                            {
                                EN = "Phase 1",
                            },

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
