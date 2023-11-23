namespace Game2048.InputSystem;

public abstract class InputNode : InputNodeAbstract
{
    protected enum ETranslateResult
    {
        Ignore,
        Block,
        BlockAll
    }

    public bool Enabled { get; protected set; }
    
    protected abstract ETranslateResult TranslateCommand(ECommand command);
    
    protected override void TranslateInput(List<ECommand> commands)
    {
        base.TranslateInput(commands);

        var j = 0;

        while (j < commands.Count)
        {
            var translateResult = TranslateCommand(commands[j]);

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
}