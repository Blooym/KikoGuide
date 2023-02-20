using System.IO;
using KikoGuide.Common;
using KikoGuide.Integrations.Interfaces;
using KikoGuide.Utility;
using Newtonsoft.Json;

namespace KikoGuide.Integrations
{
    internal abstract class IntegrationConfigurationBase : IIntegrationConfiguration
    {
        /// <inheritdoc />
        public abstract int Version { get; }

        /// <inheritdoc />
        public bool Enabled { get; set; }

        /// <inheritdoc />
        public void Save()
        {
            PathUtil.CreatePath(Constants.Directory.Integrations);

            var configJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(Path.Combine(Constants.Directory.Integrations, $"{this.GetType().Name}.json"), configJson);
        }

        /// <inheritdoc />
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
