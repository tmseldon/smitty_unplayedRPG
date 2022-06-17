using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    [CreateAssetMenuAttribute(fileName = "WeaponData", menuName = "Weapon/Create New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Weapon _weaponAtHand = null;
        [SerializeField] AnimatorOverrideController _animatorOverrideController = null;
        [SerializeField] float _distanceToAttack = 5f;
        [SerializeField] float _weaponDamage = 10f;
        [SerializeField] float _percentageBonus = 0f;
        [SerializeField] bool _isRightHanded = true;
        [SerializeField] Projectile _projectile = null;

        public float WeaponDamage
        {
            get { return _weaponDamage; }
        }

        public float PercentageBonus
        {
            get { return _percentageBonus; }
        }

        public float DistanceToAttack
        {
            get { return _distanceToAttack; }
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }

        public void LaunchProjectile(Transform rightHolder, Transform leftHolder, Health targetHealth, 
                                        GameObject instigator, float damage)
        {
            Transform placeHolder = GetWeaponHandSide(rightHolder, leftHolder);
            Projectile projectileLaunched = Instantiate(_projectile, placeHolder.position, Quaternion.identity);
            projectileLaunched.SetTarget(targetHealth, damage, instigator);
        }

        public GameObject Spawn(Transform placeHolderRight, Transform placeHolderLeft, Animator animator)
        {
            GameObject weapon = null;

            if (_weaponAtHand != null)
            {
                Transform placeHolder = GetWeaponHandSide(placeHolderRight, placeHolderLeft);
                Weapon weaponToCast = Instantiate(_weaponAtHand);
                weapon = weaponToCast.gameObject;
                weapon.transform.SetParent(placeHolder, false);
            }

            if (_animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
            else
            {
                //Debug.Log("no existe animación para este caso");
            }

            return weapon;
        }

        private Transform GetWeaponHandSide(Transform placeHolderRight, Transform placeHolderLeft)
        {
            Transform placeHolder = null;

            if (_isRightHanded)
            {
                placeHolder = placeHolderRight;
            }
            else
            {
                placeHolder = placeHolderLeft;
            }

            return placeHolder;
        }
    }
}
