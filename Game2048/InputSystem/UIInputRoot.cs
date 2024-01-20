namespace Game2048.InputSystem;

public class UiInputRoot : IInputHandler
{
    public bool InputEnabled => true;
    List<IInputHandler> IInputHandler.InputChildren { get; } = new();

    public void Add(IInputHandler handler)
    {
        ((IInputHandler) this).InputChildren.Add(handler);
    }

    ETranslateResult IInputHandler.TranslateCommand(ECommand command)
    {
        return ETranslateResult.Ignore;
    }
}