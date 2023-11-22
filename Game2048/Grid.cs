namespace Game2048;

public interface IGridReader<out TGridObject> : IGridChangeListener
{
    int Width { get; }
    int Height { get; }
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
    private readonly int _width;
    private readonly int _height;
    private TGridObject[,] _gridArray;

    public int Width => _width;
    public int Height => _height;
    public int Lenght => _gridArray.Length;
    
    public event EventHandler<OnGridObjectChangedEventArgs>? OnGridObjectChanged;
    
    public TGridObject this[int x, int y]
    {
        get => GetGridObject(x, y);
        set => SetGridObject(x, y, value);
    }
    
    public Grid(int width, int height, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        
        CreateGridNodes(createGridObject);
    }

    private void CreateGridNodes(Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _gridArray = new TGridObject[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
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
        return x >= 0 && y >= 0 && x < _width && y < _height;
    }
}