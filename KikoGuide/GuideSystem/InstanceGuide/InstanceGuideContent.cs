using Sirensong.DataStructures;

namespace KikoGuide.GuideSystem.InstanceGuide
{
    public readonly record struct InstanceGuideContent
    {
        /// <summary>
        ///     The sections of the content.
        /// </summary>
        public required Section[] Sections { get; init; }

        /// <summary>
        ///     Represents a content section.
        /// </summary>
        public readonly record struct Section
        {
            /// <summary>
            ///     The title of the section.
            /// </summary>
            public required TranslatableString Title { get; init; }

            /// <summary>
            ///     The subsections of the section.
            /// </summary>
            public required Subsection[] Subsections { get; init; }

            /// <summary>
            ///     Represents a subsection.
            /// </summary>
            public readonly record struct Subsection
            {
                /// <summary>
                ///     The title of the subsection.
                /// </summary>
                public required TranslatableString Title { get; init; }

                /// <summary>
                ///     Represents the text content of the subsection.
                /// </summary>
                public required TranslatableString Content { get; init; }

                /// <summary>
                ///     A table of mechanics in the subsection.
                /// </summary>
                public MechanicsTableRow[]? Mechanics { get; init; }

                /// <summary>
                ///     The tips/bulletpoints of the subsection.
                /// </summary>
                public TranslatableString[]? Tips { get; init; }

                /// <summary>
                ///     The links of the subsection.
                /// </summary>
                public Link[] Links { get; init; }

                /// <summary>
                ///     Represents a mechanics table row.
                /// </summary>
                public readonly record struct MechanicsTableRow
                {
                    /// <summary>
                    ///     The name of the mechanic.
                    /// </summary>
                    public required TranslatableString Name { get; init; }

                    /// <summary>
                    ///     The description of the mechanic.
                    /// </summary>
                    public required TranslatableString Description { get; init; }
                }

                /// <summary>
                ///     Represents a link.
                /// </summary>
                public readonly record struct Link
                {
                    /// <summary>
                    ///     The URL of the link.
                    /// </summary>
                    public required string URL { get; init; }

                    /// <summary>
                    ///     The text of the link (e.g. "FFXIV Wiki")
                    /// </summary>
                    public required TranslatableString Text { get; init; }
                }
            }
        }
    }
}
