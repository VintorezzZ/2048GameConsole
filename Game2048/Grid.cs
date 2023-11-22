namespace Game2048;

public interface IGridReader<out TGridObject> : IGridChangeListener
{
    int Raws { get; }
    int Columns { get; }
    int Lenght { get; }
    
    TGridObject this [int x, int y] { get; }
}

public interface IGridWriter<TGridObject> : IGridChangeListener
{
    public TGridObject this[int x, int y] { get; set; }
}

public interface IGridChangeListener
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
}

public class OnGridObjectChangedEventArgs : EventArgs
{
    public int X;
    public int Y;
}

public class Grid<TGridObject> : IGridReader<TGridObject>, IGridWriter<TGridObject>
{
    private readonly int _raws;
    private readonly int _columns;
    private TGridObject[,] _gridArray;

    public int Raws => _raws;
    public int Columns => _columns;
    public int Lenght => _gridArray.Length;
    
    public event EventHandler<OnGridObjectChangedEventArgs>? OnGridObjectChanged;
    
    public TGridObject this[int x, int y]
    {
        get => GetGridObject(x, y);
        set => SetGridObject(x, y, value);
    }
    
    public Grid(int columns, int raws, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _columns = columns;
        _raws = raws;

        CreateGridNodes(createGridObject);
    }

    private void CreateGridNodes(Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _gridArray = new TGridObject[_columns, _raws];

        for (int x = 0; x < _columns; x++)
        {
            for (int y = 0; y < _raws; y++)
                _gridArray[x, y] = createGridObject(this, x, y);
        }
    }
    
    private void SetGridObject(int x, int y, TGridObject value)
    {
        if (AreCoordsMatchesTheGrid(x, y))
        {
            _gridArray[x, y] = value;
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { X = x, Y = y });
        }
    }
    
    private TGridObject GetGridObject(int x, int y)
    {
        return AreCoordsMatchesTheGrid(x, y) ? _gridArray[x, y] : default;
    }
    
    public bool AreCoordsMatchesTheGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _columns && y < _raws;
    }
}