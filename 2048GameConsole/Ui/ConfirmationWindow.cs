using Game2048.InputSystem;
using Game2048.Ui;

namespace _2048GameConsole.Ui;

public class ConfirmationWindow : BaseWindow, IInputHandler
{
    public bool InputEnabled { get; private set; }
    List<IInputHandler> IInputHandler.InputChildren { get; } = new();
    
    private string _titleText;
    private Action _callback;
    
    public void Show(string titleText, Action callback)
    {
        base.Show();
        InputEnabled = true;
        _titleText = titleText;
        _callback = callback;
    }

    public override void Update()
    {
        Console.WriteLine();    
        Console.WriteLine(_titleText);    
    }
    
    ETranslateResult IInputHandler.TranslateCommand(ECommand command)
    {
        if (command.Is(ECommand.Confirm))
        {
            _callback?.Invoke();
            Close();
        }
        else if (command.Is(ECommand.Discard))
        {
            Close();
        }
        
        return ETranslateResult.BlockAll;
    }

    public override void Close()
    {
        base.Close();

        InputEnabled = false;
        _callback = null;
        
        if (string.IsNullOrEmpty(_titleText))
            return;
        
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }
}