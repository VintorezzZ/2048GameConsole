using _2048GameConsole.Ui;
using Game2048.InputSystem;

namespace _2048GameConsole;

public class ConsoleApplication : BaseApplication
{
    protected override void CreateScreenManager()
    {
        _screenManager = new ConsoleScreenManager(_uiInputRoot, _game, new ConsoleGameFieldViewUpdater(_game.Grid));
    }

    protected override IInputSource CreateInputSource()
    {
        return new ConsoleInputSource();
    }
}