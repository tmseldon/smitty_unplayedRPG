using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Control
{
    public class ChangeVolume : MonoBehaviour
    {
        private Slider _sliderMusic;
        //private PlayerPreferences _playerConfig;

        private void Awake()
        {
            _sliderMusic = GetComponent<Slider>();
        }

        void Start()
        {
            //_playerConfig = FindObjectOfType<PlayerPreferences>();

            _sliderMusic.value = FindObjectOfType<PlayerPreferences>().GetVolumeLevel();
            _sliderMusic.onValueChanged.AddListener(delegate { ChangeTheVolume(_sliderMusic.value);});
        }

        private void ChangeTheVolume(float powervolume)
        {
            FindObjectOfType<PlayerPreferences>().SetVolumeLevel(powervolume);
        }
    }

}

