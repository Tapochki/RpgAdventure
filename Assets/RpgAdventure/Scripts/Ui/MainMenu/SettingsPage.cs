using System.Collections;
using System.Collections.Generic;
using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Ui;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TandC.RpgAdventure.UI.MainMenu
{
    public class SettingsPage : IUIPage
    {
        public bool IsActive { get; private set; }

        private GameObject _selfObject;

        private CanvasGroup _canvasGroup;

        private Button _audioButton;
        private Button _videoButton;
        private Button _graphicButton;
        private Button _keyMappingButton;
        private Button _languageButton;
        private Button _exitButton;

        [Inject] private UIService _uiService;

        public void Init()
        {
            _selfObject = _uiService.Canvas.transform.Find("Canvas/Page_Settings").gameObject;

            _canvasGroup = _selfObject.GetComponent<CanvasGroup>();

            GameObject containerCategories = _selfObject.transform.Find("Container_Categories").gameObject;

            _audioButton = containerCategories.transform.Find("Button_Audio").GetComponent<Button>();
            _videoButton = containerCategories.transform.Find("Button_Video").GetComponent<Button>();
            _graphicButton = containerCategories.transform.Find("Button_Graphic").GetComponent<Button>();
            _keyMappingButton = containerCategories.transform.Find("Button_KeyMapping").GetComponent<Button>();
            _languageButton = containerCategories.transform.Find("Button_Language").GetComponent<Button>();
            _exitButton = containerCategories.transform.Find("Button_Exit").GetComponent<Button>();

            _audioButton.onClick.AddListener(AudioButtonOnClickHandler);
            _videoButton.onClick.AddListener(VideoButtonOnClickHandler);
            _graphicButton.onClick.AddListener(GraphicButtonOnClickHandler);
            _keyMappingButton.onClick.AddListener(KeyMappingButtonOnClickHandler);
            _languageButton.onClick.AddListener(LanguageButtonOnClickHandler);
            _exitButton.onClick.AddListener(ExitButtonOnClickHandler);

            ChangeState(true);
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
            _audioButton.onClick.RemoveAllListeners();
            _videoButton.onClick.RemoveAllListeners();
            _graphicButton.onClick.RemoveAllListeners();
            _keyMappingButton.onClick.RemoveAllListeners();
            _languageButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();

            _selfObject = null;
            _canvasGroup = null;

            _audioButton = null;
            _videoButton = null;
            _graphicButton = null;
            _keyMappingButton = null;
            _languageButton = null;
            _exitButton = null;

            _uiService = null;
        }

        private void ChangeState(bool isOn)
        {
            IsActive = isOn;

            _canvasGroup.alpha = isOn ? 1.0f : 0.0f;
            _canvasGroup.interactable = isOn;
            _canvasGroup.blocksRaycasts = isOn;
        }

        private void AudioButtonOnClickHandler()
        {

        }

        private void VideoButtonOnClickHandler()
        {

        }

        private void GraphicButtonOnClickHandler()
        {

        }

        private void KeyMappingButtonOnClickHandler()
        {

        }

        private void LanguageButtonOnClickHandler()
        {

        }

        private void ExitButtonOnClickHandler()
        {

        }
    }
}