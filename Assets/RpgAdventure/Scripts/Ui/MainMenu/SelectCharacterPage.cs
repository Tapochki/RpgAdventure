using System.Collections;
using System.Collections.Generic;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Ui;
using UnityEngine;

namespace TandC.RpgAdventure.UI.MainMenu
{
    public class SelectCharacterPage : IUIPage
    {
        public bool IsActive { get; private set; }

        private GameObject _selfObject;

        private CanvasGroup _canvasGroup;

        private IUIService _uiService;
        private DataService _dataService;

        public void Init(IUIService uiService, DataService dataService)
        {
            _uiService = uiService;
            _dataService = dataService;

            InitializeAllVariables();

            SetupButtons();

            Hide();
        }

        public void Show()
        {
            ChangeState(true);
        }

        public void Hide()
        {
            ChangeState(false);
        }

        public void Update()
        {
        }

        public void Dispose()
        {
            _selfObject = null;
            _canvasGroup = null;

            _uiService = null;
        }

        private void ChangeState(bool isOn)
        {
            IsActive = isOn;

            _canvasGroup.alpha = isOn ? 1.0f : 0.0f;
            _canvasGroup.interactable = isOn;
            _canvasGroup.blocksRaycasts = isOn;
        }

        private void InitializeAllVariables()
        {
            _selfObject = _uiService.Canvas.transform.Find("Page_Settings").gameObject;

            _canvasGroup = _selfObject.GetComponent<CanvasGroup>();
        }

        private void SetupButtons()
        {
        }
    }
}