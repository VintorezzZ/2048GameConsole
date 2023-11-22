using Game2048;

namespace _2048GameConsole;

public class ConsoleGameFieldViewUpdater : IGameFieldViewUpdater
{
    public IGridReader<int> Grid { get; }

    public ConsoleGameFieldViewUpdater(IGridReader<int> grid)
    {
        Grid = grid;
    }

    public void Update()
    {
        PrintGrid();
    }
    
    private void PrintGrid()
    {
        for (int row = 0; row < Grid.Width; row++)
        {
            Console.WriteLine(GetRowString());
    
            for (int col = 0; col < Grid.Height; col++)
            {
                Console.Write("|");

                if (Grid[row, col] != 0)
                {
                    Console.ForegroundColor = GetNumberColor(Grid[row, col]);
                    Console.Write(Grid[row, col].ToString().PadLeft(4));
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("    ");
                }
            }
    
            Console.WriteLine("|");
        }
    
        Console.WriteLine(GetRowString());
    }

    private string GetRowString()
    {
        var visualString = string.Empty;
        
        for (int i = 0; i < Grid.Width; i++)
        {
            if (i == 0)
                visualString += "|--";
            else
                visualString += "--|--";
        }

        visualString += "--|";
        
        return visualString;
    }
    
    private ConsoleColor GetNumberColor(int number)
    {
        switch (number)
        {
            case 2:
                return ConsoleColor.Cyan;
            case 4:
                return ConsoleColor.Blue;
            case 8:
                return ConsoleColor.Magenta;
            case 16:
                return ConsoleColor.DarkMagenta;
            case 32:
            case 64:
                return ConsoleColor.DarkYellow;
            default:
                return ConsoleColor.Yellow;
        }
    }
}