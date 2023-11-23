using Game2048;

namespace _2048GameConsole;

public class ConsoleGameFieldViewUpdater : IGameFieldViewUpdater
{
    private readonly IGridReader<int> _grid;
    private readonly string _defaultRawString;

    public ConsoleGameFieldViewUpdater(IGridReader<int> grid)
    {
        _grid = grid;
        _defaultRawString = GetRowString();
    }

    public void Update()
    {
        PrintGrid();
    }
    
    private void PrintGrid()
    {
        for (int row = 0; row < _grid.Raws; row++)
        {
            Console.WriteLine(_defaultRawString);
    
            for (int col = 0; col < _grid.Columns; col++)
            {
                Console.Write("|");

                if (_grid[col, row] != 0)
                {
                    Console.ForegroundColor = GetNumberColor(_grid[col, row]);
                    Console.Write(_grid[col, row].ToString().PadLeft(4));
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("    ");
                }
            }
    
            Console.WriteLine("|");
        }
    
        Console.WriteLine(_defaultRawString);
    }

    private string GetRowString()
    {
        var visualString = string.Empty;
        
        for (int i = 0; i < _grid.Columns; i++)
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