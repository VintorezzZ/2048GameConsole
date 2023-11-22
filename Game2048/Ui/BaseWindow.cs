using Game2048.InputSystem;

namespace Game2048.Ui;

public abstract class BaseWindow : InputNode, IScreen
{
    public event Action? OnClose;

    public virtual void Show()
    {
        Enabled = true;
    }

    public virtual void Update()
    {
      
    }

    public virtual void Close()
    {
        Enabled = false;
        OnClose?.Invoke();
    }
}