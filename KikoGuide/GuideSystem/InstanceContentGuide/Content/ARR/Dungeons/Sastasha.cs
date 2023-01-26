namespace KikoGuide.GuideSystem.InstanceContentGuide.Content.ARR.Dungeons
{
    internal sealed class Sastasha : InstanceContentGuideBase
    {
        public override uint DutyId { get; } = 4;
        public override uint UnlockQuestId { get; } = 66211;
        public override InstanceContentGuideFormat Content { get; } = new()
        {
            Sections = new[]
            {
                new InstanceContentGuideFormat.Section
                {
                    Title = new ()
                    {
                       EN = "Chopper",
                    },

                    Subsections = new[]
                    {
                        new InstanceContentGuideFormat.Section.Subsection
                        {
                            Content = new()
                            {
                                EN = "Avoid being hit by \"Charged Whisker\", which is an AoE attack that inflicts Paralysis.",
                            },

                            Mechanics = new[]
                            {
                                new InstanceContentGuideFormat.Section.Subsection.MechanicsTableRow
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

                            Tips = new[]
                            {
                                new InstanceContentGuideFormat.Section.Subsection.Tip
                                {
                                    Content = new()
                                    {
                                        EN = "Avoid being hit by \"Charged Whisker\", which is an AoE attack that inflicts Paralysis.",
                                    },
                                },
                            }
                        },
                    }
                },

                new InstanceContentGuideFormat.Section
                {
                    Title = new()
                    {
                        EN = "Captain Madison",
                    },

                    Subsections = new[]
                    {
                        new InstanceContentGuideFormat.Section.Subsection
                        {
                            Content = new()
                            {
                                EN = "You must defeat Captain Madison twice. In the first encounter, kill adds first and then focus on the boss until they run away. In the second encounter they will summon guard dogs at 50% HP, kill these and attack until they flees.",
                            },
                        },
                    }
                },

                new InstanceContentGuideFormat.Section
                {
                    Title = new()
                    {
                        EN = "Denn the Orcatoothed",
                    },

                    Subsections = new[]
                    {
                        new InstanceContentGuideFormat.Section.Subsection
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
