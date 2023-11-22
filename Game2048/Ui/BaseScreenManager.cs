namespace Game2048.Ui;

public abstract class BaseScreenManager<TScreenType> : IScreenManager<TScreenType> where TScreenType : struct, Enum
{
    protected Dictionary<TScreenType, IScreen> Screens => ((IScreenManager<TScreenType>) this).Screens;
    Dictionary<TScreenType, IScreen> IScreenManager<TScreenType>.Screens { get; set; } = new();
    
    protected IScreen _currentScreen;
    protected readonly List<IScreen> _openedWindows = new();
    
    public virtual void Update()
    {
        _currentScreen.Update();

        foreach (var window in _openedWindows)
            window.Update();
    }

    public virtual void ShowScreen(TScreenType type)
    {
        CloseCurrentScreen();

        if (Screens.TryGetValue(type, out var screen))
        {
            _currentScreen = screen;
            _currentScreen.Show();
        }
    }
    
    protected virtual void RemoveWindowFromOpenList(IScreen window)
    {
        if (_openedWindows.Contains(window))
            _openedWindows.Remove(window);
    }

    protected virtual void CloseCurrentScreen()
    {
        _currentScreen?.Close();
        _currentScreen = null;
    }

    protected virtual void CloseAll()
    {
        foreach (var (_, screen) in Screens)
            screen.Close();
        
        _currentScreen = null;
    }
}