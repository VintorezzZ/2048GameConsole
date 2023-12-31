﻿using Game2048.GameModel;
using Game2048.InputSystem;
using Game2048.Ui;

namespace Game2048;

public abstract class BaseApplication
{
    private const int BOARD_SIZE = 4;

    protected Model _game;
    protected InputManager _inputManager;
    protected UiInputRoot _uiInputRoot;
    protected InputTree _inputTree;
    protected IScreenManager<EScreenType> _screenManager;

    public virtual void Run()
    {
        CreateGame();
        CreateInputModule();
        CreateScreenManager();

        _game.Start();
    }

    public virtual void Update()
    {
        _inputManager.Update();
        _screenManager.Update();
    }

    private void CreateGame()
    {
        _game = new Model();
        _game.Init(BOARD_SIZE);
    }

    private void CreateInputModule()
    {
        _inputManager = new InputManager(CreateInputSource());
        _uiInputRoot = new UiInputRoot();
        _inputTree = new InputTree(_inputManager);
        _inputTree.Add(_game);
        _inputTree.Add(_uiInputRoot);
    }

    protected abstract void CreateScreenManager();
    protected abstract IInputSource CreateInputSource();
}