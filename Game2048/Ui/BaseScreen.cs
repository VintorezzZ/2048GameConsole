using Game2048.InputSystem;

namespace Game2048.Ui;

public abstract class BaseScreen<TScreenType> : IScreen where TScreenType : struct, Enum
{
    public abstract TScreenType ScreenType { get; }
   
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