namespace Game2048.InputSystem;

public class UiInputRoot : InputNode
{
    public UiInputRoot()
    {
        Enabled = true;
    }
    
    public void Add(InputNode node)
    {
        _children.Add(node);
    }

    protected override ETranslateResult TranslateCommand(ECommand command)
    {
        return ETranslateResult.Ignore;
    }
}