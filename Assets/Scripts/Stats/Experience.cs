using Newtonsoft.Json.Linq;
using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, IJsonSaveable
    {
        private float _experiencePoints = 0f;
        private float _experienceToLevelUp = 1f;
        public float ExperienceToLevelUp { 
            get 
            { 
                return _experienceToLevelUp; 
            }
            set
            {
                _experienceToLevelUp = value;
            }
        }

        public float ExperiencePoints { get { return _experiencePoints; } }
        public event Action OnExpirienceGained;

        public void GainXP(float experience)
        {
            _experiencePoints += experience;
            OnExpirienceGained();
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_experiencePoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            _experiencePoints = state.ToObject<float>();
        }
    }
}



