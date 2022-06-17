using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textCurrentHealth;
        [SerializeField] Slider _healthBar;

        private Health _playerHealth;

        void Awake()
        {
            _playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        void Update()
        {
            UpdateHealthData();
        }

        private void UpdateHealthData()
        {
            _textCurrentHealth.SetText(_playerHealth.CurrentHealth.ToString());
            _healthBar.value = _playerHealth.GetPercentageHealth(1f);
        }
    }
}


