using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursor()
        {
            return CursorType.Attack;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            //Esto es por el AggroGroup Script, si se deshabilita este enemigo, no deber�a ser posible
            //atacarlo si no esta� en modo agresivo
            if(enabled == false) { return false; }

            Fighter fighterPlayer = callingController.GetComponent<Fighter>();

            if (!fighterPlayer.CanAttack(gameObject)) { return false; }

            if (Input.GetMouseButton(0))
            {
                fighterPlayer.Attack(gameObject);
            }
            
            return true;
        }
    }

}
