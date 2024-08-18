using Game2048.InputSystem;
using Game2048.Ui;

namespace Game2048;

public abstract class BaseApplication
{
    protected InputManager _inputManager;
    protected UiInputRoot _uiInputRoot;
    protected InputTree _inputTree;
    protected IScreenManager<EScreenType> _screenManager;

    public virtual void Run()
    {
        CreateInputModule();
        CreateScreenManager();
    }

    public virtual void Update()
    {
        _inputManager.Update();
        _screenManager.Update();
    }
    
    protected void CreateInputModule()
    {
        _inputManager = new InputManager(CreateInputSource());
        _uiInputRoot = new UiInputRoot();
        _inputTree = new InputTree(_inputManager);
        AddInputHandlers();
    }

    protected virtual void AddInputHandlers()
    {
        _inputTree.Add(_uiInputRoot);
    }

    protected abstract void CreateScreenManager();
    protected abstract IInputSource CreateInputSource();
}