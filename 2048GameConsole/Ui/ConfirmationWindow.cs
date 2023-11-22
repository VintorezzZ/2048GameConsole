using Game2048;
using Game2048.InputSystem;
using Game2048.Ui;

namespace _2048GameConsole.Ui;

public class ConfirmationWindow : BaseWindow
{
    private string _titleText;
    private Action _callback;
    
    public void Show(string titleText, Action callback)
    {
        base.Show();

        _titleText = titleText;
        _callback = callback;
    }

    public override void Update()
    {
        Console.WriteLine();    
        Console.WriteLine(_titleText);    
    }

    protected override ETranslateResult TranslateCommand(ECommand command)
    {
        if (command.Is(ECommand.Confirm))
        {
            _callback?.Invoke();
            Close();
        }
        else if (command.Is(ECommand.Exit))
        {
            Close();
        }
        
        return ETranslateResult.BlockAll;
    }

    public override void Close()
    {
        base.Close();

        _callback = null;
        
        if (string.IsNullOrEmpty(_titleText))
            return;
        
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }
}