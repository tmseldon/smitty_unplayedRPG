using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.FX
{
    public class DestroyAfterFX : MonoBehaviour
    {
        [SerializeField] GameObject _targetToDestroy = null;
        ParticleSystem _particleSystem;

        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (!_particleSystem.IsAlive())
            {
                if(_targetToDestroy == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(_targetToDestroy);
                }
            }
        }
    }
}
