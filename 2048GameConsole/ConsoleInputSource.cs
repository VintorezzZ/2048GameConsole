namespace Game2048.InputSystem;

public class ConsoleInputSource : IInputSource
{
    public void UpdateInput(List<ECommand> commands)
    {
        commands.Clear();
        
        var input = Console.ReadKey(true);
        
        switch (input.Key)
        {
            case ConsoleKey.LeftArrow:
                commands.Add(ECommand.MoveLeft);
                break;
            case ConsoleKey.RightArrow:
                commands.Add(ECommand.MoveRight);
                break;
            case ConsoleKey.UpArrow:
                commands.Add(ECommand.MoveUp);
                break;
            case ConsoleKey.DownArrow:
                commands.Add(ECommand.MoveDown);
                break;
            case ConsoleKey.R:
                commands.Add(ECommand.Restart);
                break;
            case ConsoleKey.Q:
                commands.Add(ECommand.Exit);
                break;
            case ConsoleKey.Y:
                commands.Add(ECommand.Confirm);
                break;
            case ConsoleKey.N:
                commands.Add(ECommand.Discard);
                break;
        }
    }
}