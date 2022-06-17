using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int _startingLevel = 1;
        [SerializeField] ClassType _characterClass;
        [SerializeField] Progression _progressionData = null;
        [SerializeField] bool _shouldUseBonus = false;
        [Header("FX LevelUp")]
        [SerializeField] GameObject _levelUpFX = null;
        [SerializeField] UnityEvent _onLevelUp = null;

        //private int _currentLevel = 0;
        private LazyValue<int> _currentLevel;
        private Experience _experienceSystem;

        public event Action OnLevelUp;

        private void Awake()
        {
            _experienceSystem = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            _currentLevel.ForceInit();
            UpdateXPToLevelUp();
        }

        private void OnEnable()
        {
            if (_experienceSystem != null)
            {
                _experienceSystem.OnExpirienceGained += UpdateLevel;
                _experienceSystem.OnExpirienceGained += UpdateXPToLevelUp;
            }
        }

        private void OnDisable()
        {
            if (_experienceSystem != null)
            {
                _experienceSystem.OnExpirienceGained -= UpdateLevel;
                _experienceSystem.OnExpirienceGained -= UpdateXPToLevelUp;
            }
        }


        public float GetStat(Status stat)
        {
            return (GetBaseStat(stat) + GetTotalAdditive(stat)) * (1 + GetTotalMultiply(stat)) ;
        }

        private float GetBaseStat(Status stat)
        {
            return _progressionData.GetStat(stat, _characterClass, _currentLevel.Value);
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > _currentLevel.Value)
            {
                _currentLevel.Value = newLevel;
                OnLevelUp();
                StartLevelUpFX();
            }
        }

        private void UpdateXPToLevelUp()
        {
            if(_characterClass != ClassType.Player) { return; }

            float experiencePoints = GetBaseStat(Status.ExperienceToLevelUp);
            _experienceSystem.ExperienceToLevelUp = experiencePoints;
        }

        private void StartLevelUpFX()
        {
            if (_levelUpFX != null)
            {
                Instantiate(_levelUpFX, transform);
                _onLevelUp.Invoke();
            }
        }

        public int GetLevel()
        {
            if(_currentLevel.Value < 1) 
            { 
                CalculateLevel(); 
            }
            return _currentLevel.Value;
        }

        private int CalculateLevel()
        {
            if(_experienceSystem == null) { return _startingLevel; }

            float currentXPTotal = _experienceSystem.ExperiencePoints;
            float[] xpPointsPerLevel = _progressionData.GetLevels(Status.ExperienceToLevelUp, _characterClass);
            int currentLevel = 1;

            for (int index = 0; index < xpPointsPerLevel.Length; index++)
            {
                if (currentXPTotal < xpPointsPerLevel[index]) { return currentLevel; }
                currentLevel++;
            }

            return currentLevel;
        }

        public float GetTotalAdditive(Status stat)
        {
            if(!_shouldUseBonus) { return 0f; }

            float totalAdditiveMod = 0f;
            
            foreach(IModifierProvider modProvider in GetComponents<IModifierProvider>())
            {
                foreach(float activeMod in modProvider.GetAdditiveMod(stat))
                {
                    totalAdditiveMod += activeMod;
                }
            }

            return totalAdditiveMod;
        }

        public float GetTotalMultiply(Status stat)
        {
            if (!_shouldUseBonus) { return 0f; }

            float totalMultiplyMod = 0f;
            bool hasBonusChanged = false;

            foreach (IModifierProvider modProvider in GetComponents<IModifierProvider>())
            {
                foreach (float activeMod in modProvider.GetMultiplyMod(stat))
                {
                    if(activeMod > 0 && !hasBonusChanged)
                    {
                        hasBonusChanged = true;
                        totalMultiplyMod = activeMod;
                    }
                    else if(activeMod > 0 && hasBonusChanged)
                    {
                        totalMultiplyMod *= activeMod;
                    }
                }
            }
            
            return totalMultiplyMod / 100f;
        }

        //*************
        //Version where Base Stat does not get bonus, maybe for the future
        //*************

        //public float GetTotalModifiers(Status stat)
        //{
        //    float totalAdditiveMod = 0f;
        //    float totalMultiplyMod = 1f;

        //    foreach (IModifierProvider modProvider in GetComponents<IModifierProvider>())
        //    {
        //        foreach (float activeMod in modProvider.GetAdditiveMod(stat))
        //        {
        //            totalAdditiveMod += activeMod;
        //        }
        //        foreach (float activeMod in modProvider.GetMultiplyMod(stat))
        //        {
        //            totalMultiplyMod *= activeMod;
        //        }
        //    }

        //    return totalAdditiveMod * totalMultiplyMod;
        //}
    }
    }