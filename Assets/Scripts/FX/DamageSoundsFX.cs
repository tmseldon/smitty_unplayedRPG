using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.FX
{
    public class DamageSoundsFX : MonoBehaviour
    {
        [SerializeField] List<AudioClip> _listDamageHits = new List<AudioClip>();
        [SerializeField] List<AudioClip> _listDieSounds = new List<AudioClip>();
        [SerializeField] AudioClip _levelUpSound = null;

        AudioSource _audioSource = null;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayDamageSound()
        {
            if(_listDamageHits.Count == 0) { return; }

            int randomIndex = Random.Range(0, _listDamageHits.Count);
            _audioSource.clip = _listDamageHits[randomIndex];
            _audioSource.Play();
        }

        public void PlayDieSound()
        {
            if(_listDieSounds.Count == 0) { return; }

            int randomIndex = Random.Range(0, _listDieSounds.Count);
            _audioSource.clip = _listDieSounds[randomIndex];
            _audioSource.Play();
        }

        public void PlayLevelUpSound()
        {
            if(_levelUpSound == null) { return; }

            _audioSource.clip = _levelUpSound;
            _audioSource.Play();
        }
    }

}


