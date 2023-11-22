using Game2048.InputSystem;

namespace Game2048;

public interface IModelReader
{
    event Action OnGameStarted;
    event Action OnGameOvered;
    event Action<int, int, int> OnGridObjectChanged;
    Grid<int> Grid { get; }
    int Score { get; }
    int BestScore { get; }
    bool IsGameOvered { get; }
    bool GameWon { get; }
}

public class Model : InputNode, IModelReader
{
    private enum EMoveDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    private List<(int, int)> _emptyGridCells;
    private int _gridSize;
    private bool _gridChanged;
    private readonly Random _random = new();

    public Grid<int> Grid { get; private set; }
    public int Score { get; private set; }
    public int BestScore { get; private set; }
    public bool IsGameOvered { get; private set; }
    public bool GameWon { get; private set; }

    public event Action OnGameStarted;
    public event Action OnGameOvered;
    public event Action<int, int, int> OnGridObjectChanged;
    
    public void Init(int gridSize)
    {
        Enabled = true;
        _gridSize = gridSize;
        Grid = new Grid<int>(_gridSize, _gridSize, (_, _, _) => 0);
        _emptyGridCells = new List<(int, int)>(Grid.Lenght);

        EventHub.OnGameRestartRequest += Start;
    }

    public void Start()
    {
        for (int i = 0; i < Grid.Width; i++)
        {
            for (int j = 0; j < Grid.Height; j++)
                Grid[i, j] = 0;
        }

        Score = 0;
        IsGameOvered = false;
        GameWon = false;
        
        InitGameField();
    }

    private void InitGameField()
    {
        AddRandomNumber();
        AddRandomNumber();
        
        OnGameStarted?.Invoke();
    }

    private void OnGridObjectChangedHandler(object sender, OnGridObjectChangedEventArgs data)
    {
        _gridChanged = true;

        OnGridObjectChanged?.Invoke(data.X, data.Y, Grid[data.X, data.Y]);

        if (Grid[data.X, data.Y] != 2048)
            return;
        
        IsGameOvered = true;
        GameWon = true;
        OnGameOvered?.Invoke();
    }
    
    private void AddRandomNumber()
    {
        UpdateEmptyCells();
        
        if (_emptyGridCells.Count == 0)
            return;
        
        var randomCell = _emptyGridCells[_random.Next(0, _emptyGridCells.Count)];
        var value = _random.Next(1, 3) * 2;
        Grid[randomCell.Item1, randomCell.Item2] = value;
    }

    private void UpdateEmptyCells()
    {
        _emptyGridCells.Clear();

        for (int i = 0; i < Grid.Width; i++)
        {
            for (int j = 0; j < Grid.Height; j++)
            {
                if (Grid[i, j] == 0)
                    _emptyGridCells.Add((i, j));
            }
        }
    }
    
    private bool HasEmptyCell()
    {
        for (int row = 0; row < Grid.Width; row++)
        {
            for (int col = 0; col < Grid.Height; col++)
            {
                if (Grid[row, col] == 0)
                    return true;
            }
        }

        return false;
    }

    protected override ETranslateResult TranslateCommand(ECommand command)
    {
        switch (command)
        {
            case ECommand.MoveLeft:
                Move(EMoveDirection.Left);
                break;
            case ECommand.MoveRight:
                Move(EMoveDirection.Right);
                break;
            case ECommand.MoveUp:
                Move(EMoveDirection.Up);
                break;
            case ECommand.MoveDown:
                Move(EMoveDirection.Down);
                break;
            case ECommand.Restart:
                Start();
                break;
            case ECommand.Exit:
                return ETranslateResult.Ignore;
                break;
        }

        return ETranslateResult.BlockAll;
    }

    private void Move(EMoveDirection direction)
    {
        switch (direction)
        {
            case EMoveDirection.Left:
                TryMoveLeft();
                break;
            case EMoveDirection.Right:
                MoveRight();
                break;
            case EMoveDirection.Up:
                MoveUp();
                break;
            case EMoveDirection.Down:
                MoveDown();
                break;
        }
        
        if (_gridChanged)
        {
            AddRandomNumber();
            _gridChanged = false;
        }
        else
        {
            if (HasEmptyCell() || CanMergeAnyCells())
                return;
            
            IsGameOvered = true;
            OnGameOvered?.Invoke();
        }
    }

    private bool CanMergeAnyCells()
    {
        for (int row = 0; row < Grid.Width; row++)
        {
            for (int col = 0; col < Grid.Height; col++)
            {
                if (Grid[row, col] == 0)
                    continue;
                
                // Проверяем ячейку справа
                if (col < Grid.Width - 1 && Grid[row, col] == Grid[row, col + 1])
                    return true;

                // Проверяем ячейку снизу
                if (row < Grid.Height - 1 && Grid[row, col] == Grid[row + 1, col])
                    return true;
            }
        }

        return false;
    }

    
    private int FindEmptyCell(int currentRow, int currentCol, int targetCol)
    {
        if (!Grid.AreCoordsMatchesTheGrid(currentRow, targetCol))
            return currentCol;

        if (Grid[currentRow, targetCol] != 0)
            return currentCol;

        return FindEmptyCell(currentRow, currentCol - 1, targetCol - 1);
    }

    private void UpdateScore(int newValue)
    {
        Score += newValue;
                
        if (Score > BestScore)
            BestScore = Score;
    }

    private void TryMoveLeft()
    {
        Grid.OnGridObjectChanged += OnGridObjectChangedHandler;
        MoveLeft();
        Grid.OnGridObjectChanged -= OnGridObjectChangedHandler;

    }
    
    private void MoveLeft()
    {
        for (int row = 0; row < Grid.Width; row++)
        {
            for (int col = 0; col < Grid.Height; col++)
            {
                if (Grid[row, col] == 0)
                    continue;

                var targetCol = MoveCell(row, col);
                MergeCells(row, targetCol);
            }
        }
    }

    private void MergeCells(int row, int targetCol)
    {
        var resultingCell = targetCol - 1;
        
        if (Grid.AreCoordsMatchesTheGrid(row, resultingCell))
        {
            if (Grid[row, resultingCell] == Grid[row, targetCol])
            {
                var newValue = Grid[row, resultingCell] * 2;
                Grid[row, resultingCell] = newValue;
                Grid[row, targetCol] = 0;
                
                UpdateScore(newValue);
            }
        }
    }

    private int MoveCell(int row, int col)
    {
        var targetCol = FindEmptyCell(row, col, col - 1);

        if (targetCol == col)
            return targetCol;
        
        Grid[row, targetCol] = Grid[row, col];
        Grid[row, col] = 0;
        
        return targetCol;
    }

    private void MoveRight()
    {
        ReverseRow();
        TryMoveLeft();
        ReverseRow();
    }

    private void MoveUp()
    {
        Transpose();
        TryMoveLeft();
        Transpose();
    }

    private void MoveDown()
    {
        Transpose();
        MoveRight();
        Transpose();
    }

    // Транспонирование матрицы
    private void Transpose()
    {
        for (int row = 0; row < Grid.Width; row++)
        {
            for (int col = row; col < Grid.Height; col++)
            {
                (Grid[row, col], Grid[col, row]) = (Grid[col, row], Grid[row, col]);

                // int temp = _grid[row, col];
                // _grid[row, col] = _grid[col, row];
                // _grid[col, row] = temp;
            }
        }
    }

    // Инвертирование строки игрового поля
    private void ReverseRow()
    {
        int size = Grid.Width;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size / 2; col++)
            {
                (Grid[row, col], Grid[row, size - col - 1]) = (Grid[row, size - col - 1], Grid[row, col]);

                // int temp = _grid[row, col];
                // _grid[row, col] = _grid[row, size - col - 1];
                // _grid[row, size - col - 1] = temp;
            }
        }
    }
}