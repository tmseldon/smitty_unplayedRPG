using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] bool _activateOnStart = false;
        [SerializeField] Fighter[] fighters;

        private void Start()
        {
            Activate(_activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach(Fighter eachFighter in fighters)
            {
                CombatTarget combatTarget = eachFighter.GetComponent<CombatTarget>();
                if (combatTarget != null)
                {
                    combatTarget.enabled = shouldActivate;
                }

                eachFighter.enabled = shouldActivate;
            }
        
        }

    }
}



