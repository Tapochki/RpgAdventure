using System.Collections.Generic;
using TandC.RpgAdventure.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.RpgAdventure.Services 
{
    public class UIService : IUIService
    {
        private List<IUIPage> _uiPages;
        private List<IUIPopup> _uiPopups;

        public IUIPage CurrentPage { get; set; }
        public IUIPopup CurrentPopup { get; set; }

        public CanvasScaler CanvasScaler { get; set; }
        public GameObject Canvas { get; set; }

        public Camera UICamera { get; set; }

        public void Init()
        {
            Canvas = GameObject.Find("CanvasUI");
            UICamera = GameObject.Find("CameraUI").GetComponent<Camera>();
            CanvasScaler = Canvas.GetComponent<CanvasScaler>();

            foreach (var popup in _uiPopups)
                popup.Init();
        }

        public void RegisterPage(IUIPage page)
        {
            if (!_uiPages.Contains(page))
            {
                _uiPages.Add(page);

            }
        }

        public void RegisterPopup(IUIPopup popup)
        {
            if (!_uiPopups.Contains(popup))
            {
                _uiPopups.Add(popup);
                popup.Init();
            }
        }

        public void SetPage<T>() where T : IUIPage
        {
            {
                if (CurrentPage != null)
                    CurrentPage.Hide();
            }

            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    CurrentPage = _page;
                    break;
                }
            }
            CurrentPage.Show();
        }

        public void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup
        {
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    CurrentPopup = _popup;
                    break;
                }
            }

            if (setMainPriority)
                CurrentPopup.SetMainPriority();

            if (message == null)
                CurrentPopup.Show();
            else
                CurrentPopup.Show(message);
        }
    }
}

