namespace Game2048;

public interface IScreen
{
   event Action OnClose;

   void Show();
   void Update();
   void Close();
}