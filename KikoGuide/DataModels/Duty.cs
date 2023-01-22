using KikoGuide.Common;
using Lumina.Excel.GeneratedSheets;

#pragma warning disable IDE0051, IDE0044
namespace KikoGuide.DataModels
{
    public sealed record Duty
    {
        public ContentFinderCondition? CFCondition { get; init; }
        public ContentFinderConditionTransient? CFConditionTransient { get; init; }
        public InstanceContent? Instance { get; init; }

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
            // It seems that all sheets share ContentFinderCondition's ID.
            this.CFCondition = GetContentFinderCondition(id);
            this.CFConditionTransient = GetContentFinderConditionTransient(id);
            this.Instance = GetInstanceContent(id);
        }
    }
}