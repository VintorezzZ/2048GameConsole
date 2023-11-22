namespace Game2048.Ui;

public enum EScreenType
{
    GameField,
}

public interface IScreenManager<TScreenType> where TScreenType : struct, Enum
{
    Dictionary<TScreenType, IScreen> Screens { get; set; }

    void Update();
    void ShowScreen(TScreenType type);
}