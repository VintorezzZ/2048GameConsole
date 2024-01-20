namespace Game2048.InputSystem;

public enum ETranslateResult
{
    Ignore,
    Block,
    BlockAll
}

public interface IInputHandler
{
    bool InputEnabled { get; }
    List<IInputHandler> InputChildren { get; }

    ETranslateResult TranslateCommand(ECommand command);
}