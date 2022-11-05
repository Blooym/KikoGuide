using System;

namespace KikoGuide.Attributes
{
    /// <summary>
    ///     A localizable name attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class NameAttribute : Attribute
    {
        /// <summary>
        ///     The name of the attribute.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Creates a new <see cref="NameAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        public NameAttribute(string name) => this.Name = name;
    }

    /// <summary>
    ///     A localizable description attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        ///     The description of the attribute.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Creates a new <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <param name="description">The description of the attribute.</param>
        public DescriptionAttribute(string description) => this.Description = description;
    }

    public static class AttributeExtensions
    {
        public static string GetNameAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(NameAttribute), false);
            return attribute?.Length > 0 ? ((NameAttribute)attribute[0]).Name : value.ToString();
        }

        public static string GetDescriptionAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribute?.Length > 0 ? ((DescriptionAttribute)attribute[0]).Description : value.ToString();
        }
    }
}