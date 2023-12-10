namespace Game2048.InputSystem;

public sealed class InputTree : InputNodeAbstract
{
    private InputManager _inputManager;

    public InputTree(InputManager inputManager)
    {
        _inputManager = inputManager;
        _inputManager.TranslateDelegate = TranslateInput;
    }

    public void Add(InputNode node) //interface // better naming of InoutNode, cuz it means that it is InputHandler // input system is not расширяемая
        // проникла во все части программы. нужно сделать ее обособленной, самостоятельной.
        // избавиться от наследования класса InputNode и сделать его интерфейсом, через которого можно абстрактно работать 
    {
        _children.Add(node);
    }
}