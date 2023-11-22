namespace Game2048.InputSystem;

public interface IInputSource
{
    void UpdateInput(List<ECommand> commands);
}