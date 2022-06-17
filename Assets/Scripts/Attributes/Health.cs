using GameDevTV.Utils;
using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    [System.Serializable]
    public class DamageEvent : UnityEvent<float> { }

    public class Health : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] float _percentageHPThreshold = 65f;
        [SerializeField] float _gainingHPPercentage = 70f;
        [SerializeField] DamageEvent _onTakeDamageEvent;
        [SerializeField] DamageEvent _onDieEvent;

        //private float _currentHealth = -1f;
        private LazyValue<float> _currentHealth;
        private Animator _animatorcontroller;
        private BaseStats _baseStats = null;
        private bool _haveDead = false;

        public bool HaveDead { get { return _haveDead; } }
        public float CurrentHealth { get { return _currentHealth.Value; } }

        private void Awake()
        {
            _animatorcontroller = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();
            _currentHealth = new LazyValue<float>(InitHealth);
        }


        private void OnEnable()
        {
            if (_baseStats != null)
            {
                _baseStats.OnLevelUp += RegenerateHealth;
            }
        }

        private float InitHealth()
        {
            return _baseStats.GetStat(Status.Health);
        }

        private void OnDisable()
        {
            if (_baseStats != null)
            {
                _baseStats.OnLevelUp -= RegenerateHealth;
            }
        }

        private void Start()
        {
            _currentHealth.ForceInit();
            //if (_currentHealth < 0)
            //{
            //    _currentHealth = _baseStats.GetStat(Status.Health);
            //}
        }

        public float GetPercentageHealth(float scale)
        {
            return (_currentHealth.Value / _baseStats.GetStat(Status.Health)) * scale;
        }

        public void TakeDamage(GameObject instigator, float amount)
        {
            _currentHealth.Value = Mathf.Max(_currentHealth.Value - amount, 0f);

            if (_currentHealth.Value <= 0)
            {
                Die();
                _onDieEvent.Invoke(0);
                AwardInstigator(instigator);
            }
            else
            {
                _onTakeDamageEvent.Invoke(amount);
            }
        }

        public float GetLevelMaxHP()
        {
            return _baseStats.GetStat(Status.Health);
        }

        private void RegenerateHealth()
        {
            float hpMaxLevelUp = GetLevelMaxHP();

            if(GetPercentageHealth(100f) >= _percentageHPThreshold)
            {
                _currentHealth.Value = hpMaxLevelUp;
            }
            else
            {
                _currentHealth.Value = hpMaxLevelUp * _gainingHPPercentage / 100f;
            }
        }

        public void RecoverPoints(float hpToAdd)
        {
            _currentHealth.Value = Mathf.Min(_currentHealth.Value + hpToAdd, GetLevelMaxHP());
        }

        private void AwardInstigator(GameObject instigator)
        {
            Experience xpWinner = instigator.GetComponent<Experience>();
            if (xpWinner == null) { return; }
            
            xpWinner.GainXP(_baseStats.GetStat(Status.ExperienceReward));
        }

        private void Die()
        {
            if(_haveDead) { return; }

            _haveDead = true;
            _animatorcontroller.SetTrigger("isDead");

            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Collider>().enabled = false;

            //por el tema de que en Fighter se calcula un tiempo de entre peleas, entcoes para desactivarlo
            //no es necesario pero para dejarlo por si acaso
            //GetComponent<Fighter>().enabled = false;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_currentHealth.Value);
        }

        public void RestoreFromJToken(JToken state)
        {
            _currentHealth.Value = state.ToObject<float>();

            if (_currentHealth.Value <= 0)
            {
                Die();
            }
        }

        /*
         * ISaveable Implementation
         * 
        public object CaptureState()
        {
            
            //EstadoSalud currentHealthState = new EstadoSalud();
            //currentHealthState.currentHealth = totalHealth;
            //currentHealthState.isReallyDead = haveDead;
            
            return totalHealth;
        }

        public void RestoreState(object state)
        {
            
            //EstadoSalud restoreHealthStatus = (EstadoSalud)state;
            //totalHealth = restoreHealthStatus.currentHealth;
            //haveDead = restoreHealthStatus.isReallyDead;
            
            
            totalHealth = (float)state;
            
            if (totalHealth <= 0)
            {
                Die();
            }


        }
        */


    }

    /*

    [System.Serializable]
    public struct EstadoSalud
    {
        public float currentHealth;
        public bool isReallyDead;
    }
    */
}
