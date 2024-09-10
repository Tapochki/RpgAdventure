namespace TandC.RpgAdventure.Ui
{
    public interface IUIPopup
    {
        void Init();
        void Show();
        void Show(object data);
        void Hide();
        void Dispose();
        void SetMainPriority();
    }
}