namespace Game2048.GameField;

public abstract class BaseGridMover<TGridObject> : IGridMover<TGridObject>
{
    protected Grid<TGridObject> _grid;

    public event Action<int>? OnCellMerged;
    public event Action<OnGridObjectChangedEventArgs>? OnGridObjectChanged;
    
    public void Init(Grid<TGridObject> grid)
    {
        _grid = grid;
    }

    public void Move(EMoveDirection direction)
    {
        switch (direction)
        {
            case EMoveDirection.Left:
                MoveLeft();
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
    }
    
    protected abstract void MoveUp();
    protected abstract void MoveDown();
    protected abstract void MoveRight();
    protected abstract void MoveLeft();

    protected void CallCellMergedEvent(int value)
    {
        OnCellMerged?.Invoke(value);
    }
    
    protected void OnGridObjectChangedHandler(object sender, OnGridObjectChangedEventArgs data)
    {
        OnGridObjectChanged?.Invoke(data);
    }
}