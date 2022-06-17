using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        TextMeshProUGUI _textDamage;

        private void Awake()
        {
            _textDamage = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetTextDamage(float damage)
        {
            _textDamage.SetText(string.Format("-{0:0}", damage));
        }

        //Animation Event
        public void OnEndAnimationDamageText()
        {
            Destroy(gameObject);
        }
    }
}

