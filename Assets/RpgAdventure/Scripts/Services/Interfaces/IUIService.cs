using TandC.RpgAdventure.Ui;
using UnityEngine;

namespace TandC.RpgAdventure.Services
{
    public interface IUIService
    {
        GameObject Canvas { get; }
        Camera UICamera { get; }
        void Init();
        void RegisterPage(IUIPage page);
        void RegisterPopup(IUIPopup popup);
        void SetPage<T>() where T : IUIPage;
        void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup;
        void HidePopup<T>() where T : IUIPopup;
    }
}