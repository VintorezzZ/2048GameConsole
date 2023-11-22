namespace Game2048;

public static class EventHub
{
    public static event Action OnGameRestartRequest;

    public static void RequestGameRestart()
    {
        OnGameRestartRequest?.Invoke();
    }
}