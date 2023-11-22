namespace Game2048.InputSystem;

public delegate void TranslateDelegate(List<ECommand> commands);

public class InputManager
{
    public TranslateDelegate TranslateDelegate;

    private readonly IInputSource _inputSource;
    private readonly List<ECommand> _commandsList = new();

    public InputManager(IInputSource inputSource)
    {
        _inputSource = inputSource;
    }
    
    public void Update()
    {
        _inputSource.UpdateInput(_commandsList);
        DispatchInput(_commandsList);
    }
    
    private void DispatchInput(List<ECommand> commandsList)
    {
        TranslateDelegate?.Invoke(commandsList);
    }
}