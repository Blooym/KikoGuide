using KikoGuide.Common;
using Lumina.Excel.GeneratedSheets;

namespace KikoGuide.DataModels
{
    /// <summary>
    /// Represents an in-game duty and models the data associated with it.
    /// </summary>
    internal sealed record class Duty
    {
        /// <summary>
        /// The <see cref="ContentFinderCondition"/> row.
        /// </summary>
        public ContentFinderCondition CFCondition { get; init; }

        /// <summary>
        /// The <see cref="ContentFinderConditionTransient"/> row.
        /// </summary>
        public ContentFinderConditionTransient CFConditionTransient { get; init; }

        /// <summary>
        /// Gets the duty or <see langword="null"/> if unable to find necessary data.
        /// </summary>
        /// <param name="id">The RowID of the duty from the <see cref="ContentFinderCondition"/> sheet.</param>
        /// <returns>The <see cref="Duty"/> or <see langword="null"/>.</returns>
        public static Duty? GetDutyOrNull(uint id)
        {
            var cfCondition = Services.ContentFinderConditionCache.GetRow(id);
            var cfConditionTransient = Services.ContentFinderConditionTransientCache.GetRow(id);

            if (cfCondition == null || cfConditionTransient == null)
            {
                return null;
            }

            return new Duty(cfCondition, cfConditionTransient);
        }

        /// <summary>
        /// Creates a new <see cref="Duty"/> instance.
        /// </summary>
        /// <param name="cfCondition">The <see cref="ContentFinderCondition"/> row.</param>
        /// <param name="cfConditionTransient">The <see cref="ContentFinderConditionTransient"/> row.</param>
        public Duty(ContentFinderCondition cfCondition, ContentFinderConditionTransient cfConditionTransient)
        {
            this.CFCondition = cfCondition;
            this.CFConditionTransient = cfConditionTransient;
        }
    }
}
