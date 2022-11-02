namespace KikoGuide.IPC
{
    using System;

    /// <summary> 
    ///     Contains all IPC IDs for the plugin which are automatically added to the UI.
    /// </summary>
    public enum IPCProviders
    {
        [IPCProviderName("Wotsit")]
        [IPCProviderDescription("IPC.Provider.Wotsit", "Integrate with Wotsit to provide search capibilities.")]
        Wotsit
    }

    /// <summary>
    ///     The name of the IPC provider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class IPCProviderNameAttribute : Attribute
    {
        public string Name { get; set; }
        public IPCProviderNameAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    ///     The description of the IPC provider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class IPCProviderDescriptionAttribute : Attribute
    {
        public string Key { get; private set; }
        public string Fallback { get; private set; }
        public IPCProviderDescriptionAttribute(string key, string fallback)
        {
            Key = key;
            Fallback = fallback;
        }
    }

    /// <summary>
    ///     IPCProvider extension methods.
    /// </summary>
    public static class IPCProvider
    {
        /// <summary>
        ///     Get the name of an IPC provider.
        /// </summary>
        /// <param name="provider"> The IPC provider to get the name of. </param>
        /// <returns> The name of the IPC provider. </returns>
        public static string GetName(this IPCProviders provider)
        {
            var field = provider.GetType().GetField(provider.ToString());
            var attribute = field?.GetCustomAttributes(typeof(IPCProviderNameAttribute), false);
            return attribute?.Length > 0 ? ((IPCProviderNameAttribute)attribute[0]).Name : provider.ToString();
        }

        /// <summary>
        ///     Get the description of an IPC provider.
        /// </summary>
        /// <param name="provider"> The IPC provider to get the description of. </param>
        /// <returns> The description of the IPC provider. </returns>
        public static string GetDescription(this IPCProviders provider)
        {
            var field = provider.GetType().GetField(provider.ToString());
            var attribute = field?.GetCustomAttributes(typeof(IPCProviderDescriptionAttribute), false);
            return attribute?.Length > 0 ? CheapLoc.Loc.Localize(((IPCProviderDescriptionAttribute)attribute[0]).Key, ((IPCProviderDescriptionAttribute)attribute[0]).Fallback) : provider.ToString();
        }
    }
}