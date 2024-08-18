using _2048GameConsole.Ui;
using Game2048;
using Game2048.GameModel;
using Game2048.InputSystem;

namespace _2048GameConsole;

public class ConsoleApplication : BaseApplication
{
    private const int BOARD_SIZE = 4;
    private Model _game;

    public override void Run()
    {
        CreateGame();
        
        base.Run();
        
        _game.Start();
    }
    
    private void CreateGame()
    {
        _game = new Model();
        _game.Init(BOARD_SIZE);
    }

    protected override void AddInputHandlers()
    {
        _inputTree.Add(_game);
        
        base.AddInputHandlers();
    }
    
    protected override void CreateScreenManager()
    {
        _screenManager = new ConsoleScreenManager(_uiInputRoot, _game, new ConsoleGameFieldViewUpdater(_game.Grid));
    }

    protected override IInputSource CreateInputSource()
    {
        return new ConsoleInputSource();
    }
}