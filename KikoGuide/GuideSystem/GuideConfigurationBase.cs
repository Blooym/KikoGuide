using System;
using System.IO;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.GuideSystem.Interfaces;
using KikoGuide.Resources.Localization;
using KikoGuide.Utility;
using Newtonsoft.Json;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.GuideSystem
{
    /// <summary>
    ///     Represents the base class for all guide-type configurations.
    /// </summary>
    internal abstract class GuideConfigurationBase : IGuideConfiguration
    {
        /// <inheritdoc />
        public abstract int Version { get; }

        /// <inheritdoc />
        [JsonIgnore] public abstract string Name { get; }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Save()
        {
            PathUtil.CreatePath(Constants.Directory.Guides);
            var configJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(Path.Combine(Constants.Directory.Guides, $"{this.GetType().Name}.json"), configJson);
        }

        /// <inheritdoc />
        protected virtual void DrawAction()
        {
            ImGui.BeginDisabled();
            SiGui.TextWrapped(Strings.Guide_NoConfig);
            ImGui.EndDisabled();
        }

        /// <inheritdoc />
        internal static T Load<T>() where T : GuideConfigurationBase, new()
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
