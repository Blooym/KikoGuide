using System;

namespace KikoGuide.Attributes
{
    /// <summary>
    ///     A name attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class NameAttribute : Attribute
    {
        /// <summary>
        ///     The name of the attribute.
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        ///     Creates a new <see cref="NameAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        internal NameAttribute(string name) => this.Name = name;
    }

    /// <summary>
    ///     A plural name attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class PluralNameAttribute : Attribute
    {
        /// <summary>
        ///     The plural name of the attribute.
        /// </summary>
        internal string PluralName { get; set; }

        /// <summary>
        ///     Creates a new <see cref="PluralNameAttribute"/>.
        /// </summary>
        /// <param name="pluralName">The plural name of the attribute.</param>
        internal PluralNameAttribute(string pluralName) => this.PluralName = pluralName;
    }

    /// <summary>
    ///     A description attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class DescriptionAttribute : Attribute
    {
        /// <summary>
        ///     The description of the attribute.
        /// </summary>
        internal string Description { get; set; }

        /// <summary>
        ///     Creates a new <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <param name="description">The description of the attribute.</param>
        internal DescriptionAttribute(string description) => this.Description = description;
    }

    /// <summary>
    ///     Attribute extensions.
    /// </summary>
    internal static class AttributeExtensions
    {
        /// <summary>
        ///     Gets the name of the attribute.
        /// </summary>
        /// <param name="value">The attribute to get the name of.</param>
        internal static string GetNameAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(NameAttribute), false);
            return attribute?.Length > 0 ? ((NameAttribute)attribute[0]).Name : value.ToString();
        }

        /// <summary>
        ///     Gets the plural name of the attribute.
        /// </summary>
        /// <param name="value">The attribute to get the plural name of.</param>
        internal static string GetPluralNameAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(PluralNameAttribute), false);
            return attribute?.Length > 0 ? ((PluralNameAttribute)attribute[0]).PluralName : value.ToString();
        }

        /// <summary>
        ///     Gets the description of the attribute.
        /// </summary>
        /// <param name="value">The attribute to get the description of.</param>
        internal static string GetDescriptionAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribute?.Length > 0 ? ((DescriptionAttribute)attribute[0]).Description : value.ToString();
        }
    }
}
