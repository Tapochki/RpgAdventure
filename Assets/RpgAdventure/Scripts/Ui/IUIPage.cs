namespace TandC.RpgAdventure.Ui
{
    public interface IUIPage
    {
        bool IsActive { get; }
        void Init();
        void Show();
        void Hide();
        void Update();
        void Dispose();
    }
}