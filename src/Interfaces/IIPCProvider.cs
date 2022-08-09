namespace KikoGuide.Interfaces;

using System;
using KikoGuide.Managers.IPC;

public interface IIPCProvider : IDisposable
{
    IPCProviders ID { get; }
}