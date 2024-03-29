using Sirensong.DataStructures;

namespace KikoGuide.GuideSystem.InstanceGuide.Content.ARR.Dungeons
{
    internal sealed class Sastasha : InstanceGuideBase
    {
        protected override uint DutyId { get; } = 4;
        protected override uint UnlockQuestId { get; } = 66211;

        public override string[] Authors { get; } =
        {
            "Blooym",
        };

        public override InstanceGuideContent Content { get; } = new()
        {
            Sections = new[]
            {
                new InstanceGuideContent.Section
                {
                    Title = new TranslatableString
                    {
                        EN = "Chopper",
                    },
                    Subsections = new[]
                    {
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new TranslatableString
                            {
                                EN = "Coral Formations",
                            },
                            Content = new TranslatableString
                            {
                                EN =
                                    "There will be a \"Bloody Memo\" after the first set of trash mobs that will tell you which colour of Coral Formation uncovers the \"Inconspicuous Switch\". If you choose the wrong colour, a trash mob will spawn instead. Pressing the right switch will summon Chopper, which is the first boss of the dungeon.",
                            },
                        },
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new TranslatableString
                            {
                                EN = "Boss Fight",
                            },
                            Content = new TranslatableString
                            {
                                EN = "Avoid being hit by \"Charged Whisker\", a cone AoE attack that inflicts Paralysis on anyone it hits.",
                            },
                            Tips = new TranslatableString[]
                            {
                                new()
                                {
                                    EN = "\"Charged Whisker\" will occur at 70% HP, 40% HP & 20% HP.",
                                },
                            },
                            Mechanics = new[]
                            {
                                new InstanceGuideContent.Section.Subsection.MechanicsTableRow
                                {
                                    Name = new TranslatableString
                                    {
                                        EN = "Charged Whisker",
                                    },
                                    Description = new TranslatableString
                                    {
                                        EN = "Inflicts Paralysis to all players hit.",
                                    },
                                },
                            },
                        },
                    },
                },
                new InstanceGuideContent.Section
                {
                    Title = new TranslatableString
                    {
                        EN = "Captain Madison",
                    },
                    Subsections = new[]
                    {
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new TranslatableString
                            {
                                EN = "1st Encounter",
                            },
                            Content = new TranslatableString
                            {
                                EN =
                                    "You will encounter Captain Madison twice in this duty. He will be joined by some \"Shallowtail Reavers\" that will attack you with only basic skills. Once you get Captain Madison down to 50% health he will flee, allowing you to progress.",
                            },
                        },
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new TranslatableString
                            {
                                EN = "2nd Encounter",
                            },
                            Content = new TranslatableString
                            {
                                EN =
                                    "Captain Madison will open the door and summon some scurvy dogs when they reach 50% HP. Once Captain Madison reaches 20% health, he will flee for the remainder of the duty.",
                            },
                        },
                    },
                },
                new InstanceGuideContent.Section
                {
                    Title = new TranslatableString
                    {
                        EN = "Denn the Orcatoothed",
                    },
                    Subsections = new[]
                    {
                        new InstanceGuideContent.Section.Subsection
                        {
                            Title = new TranslatableString
                            {
                                EN = "Phase 1",
                            },
                            Content = new TranslatableString
                            {
                                EN =
                                    "In the four corners of the boss arena there are grates. During the fight the boss, one or two grates will bubble at a time with \"Unnatural Ripples\". All players - apart from the tank - should interact with the bubbling grates to prevent \"Baleen Guards\" from spawning. If adds spawn, they should be focused first as they can quickly snowball the fight out of control and kill the tank. Once Denn the Orcatoothed is defeated, all other adds will despawn.",
                            },
                            Mechanics = new InstanceGuideContent.Section.Subsection.MechanicsTableRow[]
                            {
                                new()
                                {
                                    Name = new TranslatableString
                                    {
                                        EN = "Hydroball",
                                    },
                                    Description = new TranslatableString
                                    {
                                        EN = "A cone AoE attack.",
                                    },
                                },
                                new()
                                {
                                    Name = new TranslatableString
                                    {
                                        EN = "True Thrust",
                                    },
                                    Description = new TranslatableString
                                    {
                                        EN = "A single-target attack on the tank (or target with highest emnity).",
                                    },
                                },
                                new()
                                {
                                    Name = new TranslatableString
                                    {
                                        EN = "Jumping Thrust",
                                    },
                                    Description = new TranslatableString
                                    {
                                        EN = "Essentially the same as True Thrust.",
                                    },
                                },
                            },
                        },
                    },
                },
            },
        };
    }
}
