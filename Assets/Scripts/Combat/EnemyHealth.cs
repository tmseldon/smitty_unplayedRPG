using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealth : MonoBehaviour
    {
        private TextMeshProUGUI _textEnemyHealth;
        private Fighter _playerFighter;
        private Health _enemyHealth;

        private void Awake()
        {
            _textEnemyHealth = GetComponent<TextMeshProUGUI>();
            _playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            UpdateEnemyHealth();
        }

        private void UpdateEnemyHealth()
        {
            _enemyHealth = _playerFighter.GetTargetHealth();
            if (_enemyHealth == null)
            {
                _textEnemyHealth.SetText("Enemy: N/A");
                return;
            }

            _textEnemyHealth.SetText(string.Format("Enemy: {0:0.0}% {1:0.0}/{2:0.0}",
                                        _enemyHealth.GetPercentageHealth(100f), _enemyHealth.CurrentHealth,
                                        _enemyHealth.GetLevelMaxHP()));

        }

    }

}


