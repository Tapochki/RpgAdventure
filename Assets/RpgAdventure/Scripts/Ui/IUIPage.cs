using TandC.RpgAdventure.Services;

namespace TandC.RpgAdventure.Ui
{
    public interface IUIPage
    {
        bool IsActive { get; }
        void Init(IUIService uiService, DataService dataService);
        void Show();
        void Hide();
        void Update();
        void Dispose();
    }
}