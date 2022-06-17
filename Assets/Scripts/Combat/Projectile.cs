using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _speedProjectile = 1f;
        [SerializeField] bool _isHomingProjectile = false;
        [SerializeField] GameObject _hitEffect = null;
        [SerializeField] float _maxLifeTime = 8f;
        [SerializeField] float _timeLifeAfterImpact = 2f;
        [SerializeField] GameObject[] _elementDestroyAfterImpact = null;
        [SerializeField] UnityEvent _onShootProjectile = null;
        [SerializeField] UnityEvent _onHitProjectile = null;

        CapsuleCollider colliderTarget;
        Health _target = null;
        GameObject _instigator = null;
        float _damage = 0f;

        void Update()
        {
            if (_target == null) { return; }

            if (_isHomingProjectile && !_target.HaveDead)
            {
                gameObject.transform.LookAt(GetAimLocation());
            }
            gameObject.transform.Translate(_speedProjectile * Vector3.forward * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage, GameObject instigator)
        {
            _onShootProjectile.Invoke();

            this._target = target;
            this._damage = damage;
            this._instigator = instigator;

            SetLookAtStart();
            Destroy(gameObject, _maxLifeTime);
        }

        private void SetLookAtStart()
        {
            colliderTarget = _target.GetComponent<CapsuleCollider>();
            gameObject.transform.LookAt(GetAimLocation());
        }

        private Vector3 GetAimLocation()
        {
            if (colliderTarget == null)
            {
                return _target.transform.position;
            }
            return _target.transform.position + colliderTarget.center;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_target.HaveDead) { return; }

            Health healthTarget = other.GetComponent<Health>();
            if (healthTarget != _target) { return; }
            _target.TakeDamage(_instigator, _damage);

            _speedProjectile = 0f;
            StartHitEffect();
            Destroy(gameObject, _timeLifeAfterImpact);
        }

        private void StartHitEffect()
        {
            foreach(GameObject element in _elementDestroyAfterImpact)
            {
                Destroy(element);
            }

            if (_hitEffect != null)
            {
                Instantiate(_hitEffect, transform.position, Quaternion.identity);
            }

            //Sonido Hit
            _onHitProjectile.Invoke();
        }
    }
}
