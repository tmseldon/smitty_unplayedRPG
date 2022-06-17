using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText _damageCanvasPrefab = null;
        [Range(0f, 1f)]
        [SerializeField] float _offsetYMin = 0.1f;
        [Range(0f, 1f)]
        [SerializeField] float _offsetYMax = 0.8f;

        public void Spawn(float damageAmount)
        {
            Vector3 positionOffset = new Vector3(0, Random.Range(_offsetYMin, _offsetYMax), 0);
            //Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));

            positionOffset += transform.position;

            DamageText damageText = Instantiate(_damageCanvasPrefab, positionOffset, Quaternion.identity, transform);
            damageText.SetTextDamage(damageAmount);
        }
    }
}


