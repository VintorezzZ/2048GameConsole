namespace Game2048.Ui;

public interface IScreen
{
   event Action OnClose;

   void Show();
   void Update();
   void Close();
}