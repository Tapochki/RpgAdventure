using Newtonsoft.Json.Linq;
using TandC.RpgAdventure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using VContainer;

namespace TandC.RpgAdventure.UI.MainMenu.SettingGroups
{
    public class AudioSettingGroup : ISettingGroup
    {
        public bool IsModified { get; private set; }

        public bool WasSaved { get; private set; }

        private const string MASTER_VOLUME_NAME = "MasterVolume";
        private const string MUSIC_VOLUME_NAME = "MusicVolume";
        private const string SOUNDS_VOLUME_NAME = "SoundVolume";

        private GameObject _selfObject;

        private AudioMixer _mixer;

        private Slider _masterSlider;
        private Slider _musicSlider;
        private Slider _soundsSlider;

        private TextMeshProUGUI _masterValueText;
        private TextMeshProUGUI _musicValueText;
        private TextMeshProUGUI _soundsValueText;

        private IUIService _uiService;
        private DataService _dataService;

        public void Init(IUIService uiService, DataService dataService)
        {
            _uiService = uiService;
            _dataService = dataService;

            InitializeAllVariables();

            SetupSlider();

            ConfigureOnFirstStart();

            Hide();
        }

        public void Show()
        {
            _selfObject.SetActive(true);
        }

        public void Hide()
        {
            _selfObject.SetActive(false);
        }

        public void Dispose()
        {
            _masterSlider.onValueChanged.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
            _soundsSlider.onValueChanged.RemoveAllListeners();
        }

        private void InitializeAllVariables()
        {
            _selfObject = _uiService.Canvas.transform.Find("Page_Settings/Container_Audio").gameObject;

            _mixer = Resources.Load<AudioMixer>("Audio");

            _masterSlider = _selfObject.transform.Find("Master/Slider").GetComponent<Slider>();
            _musicSlider = _selfObject.transform.Find("Music/Slider").GetComponent<Slider>();
            _soundsSlider = _selfObject.transform.Find("Sounds/Slider").GetComponent<Slider>();

            _masterValueText = _selfObject.transform.Find("Master/Text_Value").GetComponent<TextMeshProUGUI>();
            _musicValueText = _selfObject.transform.Find("Music/Text_Value").GetComponent<TextMeshProUGUI>();
            _soundsValueText = _selfObject.transform.Find("Sounds/Text_Value").GetComponent<TextMeshProUGUI>();
        }

        private void ConfigureOnFirstStart()
        {
            if (_dataService.AppSettingsData.isFirstRun == true)
            {
                _mixer.SetFloat(MASTER_VOLUME_NAME, 0.0f);
                _mixer.SetFloat(MUSIC_VOLUME_NAME, 0.0f);
                _mixer.SetFloat(SOUNDS_VOLUME_NAME, 0.0f);
            }
            else
            {
                var data = _dataService.AppSettingsData;

                _mixer.SetFloat(MASTER_VOLUME_NAME, data.masterVolume);
                _mixer.SetFloat(MUSIC_VOLUME_NAME, data.musicVolume);
                _mixer.SetFloat(SOUNDS_VOLUME_NAME, data.soundVolume);
            }
        }

        private void SetupSlider()
        {
            _masterSlider.onValueChanged.AddListener(MasterSliderValueChangedHandler);
            _musicSlider.onValueChanged.AddListener(MusicSliderValueChangedHandler);
            _soundsSlider.onValueChanged.AddListener(SoundsSliderValueChangedHandler);
        }


        private void MasterSliderValueChangedHandler(float value)
        {
            _masterValueText.text = CalculateVolumePercent(value).ToString("0") + "%";
            _mixer.SetFloat(MASTER_VOLUME_NAME, value);
        }

        private void MusicSliderValueChangedHandler(float value)
        {
            _musicValueText.text = CalculateVolumePercent(value).ToString("0") + "%";
            _mixer.SetFloat(MUSIC_VOLUME_NAME, value);
        }

        private void SoundsSliderValueChangedHandler(float value)
        {
            _soundsValueText.text = CalculateVolumePercent(value).ToString("0") + "%";
            _mixer.SetFloat(SOUNDS_VOLUME_NAME, value);
        }

        private float CalculateVolumePercent(float current/*, float min, float max*/)
        {
            return Mathf.InverseLerp(-80f, 0f, current) * 100.0f;
        }
    }
}