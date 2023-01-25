using System;

namespace KikoGuide.Extensions
{
    internal static class StringExtensions
    {
        public static string TrimAndSquish(this string str) => string.Join(" ", str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }
}