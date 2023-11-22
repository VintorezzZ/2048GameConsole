namespace Game2048.InputSystem;

public sealed class InputTree : InputNodeAbstract
{
    private InputManager _inputManager;

    public InputTree(InputManager inputManager)
    {
        _inputManager = inputManager;
        _inputManager.TranslateDelegate = TranslateInput;
    }

    public void Add(InputNode node)
    {
        _children.Add(node);
    }
}