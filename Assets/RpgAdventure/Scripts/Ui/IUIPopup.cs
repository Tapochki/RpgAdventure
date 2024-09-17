namespace TandC.RpgAdventure.Ui
{
    public interface IUIPopup
    {
        bool IsActive { get; }
        void Init();
        void Show();
        void Show(object data);
        void Hide();
        void Update();
        void Dispose();
        void SetMainPriority();
    }
}