using KikoGuide.Common;
using Lumina.Excel.GeneratedSheets;

namespace KikoGuide.Guides
{
    /// <summary>
    ///     Quick access to commonly used statuses.
    /// </summary>
    public static class Statuses
    {
        public static readonly Status Paralysis = Services.StatusCache.GetRow(17)!;
    }
}
