using System;
using System.IO;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using KikoGuide.Utility;
using Newtonsoft.Json;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.GuideSystem
{
    /// <summary>
    /// Represents the base class for all guide-type configurations.
    /// </summary>
    internal abstract class GuideConfigurationBase
    {
        /// <summary>
        /// The version of the guide configuration.
        /// </summary>
        public abstract int Version { get; }

        /// <summary>
        /// The user-friendly name of the guide-type to be displayed.
        /// </summary>
        [JsonIgnore]
        public abstract string Name { get; }

        /// <summary>
        /// The action to run to draw the configuration, this is wrapped in a try-catch in the base class.
        /// </summary>
        protected virtual void DrawAction()
        {
            ImGui.BeginDisabled();
            SiGui.TextWrapped(Strings.Guide_NoConfig);
            ImGui.EndDisabled();
        }

        /// <summary>
        /// Draws the configuration layout for the guide.
        /// </summary>
        public void Draw()
        {
            try
            {
                this.DrawAction();
            }
            catch (Exception)
            {
                SiGui.TextWrappedColoured(Colours.Error, Strings.Errors_DrawFailed);
            }
        }

        /// <summary>
        /// Saves the configuration to the file.
        /// </summary>
        public void Save()
        {
            PathUtil.CreatePath(Constants.Directory.Guides);
            var configJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(Path.Combine(Constants.Directory.Guides, $"{this.GetType().Name}.json"), configJson);
        }

        /// <summary>
        /// Loads the configuration from the file or cretaes a new one.
        /// </summary>
        public static T Load<T>() where T : GuideConfigurationBase, new()
        {
            PathUtil.CreatePath(Constants.Directory.Guides);

            var configPath = Path.Combine(Constants.Directory.Guides, $"{typeof(T).Name}.json");

            if (!File.Exists(configPath))
            {
                return new T();
            }

            var configJson = File.ReadAllText(configPath);
            return JsonConvert.DeserializeObject<T>(configJson) ?? new T();
        }
    }
}
