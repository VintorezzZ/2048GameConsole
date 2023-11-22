namespace Game2048.InputSystem;

public delegate void TranslateDelegate(List<ECommand> commands);

public class InputManager
{
    public TranslateDelegate TranslateDelegate;

    private IInputSource _inputSource;
    private List<ECommand> _commandsList = new();
    private bool _haveNewInput;

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
        if (TranslateDelegate == null)
            return;

        TranslateDelegate(commandsList);
    }
}