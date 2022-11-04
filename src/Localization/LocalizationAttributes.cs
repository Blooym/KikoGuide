using System;
using CheapLoc;

namespace KikoGuide.Localization
{
    /// <summary>
    ///     A localizable name attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LocalizableNameAttribute : Attribute
    {
        public string Key { get; set; }
        public string Fallback { get; set; }

        public LocalizableNameAttribute(string key, string fallback)
        {
            Key = key;
            Fallback = fallback;
        }
    }

    /// <summary>
    ///     A localizable description attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LocalizableDescriptionAttribute : Attribute
    {
        public string Key { get; set; }
        public string Fallback { get; set; }

        public LocalizableDescriptionAttribute(string key, string fallback)
        {
            Key = key;
            Fallback = fallback;
        }
    }

    public static class LoCExtensions
    {
        public static string GetLocalizedName(this Enum value)
        {
            System.Reflection.FieldInfo? field = value.GetType().GetField(value.ToString());
            object[]? attribute = field?.GetCustomAttributes(typeof(LocalizableNameAttribute), false);
            return attribute?.Length > 0 ? Loc.Localize(((LocalizableNameAttribute)attribute[0]).Key, ((LocalizableNameAttribute)attribute[0]).Fallback) : value.ToString();
        }

        public static string GetLocalizedDescription(this Enum value)
        {
            System.Reflection.FieldInfo? field = value.GetType().GetField(value.ToString());
            object[]? attribute = field?.GetCustomAttributes(typeof(LocalizableDescriptionAttribute), false);
            return attribute?.Length > 0 ? Loc.Localize(((LocalizableDescriptionAttribute)attribute[0]).Key, ((LocalizableDescriptionAttribute)attribute[0]).Fallback) : value.ToString();
        }
    }
}