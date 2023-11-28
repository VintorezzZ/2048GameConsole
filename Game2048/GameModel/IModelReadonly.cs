namespace Game2048.GameModel;

public interface IModelReadonly
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