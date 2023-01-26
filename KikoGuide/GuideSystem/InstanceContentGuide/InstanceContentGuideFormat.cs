using Sirensong.DataStructures;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    /// <summary>
    /// Represents the content to be layed out in the user interface.
    /// </summary>
    public readonly record struct InstanceContentGuideFormat
    {
        /// <summary>
        /// The sections of the content.
        /// </summary>
        public Section[]? Sections { get; init; }

        /// <summary>
        /// Represents a content section.
        /// </summary>
        public readonly record struct Section
        {
            /// <summary>
            /// The title of the section.
            /// </summary>
            public TranslatableString Title { get; init; }

            /// <summary>
            /// The subsections of the section.
            /// </summary>
            public Subsection[]? Subsections { get; init; }

            /// <summary>
            /// Represents a subsection.
            /// </summary>
            public readonly record struct Subsection
            {
                /// <summary>
                /// Represents the text content of the subsection.
                /// </summary>
                public TranslatableString Content { get; init; }

                /// <summary>
                /// A table of mechanics in the subsection.
                /// </summary>
                public MechanicsTableRow[]? Mechanics { get; init; }

                /// <summary>
                /// The tips/bulletpoints of the subsection.
                /// </summary>
                public Tip[]? Tips { get; init; }

                /// <summary>
                /// Represents a mechanics table row.
                /// </summary>
                public readonly record struct MechanicsTableRow
                {
                    public TranslatableString Name { get; init; }
                    public TranslatableString Description { get; init; }
                }

                /// <summary>
                /// Represents a bulletpoint/tip.
                /// </summary>
                public readonly record struct Tip
                {
                    public TranslatableString Content { get; init; }
                }
            }
        }
    }
}