using System;
using ImGuiNET;
using KikoGuide.Common;
using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;

namespace KikoGuide.Integrations
{
    internal abstract class IntegrationBase : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The user-friendly name of the integration.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The user-friendly description of the integration.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Whether or not the integration has failed to load and has been force-disabled until the next plugin reload.
        /// </summary>
        public bool ForceDisabled { get; protected set; }

        /// <summary>
        /// The configuration of the integration.
        /// </summary>
        public abstract IntegrationConfigurationBase Configuration { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="IntegrationBase" /> class.
        /// </summary>
        public IntegrationBase()
        {
            if (this.Configuration.Enabled)
            {
                this.Enable();
            }
        }

        /// <summary>
        /// The code to run when the integration is enabled, this should initialize any resources needed for the integration instead of doing it in a constructor.
        /// </summary>
        protected abstract void OnEnable();

        /// <summary>
        /// Enables the integration.
        /// </summary>
        public void Enable()
        {
            if (this.ForceDisabled)
            {
                return;
            }
            try
            {
                this.OnEnable();
            }
            catch (Exception ex)
            {
                BetterLog.Error($"{this.Name} integration failed to load: {ex}");
                this.Dispose();
                this.ForceDisabled = true;
            }
        }

        /// <summary>
        /// The code to run when the integration is disabled, this should dispose of any resources just like a normal dispose.
        /// </summary>
        protected abstract void OnDisable();

        /// <summary>
        /// Disables the integration.
        /// </summary>
        public void Disable()
        {
            if (this.ForceDisabled)
            {
                return;
            }
            try
            {
                this.OnDisable();
            }
            catch (Exception ex)
            {
                BetterLog.Error($"{this.Name} integration failed to unload: {ex}");
                this.Dispose(false);
                this.ForceDisabled = true;
            }
        }

        /// <summary>
        /// The UI to draw to configure the integration.
        /// </summary>
        protected virtual void DrawAction()
        {

        }

        /// <summary>
        /// Draw the UI to configure the integration.
        /// </summary>
        public void Draw()
        {
            // If the integration is force disabled, don't allow configuration.
            if (this.ForceDisabled)
            {
                SiGui.TextWrapped(Strings.Integrations_ForceDisabled);
                return;
            }

            // Draw settings that appear on all integrations.
            var enabled = this.Configuration.Enabled;
            if (ImGui.Checkbox(Strings.Integrations_Enabled, ref enabled))
            {
                this.Configuration.Enabled = enabled;
                this.Configuration.Save();
                if (enabled)
                {
                    this.Enable();
                }
                else
                {
                    this.Disable();
                }
            }
            ImGui.Dummy(Spacing.SectionSpacing);

            ImGui.BeginDisabled(!this.Configuration.Enabled);
            this.DrawAction();
            ImGui.EndDisabled();
        }

        /// <summary>
        /// Disposes of the integration.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the integration.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (this.Configuration.Enabled)
                    {
                        this.Disable();
                    }
                }

                this.disposedValue = true;
            }
        }

        ~IntegrationBase()
        {
            this.Dispose(false);
        }
    }
}