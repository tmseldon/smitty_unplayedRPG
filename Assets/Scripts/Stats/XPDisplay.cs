using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class XPDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textCurrentXP;
        [SerializeField] Slider _xPBar;

        private Experience _experiencePlayer;

        void Awake()
        {
            _experiencePlayer = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        void Update()
        {
            UpdateXPPoints();
        }

        private void UpdateXPPoints()
        {
            _textCurrentXP.SetText(_experiencePlayer.ExperiencePoints.ToString());
            _xPBar.value = _experiencePlayer.ExperiencePoints/_experiencePlayer.ExperienceToLevelUp;
        }
    }


}
