namespace KikoGuide.Interfaces;

using System;

/// <summary>
///     Provides a common interface that screens must implement for the window manager.
/// </summary>
interface IScreen : IDisposable
{
    void Draw();
    void Show();
    void Hide();
}