using System;
using KikoGuide.Common;
using Lumina.Excel.GeneratedSheets;

#pragma warning disable IDE0051, IDE0044
namespace KikoGuide.DataModels
{
    /// <summary>
    ///     Represents an in-game duty.
    /// </summary>
    public sealed record Duty
    {
        /// <summary>
        ///     The <see cref="ContentFinderCondition"/> row.
        /// </summary>
        public ContentFinderCondition CFCondition { get; init; }

        /// <summary>
        ///     The <see cref="ContentFinderConditionTransient"/> row.
        /// </summary>
        public ContentFinderConditionTransient CFConditionTransient { get; init; }

        /// <summary>
        ///     The <see cref="InstanceContent"/> row.
        /// </summary>
        public InstanceContent Instance { get; init; }

        /// <summary>
        ///     Gets the <see cref="ContentFinderCondition"/> row from the given ID.
        /// </summary>
        /// <param name="id">The ID of the row to get.</param>
        /// <returns>The <see cref="ContentFinderCondition"/> row, or <see langword="null"/> if it wasn't found.</returns>
        private static ContentFinderCondition? GetContentFinderCondition(uint id)
            => Services.Data.GetExcelSheet<ContentFinderCondition>()?.GetRow(id);

        /// <summary>
        ///     Gets the <see cref="ContentFinderConditionTransient"/> row from the given ID.
        /// </summary>
        /// <param name="id">The ID of the row to get.</param>
        /// <returns>The <see cref="ContentFinderConditionTransient"/> row, or <see langword="null"/> if it wasn't found.</returns>
        private static ContentFinderConditionTransient? GetContentFinderConditionTransient(uint id)
            => Services.Data.GetExcelSheet<ContentFinderConditionTransient>()?.GetRow(id);

        /// <summary>
        ///     Gets the <see cref="Instance"/> row from the given ID.
        /// </summary>
        /// <param name="id">The ID of the row to get.</param>
        /// <returns>The <see cref="Instance"/> row, or <see langword="null"/> if it wasn't found.</returns>
        private static InstanceContent? GetInstanceContent(uint id)
            => Services.Data.GetExcelSheet<InstanceContent>()?.GetRow(id);

        /// <summary>
        ///     Creates a new <see cref="Duty"/> instance.
        /// </summary>
        /// <param name="id">The RowID of the duty from the <see cref="ContentFinderCondition"/> sheet.</param>
        internal Duty(uint id)
        {
            this.CFCondition = GetContentFinderCondition(id) ?? throw new ArgumentException(Constants.ExceptionMessages.NoContentFinderCondition);
            this.CFConditionTransient = GetContentFinderConditionTransient(id) ?? throw new ArgumentException(Constants.ExceptionMessages.NoContentFinderConditionTransient);
            this.Instance = GetInstanceContent(id) ?? throw new ArgumentException(Constants.ExceptionMessages.NoInstanceContent);
        }
    }
}
