using Game2048;
using Game2048.InputSystem;
using Game2048.Ui;

namespace _2048GameConsole.Ui;

public enum EScreenType
{
    GameField,
}

public class ConsoleScreenManager : IScreenManager<EScreenType>
{
    private static ConsoleScreenManager _instance;
    public static ConsoleScreenManager Instance => _instance;

    private Dictionary<EScreenType, BaseScreen<EScreenType>> _screens => ((IScreenManager<EScreenType>) this).Screens;
    Dictionary<EScreenType, BaseScreen<EScreenType>> IScreenManager<EScreenType>.Screens { get; set; } = new();
    
    private readonly BaseScreen<EScreenType> _gameFieldScreen;
    private readonly ConfirmationWindow _confirmationWindow;

    private BaseScreen<EScreenType> _currentScreen;
    private readonly List<IScreen> _openedWindows = new();
    
    public ConsoleScreenManager(UIInputRoot uiInputRoot, IModelReader game, IGameFieldViewUpdater gameFieldViewUpdater)
    {
        _instance = this;
        
        _gameFieldScreen = new GameFieldScreen(gameFieldViewUpdater, game);
        _confirmationWindow = new ConfirmationWindow();
        
        _screens.Add(_gameFieldScreen.ScreenType, _gameFieldScreen);
        
        uiInputRoot.Add(_gameFieldScreen);
        uiInputRoot.Add(_confirmationWindow);
    }
    
    public void Update()
    {
        _currentScreen.Update();

        foreach (var window in _openedWindows)
            window.Update();
    }

    public void ShowScreen(EScreenType type)
    {
        CloseCurrentScreen();

        if (_screens.TryGetValue(type, out var screen))
        {
            _currentScreen = screen;
            _currentScreen.Show();
        }
    }

    public void ShowConfirmationWindow(string title, Action callback)
    {
        _openedWindows.Add(_confirmationWindow);
        _confirmationWindow.Show(title, callback);
        _confirmationWindow.OnClose += () => RemoveWindowFromOpenList(_confirmationWindow);
    }

    private void RemoveWindowFromOpenList(IScreen window)
    {
        if (_openedWindows.Contains(window))
            _openedWindows.Remove(window);
    }

    public void CloseCurrentScreen()
    {
        _currentScreen?.Close();
        _currentScreen = null;
    }

    public void CloseAll()
    {
        _gameFieldScreen.Close();
        _confirmationWindow.Close();
        _currentScreen = null;
    }
}