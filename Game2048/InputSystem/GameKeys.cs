namespace Game2048.InputSystem;

public enum ECommand
{
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
    Restart,
    Exit,
    Confirm,
    Discard,
}

public static class ECommandExtension
{
    public static bool Is(this ECommand command, ECommand targetCommand)
    {
        return command == targetCommand;        
    }
}