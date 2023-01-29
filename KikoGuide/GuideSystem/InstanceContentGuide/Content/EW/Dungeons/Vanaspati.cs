namespace KikoGuide.GuideSystem.InstanceContentGuide.Content.EW.Dungeons
{
    internal sealed class Vanaspati : InstanceContentGuideBase
    {
        protected override uint DutyId { get; } = 789;
        protected override uint UnlockQuestId { get; } = 69945;
        public override InstanceContentGuideContent Content { get; } = new()
        {
            Sections = new[]
            {
                new InstanceContentGuideContent.Section
                {
                    Title = new()
                    {
                       EN = "Terminus Snatcher",
                    },

                    Subsections = new[]
                    {
                        new InstanceContentGuideContent.Section.Subsection
                        {
                            Title = new()
                            {
                                EN = "Phase 1",
                            },

                            Content = new()
                            {
                                EN = "Stay away from mouths that are open watch the boss's animations to predict mechanics. Also has a spinning hand cursor mechanic that obstructs your movement.",
                            },

                            Mechanics = new[]
                            {
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Last Gasp",
                                    },

                                    Description = new()
                                    {
                                        EN = "Tankbuster.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Note of Despair",
                                    },

                                    Description = new()
                                    {
                                        EN = "Unavoidable raidwide damage.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Mouth Off",
                                    },

                                    Description = new()
                                    {
                                        EN = "The boss spawns mouths around the floor of the arena. The mouths that open will cast a large circle AoE around themselves.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "What is Left/Right",
                                    },

                                    Description = new()
                                    {
                                        EN = "Half room AoE on the left/right side of the boss. His raised arm and cast indicate which side isn't safe.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Lost Hope",
                                    },

                                    Description = new()
                                    {
                                        EN = "Places a spinning, pointing hand cursor above players heads. When players press a movement key the cursor stops spinning and the player moves into the direction the cursor is pointing.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Wallow",
                                    },

                                    Description = new()
                                    {
                                        EN = "Targeted circle AoEs at all players.",
                                    },
                                },
                            }
                        },
                    }
                },

                new InstanceContentGuideContent.Section
                {
                    Title = new()
                    {
                       EN = "Terminus Wrecker",
                    },

                    Subsections = new[]
                    {
                        new InstanceContentGuideContent.Section.Subsection
                        {
                            Title = new()
                            {
                                EN = "Phase 1",
                            },

                            Content = new()
                            {
                                EN = "The boss surrounds the arena with a damaging ring of fire. Try to not get knocked into it, and run into water orbs when he casts Aether Spray - Fire.",
                            },

                            Mechanics = new[]
                            {
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Total Wreck",
                                    },

                                    Description = new()
                                    {
                                        EN = "Tankbuster.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Meaningless Destruction",
                                    },

                                    Description = new()
                                    {
                                        EN = "Unavoidable raidwide damage.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Poison Heart",
                                    },

                                    Description = new()
                                    {
                                        EN = "Party Stack.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Unholy Water",
                                    },

                                    Description = new()
                                    {
                                        EN = "Summons water orbs around the arena. Running into them inflicts the player with Fetters and applies a Water Resistance Down debuff, but makes them immune against fire damage.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Aether Siphon",
                                    },

                                    Description = new()
                                    {
                                        EN = "The boss absorbs aether from either the burning buildings or the nearby lake to cast another mechanic afterwards.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Aether Spray - Fire",
                                    },

                                    Description = new()
                                    {
                                        EN = "Unavoidable raidwide damage that inflicts players with a 20 second Burns debuff. Can be completely negated by running into water orbs.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Aether Spray - Water",
                                    },

                                    Description = new()
                                    {
                                        EN = "Large knockback from the center of the room.",
                                    },
                                },
                            }
                        }
                    }
                },

                new InstanceContentGuideContent.Section
                {
                    Title = new()
                    {
                       EN = "Svarbhanu",
                    },

                    Subsections = new[]
                    {
                        new InstanceContentGuideContent.Section.Subsection
                        {
                            Title = new()
                            {
                                EN = "Phase 1",
                            },

                            Content = new()
                            {
                                EN = "When the boss casts Aetherial Disruption a large, coloured icon covering the whole screen will show which AoEs go off. When the boss flies to the edge of the arena to launch meteors at it there are set safe zones, as the boss always targets the same lines with his meteors. From left to right, on the first set line 2 is safe, on the second set it's line 3 and on the third set it's line 1.",
                            },

                            Mechanics = new[]
                            {
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Gnashing Teeth",
                                    },

                                    Description = new()
                                    {
                                        EN = "Tankbuster.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Flames of Decay",
                                    },

                                    Description = new()
                                    {
                                        EN = "Unavoidable raidwide damage.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Aetherial Disruption",
                                    },

                                    Description = new()
                                    {
                                        EN = "Dives the arena into four sections via two red and two blue line AoEs. A red circle or blue triangle in front of the screen will show which lines explode.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Crumbling Sky (1)",
                                    },

                                    Description = new()
                                    {
                                        EN = "Casts either targeted circular AoEs around all players, or a short knockback from the center, or both. Used during Aetherial Disruption.",
                                    },
                                },
                                new InstanceContentGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new()
                                    {
                                        EN = "Crumbling Sky (2)",
                                    },

                                    Description = new()
                                    {
                                        EN = "The boss flies out of the arena and starts launching sets of three meteors towards it that result in line AoEs, along with circular AoEs that are spawned in random locations.",
                                    },
                                },
                            }
                        }
                    }
                }
            },
        };
    }
}