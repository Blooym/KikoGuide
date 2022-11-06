// using Dalamud.Plugin.Ipc;
// using KikoGuide.Base;
// using KikoGuide.IPC.Interfaces;

// namespace KikoGuide.IPC.Providers
// {
// #pragma warning disable IDE0051 // Remove unused private members
//     /// <summary>
//     ///     Provider for Tippy
//     /// </summary>
//     public sealed class TippyIPC : IIPCProvider
//     {
//         public IPCProviders ID { get; } = IPCProviders.Tippy;

//         /// <summary>
//         ///     API Version.
//         /// </summary>
//         public const string LabelProviderApiVersion = "Tippy.APIVersion";

//         /// <summary>
//         ///     IsInitialized state.
//         /// </summary>
//         public const string LabelProviderIsInitialized = "Tippy.IsInitialized";

//         /// <summary>
//         ///     Register Tip.
//         /// </summary>
//         public const string LabelProviderRegisterTip = "Tippy.RegisterTip";

//         /// <summary>
//         ///     Register Message.
//         /// </summary>
//         public const string LabelProviderRegisterMessage = "Tippy.RegisterMessage";

//         /// <summary>
//         ///     RegisterTip CallGateSubscriber.
//         /// </summary>
//         private readonly ICallGateSubscriber<string, bool>? registerTip;

//         /// <summary>
//         ///     IsInitialized CallGateSubscriber.
//         /// </summary>
//         private ICallGateSubscriber<bool>? isInitialized;

//         /// <summary>
//         ///     RegisterMessage CallGateSubscriber.
//         /// </summary>
//         private readonly ICallGateSubscriber<string, bool>? registerMessage;

//         public void Enable()
//         {
//             try
//             { Initialize(); }
//             catch { /* ignore*/ }

//             this.isInitialized = PluginService.PluginInterface.GetIpcSubscriber<bool>(LabelProviderIsInitialized);
//             this.isInitialized?.Subscribe(Initialize);
//         }

//         public void Dispose() => this.isInitialized?.Unsubscribe(Initialize);

//         private static void Initialize() { }

//         private static void RegisterTips() { }
//         private static void ShowMessage(string message) { }
//     }
// }
