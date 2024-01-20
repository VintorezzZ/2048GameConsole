using Game2048.GameField;
using Game2048.InputSystem;

namespace Game2048.GameModel;

public class Model : IModelReadonly, IInputHandler
{
    private const int FINISH_VALUE = 2048;

    public bool InputEnabled { get; private set; }
    List<IInputHandler> IInputHandler.InputChildren { get; } = new();
    
    private IGridMover<int> _gridMover;
    private List<(int, int)> _emptyGridCells;
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
        InputEnabled = true;
        Grid = new Grid<int>(gridSize, gridSize, (_, _, _) => 0);
        _emptyGridCells = new List<(int, int)>(Grid.Lenght);
        
        _gridMover = new SymmetricGridMover();
        _gridMover.Init(Grid);
        _gridMover.OnCellMerged += UpdateScore;
        _gridMover.OnGridObjectChanged += OnGridObjectChangedHandler;
        
        EventHub.OnGameRestartRequest += Start;
    }

    public void Start()
    {
        for (int row = 0; row < Grid.Raws; row++)
        {
            for (int col = 0; col < Grid.Columns; col++)
                Grid[col, row] = 0;
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

    private void OnGridObjectChangedHandler(OnGridObjectChangedEventArgs data)
    {
        _gridChanged = true;

        OnGridObjectChanged?.Invoke(data.X, data.Y, Grid[data.X, data.Y]);

        if (Grid[data.X, data.Y] != FINISH_VALUE)
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

        for (int row = 0; row < Grid.Raws; row++)
        {
            for (int col = 0; col < Grid.Columns; col++)
            {
                if (Grid[col, row] == 0)
                    _emptyGridCells.Add((col, row));
            }
        }
    }
    
    private bool HasEmptyCell()
    {
        for (int row = 0; row < Grid.Raws; row++)
        {
            for (int col = 0; col < Grid.Columns; col++)
            {
                if (Grid[col, row] == 0)
                    return true;
            }
        }

        return false;
    }
    
    private void Move(EMoveDirection direction)
    {
        _gridMover.Move(direction);
        
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
        for (int row = 0; row < Grid.Raws; row++)
        {
            for (int col = 0; col < Grid.Columns; col++)
            {
                if (Grid[col, row] == 0)
                    continue;
                
                // Проверяем ячейку справа
                if (col < Grid.Columns - 1 && Grid[col, row] == Grid[col + 1, row])
                    return true;

                // Проверяем ячейку снизу
                if (row < Grid.Raws - 1 && Grid[col, row] == Grid[col, row + 1])
                    return true;
            }
        }

        return false;
    }

    private void UpdateScore(int newValue)
    {
        Score += newValue;
                
        if (Score > BestScore)
            BestScore = Score;
    }
    
    ETranslateResult IInputHandler.TranslateCommand(ECommand command)
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
        }

        return ETranslateResult.BlockAll;
    }
}