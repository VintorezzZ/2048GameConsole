using Game2048;
using Game2048.GameModel;
using Game2048.InputSystem;
using Game2048.Ui;

namespace _2048GameConsole.Ui;

public class GameFieldScreen : BaseScreen<EScreenType>
{
    private readonly IModelReader _game;
    private readonly IGameFieldViewUpdater _gameFieldViewUpdater;
    
    public override EScreenType ScreenType => EScreenType.GameField;

    public GameFieldScreen(IGameFieldViewUpdater gameFieldViewUpdater, IModelReader game)
    {
        _gameFieldViewUpdater = gameFieldViewUpdater;
        _game = game;
    }
    
    public override void Show()
    {
        base.Show();
        
        Update();
    }

    public override void Update()
    {
        Console.Clear();
        Console.WriteLine($"Arrows - move grid \nR - restart \nQ - exit");
        Console.WriteLine();
        Console.WriteLine($"Score: {_game.Score}    BestScore {_game.BestScore}");
        Console.WriteLine();
        _gameFieldViewUpdater.Update();
        
        if (_game.IsGameOvered)
        {
            Console.WriteLine();
            Console.WriteLine(_game.GameWon ? "Congratulations, you won the game!" : "Game overed!");
        }
    }
    
    protected override ETranslateResult TranslateCommand(ECommand command)
    {
        if (command.Is(ECommand.Restart))
        {
            ConsoleScreenManager.Instance.ShowConfirmationWindow("Restart game? Y/N", EventHub.RequestGameRestart);
            return ETranslateResult.Block;
        }

        if (command.Is(ECommand.Exit))
        {
            ConsoleScreenManager.Instance.ShowConfirmationWindow("Quit game? Y/N", () => Environment.Exit(0));
            return ETranslateResult.Block;
        }

        return _game.IsGameOvered ? ETranslateResult.Block : ETranslateResult.Ignore;
    }
}