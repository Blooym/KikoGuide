using System.IO;
using Dalamud.Plugin.Ipc;
using KikoGuide.Common;
using Newtonsoft.Json;

namespace KikoGuide.Integrations
{
    internal abstract class IntegrationConfigurationBase
    {
        /// <summary>
        /// The version of the integration configuration.
        /// </summary>
        public abstract int Version { get; }

        /// <summary>
        /// The user-friendly name of the integration.
        /// </summary>
        [JsonIgnore]
        public abstract string Name { get; }

        /// <summary>
        /// Whether or not the integration is enabled and will try to find the activation <see cref="ICallGateSubscriber{TRet}"/>.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Saves the configuration to the file.
        /// </summary>
        public void Save()
        {
            CreatePath();

            var configJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(Path.Combine(Constants.IntegrationsDirectory, $"{this.Name}.json"), configJson);
        }

        /// <summary>
        /// Loads the configuration from the file or cretaes a new one.
        /// </summary>
        public static T Load<T>() where T : IntegrationConfigurationBase, new()
        {
            CreatePath();

            var configPath = Path.Combine(Constants.IntegrationsDirectory, $"{new T().Name}.json");

            if (!File.Exists(configPath))
            {
                return new T();
            }

            var configJson = File.ReadAllText(configPath);
            return JsonConvert.DeserializeObject<T>(configJson) ?? new T();
        }

        /// <summary>
        /// Creates the directory for the configuration files if it doesn't exist.
        /// </summary>
        private static void CreatePath()
        {
            if (!Directory.Exists(Constants.IntegrationsDirectory))
            {
                Directory.CreateDirectory(Constants.IntegrationsDirectory);
            }
        }
    }
}