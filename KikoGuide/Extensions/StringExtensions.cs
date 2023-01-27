using System;

namespace KikoGuide.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Removes all proceeding and trailing whitespace and any duplicate whitespace.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A trimmed and squished string.</returns>
        public static string TrimAndSquish(this string str) => string.IsNullOrEmpty(str) ? string.Empty : string.Join(" ", str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }
}