namespace Game2048.GameField;

public enum EMoveDirection
{
    Left,
    Right,
    Up,
    Down
}

public interface IGridMover<TGridObject>
{
    event Action<int> OnCellMerged;
    event Action<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    
    void Init(Grid<TGridObject> grid);
    void Move(EMoveDirection direction);
}