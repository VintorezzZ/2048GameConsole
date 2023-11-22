﻿namespace Game2048.Ui;

public interface IScreenManager<TScreenType> where TScreenType : struct, Enum
{
    Dictionary<TScreenType, BaseScreen<TScreenType>> Screens { get; set; }

    void Update();
    void ShowScreen(TScreenType type);
    void CloseCurrentScreen();
    void CloseAll();
}