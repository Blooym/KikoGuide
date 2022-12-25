using System;

namespace KikoGuide.IPC.Interfaces
{
    /// <summary>
    ///     Provides a common interface that IPC providers must implement to be used by the plugin.
    /// </summary>
    internal interface IIPCProvider : IDisposable
    {
        /// <summary>
        ///     The ID of the IPC provider, used to identify it in the configuration and other places.
        /// </summary>
        IPCProviders ID { get; }

        /// <summary>
        ///    Whether or not the IPC provider is initialized.
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        ///     Whether or not the IPC provider is forcefully disabled.
        /// </summary>
        bool ForcefullyDisabled { get; }

        /// <summary>
        ///     Enables the IPC provider, either initializing it immediately or subscribing to it being available.
        /// </summary>
        void Enable();
    }
}
