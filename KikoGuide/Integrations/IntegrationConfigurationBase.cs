using System.IO;
using Dalamud.Plugin.Ipc;
using KikoGuide.Common;
using KikoGuide.Utility;
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
        /// Whether or not the integration is enabled and will try to find the activation <see cref="ICallGateSubscriber{TRet}"/>.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Saves the configuration to the file.
        /// </summary>
        public void Save()
        {
            PathUtil.CreatePath(Constants.Directory.Integrations);

            var configJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(Path.Combine(Constants.Directory.Integrations, $"{this.GetType().Name}.json"), configJson);
        }

        /// <summary>
        /// Loads the configuration from the file or cretaes a new one.
        /// </summary>
        public static T Load<T>() where T : IntegrationConfigurationBase, new()
        {
            PathUtil.CreatePath(Constants.Directory.Integrations);

            var configPath = Path.Combine(Constants.Directory.Integrations, $"{typeof(T).Name}.json");

            if (!File.Exists(configPath))
            {
                return new T();
            }

            var configJson = File.ReadAllText(configPath);
            return JsonConvert.DeserializeObject<T>(configJson) ?? new T();
        }
    }
}
