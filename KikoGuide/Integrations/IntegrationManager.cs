using System;
using System.Collections.Generic;
using System.Reflection;

namespace KikoGuide.Integrations
{
    internal sealed class IntegrationManager : IDisposable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IntegrationManager" /> class.
        /// </summary>
        public IntegrationManager() => this.Integrations = LoadIntegrations();

        /// <summary>
        ///     All loaded integrations.
        /// </summary>
        public HashSet<IntegrationBase> Integrations { get; } = new();

        /// <summary>
        ///     Disposes of all integrations and the manager.
        /// </summary>
        public void Dispose()
        {
            foreach (var integration in this.Integrations)
            {
                integration.Dispose();
            }
            this.Integrations.Clear();
        }

        /// <summary>
        ///     Loads all integrations.
        /// </summary>
        /// <returns></returns>
        private static HashSet<IntegrationBase> LoadIntegrations()
        {
            var loadedIntegrations = new HashSet<IntegrationBase>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(IntegrationBase)))
                {
                    loadedIntegrations.Add((IntegrationBase)type.GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>()));
                }
            }

            return loadedIntegrations;
        }
    }
}
