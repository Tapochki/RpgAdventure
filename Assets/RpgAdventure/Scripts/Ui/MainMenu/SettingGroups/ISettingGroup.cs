using TandC.RpgAdventure.Services;

namespace TandC.RpgAdventure.UI.MainMenu.SettingGroups
{
    public interface ISettingGroup
    {
        bool IsModified { get; }
        bool WasSaved { get; }
        void Init(IUIService uiService, DataService dataService);
        void Show();
        void Hide();
        void Dispose();
    }
}