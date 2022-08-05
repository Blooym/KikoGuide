namespace KikoGuide.Interfaces;

using System;

interface IScreen : IDisposable
{
    void Draw();
    void Show();
    void Hide();
}