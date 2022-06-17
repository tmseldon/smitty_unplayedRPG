using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class EnemyBarHealth : MonoBehaviour
    {
        [SerializeField] RectTransform _barHealth = null;
        [SerializeField] Health _enemyHealth = null;   
        [SerializeField] Canvas _canvasEnemyBar = null;

        void Update()
        {
            UpdateBarHealth();
        }

        private void UpdateBarHealth()
        {
            float enemyHealthRatio = _enemyHealth.GetPercentageHealth(1f);

            if (Mathf.Approximately(enemyHealthRatio, 0) || Mathf.Approximately(enemyHealthRatio, 1))
            {
                _canvasEnemyBar.enabled = false;
                return;
            }

            _canvasEnemyBar.enabled = true;
            _barHealth.transform.localScale = new Vector3(enemyHealthRatio, 1f, 1f);
        }
    }

}


