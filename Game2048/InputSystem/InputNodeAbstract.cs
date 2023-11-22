namespace Game2048.InputSystem;

public abstract class InputNodeAbstract
{
    protected List<InputNode> _children = new();
    
    protected virtual void TranslateInput(List<ECommand> commands)
    {
        for (var i = _children.Count - 1; i >= 0; --i)
        {
            var child = _children[i];

            if (child == null)
            {
                _children.RemoveAt(i);
                i--;
            }
            else if (child.Enabled)
            {
                child.TranslateInput(commands);
            }
        }
    }
}