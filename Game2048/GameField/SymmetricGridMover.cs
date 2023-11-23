namespace Game2048.GameField;

public class SymmetricGridMover : BaseGridMover<int>
{
    private void ActualMoveLeft()
    {
        for (int row = 0; row < _grid.Raws; row++)
        {
            for (int col = 0; col < _grid.Columns; col++)
            {
                if (_grid[col, row] == 0)
                    continue;

                var targetCol = MoveCellLeft(row, col);
                MergeCells(row, targetCol);
            }
        }  
    }
    
    protected override void MoveLeft()
    {
        MoveLeftAndObserve();
    }
    
    protected override void MoveRight()
    {
        ReverseRow();
        MoveLeftAndObserve();
        ReverseRow();
    }

    protected override void MoveUp()
    {
        Transpose();
        MoveLeftAndObserve();
        Transpose();
    }

    protected override void MoveDown()
    {
        Transpose();
        MoveRight();
        Transpose();
    }
    
    private int MoveCellLeft(int row, int col)
    {
        var targetCol = FindEmptyCellOnTheLeft(row, col, col - 1);

        if (targetCol == col)
            return targetCol;
        
        _grid[targetCol, row] = _grid[col, row];
        _grid[col, row] = 0;
        
        return targetCol;
    }
    
    private int FindEmptyCellOnTheLeft(int currentRow, int currentCol, int targetCol)
    {
        if (!_grid.AreCoordsMatchesTheGrid(targetCol, currentRow))
            return currentCol;

        if (_grid[targetCol, currentRow] != 0)
            return currentCol;

        return FindEmptyCellOnTheLeft(currentRow, currentCol - 1, targetCol - 1);
    }

    private void MergeCells(int row, int targetCol)
    {
        var resultingCell = targetCol - 1;
        
        if (_grid.AreCoordsMatchesTheGrid(resultingCell, row))
        {
            if (_grid[resultingCell, row] == _grid[targetCol, row])
            {
                var newValue = _grid[resultingCell, row] * 2;
                _grid[resultingCell, row] = newValue;
                _grid[targetCol, row] = 0;
                
                CallCellMergedEvent(newValue);
            }
        }
    }
    
    private void MoveLeftAndObserve()
    {
        _grid.OnGridObjectChanged += OnGridObjectChangedHandler;
        ActualMoveLeft();
        _grid.OnGridObjectChanged -= OnGridObjectChangedHandler;
    }
    
    // Транспонирование матрицы
    private void Transpose()
    {
        for (int row = 0; row < _grid.Raws; row++)
        {
            for (int col = row; col < _grid.Columns; col++)
            {
                (_grid[col, row], _grid[row, col]) = (_grid[row, col], _grid[col, row]);

                // int temp = _grid[row, col];
                // _grid[row, col] = _grid[col, row];
                // _grid[col, row] = temp;
            }
        }
    }

    // Инвертирование строки игрового поля
    private void ReverseRow()
    {
        for (int row = 0; row < _grid.Raws; row++)
        {
            for (int col = 0; col < _grid.Columns / 2; col++)
            {
                (_grid[col, row], _grid[_grid.Columns - col - 1, row]) = (_grid[_grid.Columns - col - 1, row], _grid[col, row]);
            }
        }
    }
}