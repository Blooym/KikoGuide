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
        ///    The plural name of the attribute.
        /// </summary>
        internal string PluralName { get; set; }

        /// <summary>
        ///     Creates a new <see cref="NameAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="pluralName">The plural name of the attribute.</param>
        internal NameAttribute(string name, string? pluralName = null)
        {
            this.Name = name;
            this.PluralName = pluralName ?? name;
        }
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
        ///     Gets the singular name of the attribute.
        /// </summary>
        /// <param name="value">The attribute to get the name of.</param>
        /// <returns>The singular name of the attribute.</returns>
        internal static string GetNameAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(NameAttribute), false);
            return attribute?.Length > 0 ? ((NameAttribute)attribute[0]).Name : value.ToString();
        }

        /// <summary>
        ///    Gets the plural name of the attribute.
        /// </summary>
        /// <param name="value">The attribute to get the plural name of.</param>
        /// <returns>The plural name of the attribute.</returns>
        internal static string GetPluralNameAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(NameAttribute), false);
            return attribute?.Length > 0 ? ((NameAttribute)attribute[0]).PluralName : value.ToString();
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
