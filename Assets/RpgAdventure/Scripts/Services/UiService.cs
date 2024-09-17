using System;
using System.Collections.Generic;
using TandC.RpgAdventure.Ui;
using UniRx;
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

        private IDisposable _updateSubscription;

        public void Init()
        {
            Canvas = GameObject.Find("CanvasUI");
            UICamera = GameObject.Find("CameraUI").GetComponent<Camera>();
            CanvasScaler = Canvas.GetComponent<CanvasScaler>();

            //_updateSubscription = Observable.EveryUpdate()
            //    .Where(_ => CurrentPage != null && CurrentPage.IsActive)
            //    .Subscribe(_ => CurrentPage.Update());
        }

        public void RegisterPage(IUIPage page)
        {
            if (!_uiPages.Contains(page))
            {
                _uiPages.Add(page);
                page.Init();
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
            if (CurrentPage != null)
            {
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
            {
                CurrentPopup.SetMainPriority();
            }

            if (message == null)
            {
                CurrentPopup.Show();
            }
            else
            {
                CurrentPopup.Show(message);
            }
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    _popup.Hide();
                    break;
                }
            }
        }

        public void Dispose()
        {
            foreach (var _popup in _uiPopups)
            {
                _popup.Dispose();
            }

            foreach (var _page in _uiPages)
            {
                _page.Dispose();
            }

            _updateSubscription?.Dispose();
        }
    }
}

