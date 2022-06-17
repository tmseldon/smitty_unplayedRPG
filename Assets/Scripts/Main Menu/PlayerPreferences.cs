using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class PlayerPreferences : MonoBehaviour
    {
        [SerializeField] float _defaultVolume = 0.8f;

        const string VOLUME_KEY = "Player Volume";
        const float _minVolume = 0f;
        const float _maxVolume = 1f;

        public event EventHandler<float> ChangeSoundConfig;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(VOLUME_KEY))
            {
                PlayerPrefs.SetFloat(VOLUME_KEY, _defaultVolume);
            }
        }

        public void SetDefault()
        {
            SetVolumeLevel(_defaultVolume);
        }

        public float GetVolumeLevel()
        {
            return PlayerPrefs.GetFloat(VOLUME_KEY);
        }

        public void SetVolumeLevel(float powerSelected)
        {
            float setLevelTo = Mathf.Clamp(powerSelected, _minVolume, _maxVolume);
            PlayerPrefs.SetFloat(VOLUME_KEY, setLevelTo);
            OnChangeSoundConfig(PlayerPrefs.GetFloat(VOLUME_KEY));
        }

        protected virtual void OnChangeSoundConfig(float newVolume)
        {
            ChangeSoundConfig?.Invoke(this, newVolume);
        }
    }

}

