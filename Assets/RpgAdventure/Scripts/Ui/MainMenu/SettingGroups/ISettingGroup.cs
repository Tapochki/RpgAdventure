namespace TandC.RpgAdventure.UI.MainMenu.SettingGroups
{
    public interface ISettingGroup
    {
        bool IsModified { get; }
        bool WasSaved { get; }
        void Init();
        void Show();
        void Hide();
    }
}