using System;
using KikoGuide.Common;
using Lumina.Excel.GeneratedSheets;

namespace KikoGuide.DataModels
{
    /// <summary>
    /// Represents an in-game duty and models the data associated with it.
    /// </summary>
    internal sealed class Duty
    {
        /// <summary>
        /// The <see cref="ContentFinderCondition"/> row.
        /// </summary>
        public ContentFinderCondition CFCondition { get; init; }

        /// <summary>
        /// The <see cref="ContentFinderConditionTransient"/> row.
        /// </summary>
        public ContentFinderConditionTransient CFConditionTransient { get; init; }

        /// <returns>The duty, or <see langword="null"/> if unable to find necessary data.</returns>
        public static Duty? GetDutyOrNull(uint id)
        {
            var cfCondition = Services.Data.GetExcelSheet<ContentFinderCondition>()?.GetRow(id);
            var cfConditionTransient = Services.Data.GetExcelSheet<ContentFinderConditionTransient>()?.GetRow(id);

            if (cfCondition == null || cfConditionTransient == null)
            {
                return null;
            }

            return new Duty(cfCondition, cfConditionTransient);
        }

        public static Duty GetDuty(uint id)
        {
            var cfCondition = Services.Data.GetExcelSheet<ContentFinderCondition>()?.GetRow(id);
            var cfConditionTransient = Services.Data.GetExcelSheet<ContentFinderConditionTransient>()?.GetRow(id);

            if (cfCondition == null || cfConditionTransient == null)
            {
                throw new ArgumentException($"Unable to find duty with ID {id}.");
            }

            return new Duty(cfCondition, cfConditionTransient);
        }

        /// <summary>
        /// Creates a new <see cref="Duty"/> instance.
        /// </summary>
        /// <param name="id">The RowID of the duty from the <see cref="ContentFinderCondition"/> sheet.</param>
        public Duty(ContentFinderCondition cfCondition, ContentFinderConditionTransient cfConditionTransient)
        {
            this.CFCondition = cfCondition;
            this.CFConditionTransient = cfConditionTransient;
        }
    }
}
