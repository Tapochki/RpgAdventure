using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TandC.RpgAdventure.UI.MainMenu.SettingGroups
{
    public class AudioSettingGroup : ISettingGroup
    {
        public bool IsModified { get; private set; }

        public bool WasSaved { get; private set; }

        private GameObject _selfObject;

        private AudioMixer _mixer;

        public void Init()
        {
        }

        public void Show()
        {
        }

        public void Hide()
        {
        }
    }
}