using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Ui;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TandC.RpgAdventure.UI.MainMenu
{
    public class MainMenuPage : IUIPage
    {
        public bool IsActive { get; private set; }

        private GameObject _selfObject;

        private CanvasGroup _canvasGroup;

        private Button _startButton;
        private Button _continueButton;
        private Button _settingsButton;
        private Button _collectionsButton;
        private Button _upgradesButton;

        private IUIService _uiService;

        public void Init(IUIService uiService, DataService dataService)
        {
            _uiService = uiService;

            InitializeAllVariables();

            SetupAllButtons();

            Show();
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
            Debug.Log("Hehe, not hehe");
        }

        public void Dispose()
        {
            _startButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _collectionsButton.onClick.RemoveAllListeners();
            _upgradesButton.onClick.RemoveAllListeners();

            _selfObject = null;
            _canvasGroup = null;

            _startButton = null;
            _continueButton = null;
            _settingsButton = null;
            _collectionsButton = null;
            _upgradesButton = null;

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
            _selfObject = _uiService.Canvas.transform.Find("Page_MainMenu").gameObject;

            _canvasGroup = _selfObject.GetComponent<CanvasGroup>();

            GameObject containerButtons = _selfObject.transform.Find("Container_Buttons").gameObject;

            _startButton = containerButtons.transform.Find("Button_Start").GetComponent<Button>();
            _continueButton = containerButtons.transform.Find("Button_Continue").GetComponent<Button>();
            _settingsButton = containerButtons.transform.Find("Button_Settings").GetComponent<Button>();
            _collectionsButton = containerButtons.transform.Find("Button_Collections").GetComponent<Button>();
            _upgradesButton = containerButtons.transform.Find("Button_Upgrades").GetComponent<Button>();
        }

        private void SetupAllButtons()
        {
            _startButton.onClick.AddListener(StartButtonOnClickHandler);
            _continueButton.onClick.AddListener(ContinueButtonOnClickHandler);
            _settingsButton.onClick.AddListener(SettingsButtonOnClickHandler);
            _collectionsButton.onClick.AddListener(CollectionsButtonOnClickHandler);
            _upgradesButton.onClick.AddListener(UpgradesButtonOnClickHandler);
        }

        private void StartButtonOnClickHandler()
        {
        }

        private void ContinueButtonOnClickHandler()
        {
            // TODO - continue game
        }

        private void SettingsButtonOnClickHandler()
        {
            // TODO - add settings page script
            _uiService.SetPage<SettingsPage>();
        }

        private void CollectionsButtonOnClickHandler()
        {
            // TODO - add collections page script
            //_uiService.SetPage<>();
        }

        private void UpgradesButtonOnClickHandler()
        {
            // TODO - add upgrades page script
            //_uiService.SetPage<>();
        }
    }
}