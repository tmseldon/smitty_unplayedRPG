using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig _weaponData = null;
        [SerializeField] float _recoverHelathPoints = 0f;
        [SerializeField] float _timeUntilReSpawn = 5f;
        [SerializeField] UnityEvent _onPickup = null;

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
            if( other.gameObject.CompareTag("Player") )
            {
                PickUp(other.gameObject);
                _audioSource.Play();
            }
        }

        private void PickUp(GameObject subject)
        {
            if(_weaponData != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weaponData);
            }
            if(_recoverHelathPoints > 0)
            {
                subject.GetComponent<Health>().RecoverPoints(_recoverHelathPoints);
            }

            if(_onPickup != null)
            {
                _onPickup.Invoke();
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
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(state);
            }
        }

        public CursorType GetCursor()
        {
            return CursorType.Item;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp(callingController.gameObject);
                _audioSource.Play();
            }
            return true;
        }
    }
}
