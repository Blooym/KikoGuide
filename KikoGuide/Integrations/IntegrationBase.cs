using System;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.Integrations
{
    internal abstract class IntegrationBase : IDisposable
    {
        /// <summary>
        /// Whether or not the integration has been disposed.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Whether or not the integration found the activation <see cref="ICallGateSubscriber{TRet}"/> and is registered to it.
        /// </summary>
        public abstract bool Activated { get; protected set; }

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
        /// The code to run when the integration is enabled.
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
                BetterLog.Error($"{this.Configuration.Name} integration failed to load: {ex}");
                this.Dispose();
                this.ForceDisabled = true;
            }
        }

        /// <summary>
        /// The code to run when the integration is disabled.
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
                BetterLog.Error($"{this.Configuration.Name} integration failed to unload: {ex}");
                this.Dispose(false);
                this.ForceDisabled = true;
            }
        }

        /// <summary>
        /// The UI to draw to configure the integration.
        /// </summary>
        protected virtual Action? DrawAction { get; }

        /// <summary>
        /// Draw the UI to configure the integration.
        /// </summary>
        public void Draw()
        {
            if (this.ForceDisabled)
            {
                ImGui.TextWrapped($"An error occured with this integration. It has been automatically disabled until the next plugin reload. Please check the log for more information.");
                return;
            }

            var enabled = this.Configuration.Enabled;
            if (ImGui.Checkbox("Enabled", ref enabled))
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

            if (this.DrawAction == null)
            {
                ImGui.TextDisabled($"No configuration available for {this.Configuration.Name}.");
                return;
            }

            this.DrawAction();
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
                    this.Disable();
                }

                this.disposedValue = true;
            }
        }
    }
}