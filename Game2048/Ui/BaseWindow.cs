using Game2048.InputSystem;

namespace Game2048.Ui;

public abstract class BaseWindow : IScreen
{
    public event Action? OnClose;

    public virtual void Show()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Close()
    {
        OnClose?.Invoke();
    }
}