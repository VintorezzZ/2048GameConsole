using Game2048;
using Game2048.InputSystem;
using Game2048.Ui;

namespace _2048GameConsole.Ui;

public class ConsoleScreenManager : BaseScreenManager<EScreenType>
{
    public static ConsoleScreenManager Instance { get; private set; }

    private readonly GameFieldScreen _gameFieldScreen;
    private readonly ConfirmationWindow _confirmationWindow;
    
    public ConsoleScreenManager(UiInputRoot uiInputRoot, IModelReader game, IGameFieldViewUpdater gameFieldViewUpdater)
    {
        Instance = this;
        
        _gameFieldScreen = new GameFieldScreen(gameFieldViewUpdater, game);
        _confirmationWindow = new ConfirmationWindow();
        
        Screens.Add(_gameFieldScreen.ScreenType, _gameFieldScreen);
        
        uiInputRoot.Add(_gameFieldScreen);
        uiInputRoot.Add(_confirmationWindow);
        
        game.OnGameStarted += () =>
        {
            CloseAll();
            ShowScreen(EScreenType.GameField);
        };
    }
    
    public void ShowConfirmationWindow(string title, Action callback)
    {
        _openedWindows.Add(_confirmationWindow);
        _confirmationWindow.Show(title, callback);
        _confirmationWindow.OnClose += () => RemoveWindowFromOpenList(_confirmationWindow);
    }
    
    protected override void CloseAll()
    {
        _confirmationWindow.Close();
        base.CloseAll();
    }
}