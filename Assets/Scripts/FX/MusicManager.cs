using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.FX
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] float _initVol = 0.15f;
        [SerializeField] float _timeToWaitSuscription = 0f;

        AudioSource _audioSource;
        PlayerPreferences _playerConfig;

        private void Start()
        {
            if(_timeToWaitSuscription == 0)
            {
                InitializeBGMManager();
            }
            else
            {
                StartCoroutine(DelayedInit());
            }
        }

        private void InitializeBGMManager()
        {
            _audioSource = GetComponent<AudioSource>();
            _playerConfig = FindObjectOfType<PlayerPreferences>();
            _playerConfig.ChangeSoundConfig += ChangeSoundLevel;
            _audioSource.volume = _playerConfig.GetVolumeLevel();
        }

        IEnumerator DelayedInit()
        {
            yield return new WaitForSeconds(_timeToWaitSuscription);
            InitializeBGMManager();
        }

        //private void OnDisable()
        //{
        //    _playerConfig.ChangeSoundConfig -= ChangeSoundLevel;
        //}

        public void StopOST()
        {
            _audioSource.Stop();
        }

        public void PauseOST()
        {
            _audioSource.Pause();
        }

        public void PlayOST()
        {
            _audioSource.Play();
        }

        private void ChangeSoundLevel(object sender, float newVolume)
        {
            _audioSource.volume = newVolume;
        }
    }
}
