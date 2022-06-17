using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent _onMeleeHit = null;

        public void OnHit()
        {
            _onMeleeHit.Invoke();
        }
    }

}

