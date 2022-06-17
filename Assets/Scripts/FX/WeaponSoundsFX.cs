using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.FX
{
    public class WeaponSoundsFX : MonoBehaviour
    {
        [SerializeField] List<AudioClip> _listShootSounds = new List<AudioClip>();
        [SerializeField] List<AudioClip> _listHitSounds= new List<AudioClip>();

        AudioSource _audioSource = null;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayShootSound()
        {
            if (_listShootSounds.Count == 0) { return; }

            int randomIndex = Random.Range(0, _listShootSounds.Count);
            _audioSource.clip = _listShootSounds[randomIndex];
            _audioSource.Play();
        }

        public void PlayHitSound()
        {
            if (_listHitSounds.Count == 0) { return; }

            int randomIndex = Random.Range(0, _listHitSounds.Count);
            _audioSource.clip = _listHitSounds[randomIndex];
            _audioSource.Play();
        }
    }

}
