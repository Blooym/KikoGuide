using Sirensong.DataStructures;

namespace KikoGuide.GuideSystem.InstanceContentGuide
{
    public readonly record struct InstanceContentGuideContent
    {
        /// <summary>
        /// The sections of the content.
        /// </summary>
        public required Section[] Sections { get; init; }

        /// <summary>
        /// Represents a content section.
        /// </summary>
        public readonly record struct Section
        {
            /// <summary>
            /// The title of the section.
            /// </summary>
            public required TranslatableString Title { get; init; }

            /// <summary>
            /// The subsections of the section.
            /// </summary>
            public required Subsection[] Subsections { get; init; }

            /// <summary>
            /// Represents a subsection.
            /// </summary>
            public readonly record struct Subsection
            {
                /// <summary>
                /// The title of the subsection.
                /// </summary>
                public required TranslatableString Title { get; init; }

                /// <summary>
                /// Represents the text content of the subsection.
                /// </summary>
                public required TranslatableString Content { get; init; }

                /// <summary>
                /// A table of mechanics in the subsection.
                /// </summary>
                public MechanicsTableRow[]? Mechanics { get; init; }

                /// <summary>
                /// The tips/bulletpoints of the subsection.
                /// </summary>
                public TranslatableString[]? Tips { get; init; }

                /// <summary>
                /// The links of the subsection.
                /// </summary>
                public string[] Links { get; init; }

                /// <summary>
                /// Represents a mechanics table row.
                /// </summary>
                public readonly record struct MechanicsTableRow
                {
                    public required TranslatableString Name { get; init; }
                    public required TranslatableString Description { get; init; }
                }
            }
        }
    }
}