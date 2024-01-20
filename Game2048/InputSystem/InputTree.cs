namespace Game2048.InputSystem;

public sealed class InputTree : IInputHandler
{
    bool IInputHandler.InputEnabled => true;
    List<IInputHandler> IInputHandler.InputChildren { get; } = new();

    public InputTree(InputManager inputManager)
    {
        inputManager.TranslateDelegate = HandleInput;
    }
    
    public void Add(IInputHandler handler)
    {
        ((IInputHandler) this).InputChildren.Add(handler);
    }
    
    private void HandleInput(List<ECommand> commands)
    {
        TranslateInputToChildren(commands, ((IInputHandler) this).InputChildren);
    }
    
    private void TranslateInputToChildren(List<ECommand> commands, List<IInputHandler> inputChildren)
    {
        for (var i = inputChildren.Count - 1; i >= 0; --i)
        {
            var child = inputChildren[i];

            if (child == null)
            {
                inputChildren.RemoveAt(i);
                i--;
            }
            else if (child.InputEnabled)
            {
                TranslateInput(commands, child);
            }
        }
    }

    private void TranslateInput(List<ECommand> commands, IInputHandler inputHandler)
    {
        TranslateInputToChildren(commands, inputHandler.InputChildren);

        var j = 0;

        while (j < commands.Count)
        {
            var translateResult = inputHandler.TranslateCommand(commands[j]);

            if (translateResult == ETranslateResult.Block)
            {
                commands.RemoveAt(j);
            }
            else if (translateResult == ETranslateResult.BlockAll)
            {
                commands.Clear();
                break;
            }
            else
            {
                j++;
            }
        }
    }
    
    public ETranslateResult TranslateCommand(ECommand command)
    {
        return ETranslateResult.Ignore;
    }
}