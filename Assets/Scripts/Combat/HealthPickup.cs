using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] float _recoverHelathPoints = 0f;
        [SerializeField] float _timeUntilReSpawn = 5f;

        WaitForSeconds _waitTime;
        Collider _colliderPickup;
        AudioSource _audioSource = null;

        private void Awake()
        {
            _waitTime = new WaitForSeconds(_timeUntilReSpawn);
            _colliderPickup = GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PickUp(other.gameObject);
                _audioSource.Play();
            }
        }

        private void PickUp(GameObject subject)
        {
            if (_recoverHelathPoints > 0)
            {
                subject.GetComponent<Health>().RecoverPoints(_recoverHelathPoints);
            }

            StartCoroutine(HideForSeconds());
        }

        private IEnumerator HideForSeconds()
        {
            ShowPickup(false);
            yield return _waitTime;
            ShowPickup(true);
        }

        private void ShowPickup(bool state)
        {
            _colliderPickup.enabled = state;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(state);
            }
        }
    }
}
