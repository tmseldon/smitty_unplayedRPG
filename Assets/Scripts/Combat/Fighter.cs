 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IJsonSaveable, IModifierProvider
    {

        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] Transform _placeHolderWeaponRight = null;
        [SerializeField] Transform _placeHolderWeaponLeft = null;
        [SerializeField] WeaponConfig _defaultWeaponData = null;

        float timeSinceLastAttack = Mathf.Infinity;

        Health target;
        Mover mover;
        BaseStats baseStats;
        ActionScheduler actionScheduler;
        Animator animatorController;
        LazyValue<WeaponConfig> _currentWeaponData;
        GameObject _weaponEquipped = null;


        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animatorController = GetComponent<Animator>();

            _currentWeaponData = new LazyValue<WeaponConfig>(SetDefaultWeapon);
        }

        private void Start()
        {
            _currentWeaponData.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) { return; }
            if(target.HaveDead) { return; }

            //solo se deja asi para mostrar ejemplo de short-circuit
            if(target != null && IsOnZoneAttack(target.transform))
            {
                mover.Cancel();
                AttackBehaviour();
                return;
            }

            mover.MoveTo(target.transform.position, 1f);
        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack < timeBetweenAttack) { return; }

            transform.LookAt(target.transform);
            animatorController.ResetTrigger("cancelAttack");
            animatorController.SetTrigger("playerAttacks");
            timeSinceLastAttack = 0;
        }

        private bool IsOnZoneAttack(Transform targetTransform)
        {
            float currentDistance2Target = Vector3.Distance(targetTransform.position, transform.position);
            return currentDistance2Target <= _currentWeaponData.Value.DistanceToAttack;
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) { return false; };

            //En el caso de que este el player y enemigo separado por algun opbstculo fisico
            //y aún así lo ve, esta condicion permite seguir atacando con range
            if(!mover.CanMoveTo(combatTarget.transform.position) && !IsOnZoneAttack(combatTarget.transform)) 
            { 
                return false; 
            }

            Health healthTarget = GetComponent<Health>();
            return healthTarget !=null && !healthTarget.HaveDead; 
        }

        public void Cancel()
        {
            animatorController.ResetTrigger("playerAttacks");
            animatorController.SetTrigger("cancelAttack");
            target = null;
            mover.Cancel();
        }

        private WeaponConfig SetDefaultWeapon()
        {
            AttachWeapon(_defaultWeaponData);
            return _defaultWeaponData;
        }

        public void EquipWeapon(WeaponConfig newWeaponData)
        {
            if (newWeaponData == null) { return; }

            _currentWeaponData.Value = newWeaponData;
            AttachWeapon(newWeaponData);
        }

        private void AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();

            if (_weaponEquipped)
            {
                Destroy(_weaponEquipped);
            }
            _weaponEquipped = weapon.Spawn(_placeHolderWeaponRight, _placeHolderWeaponLeft, animator);
        }

        public Health GetTargetHealth()
        {
            return target;
        }


        //Animation Event
        private void Hit()
        {
            if (target == null) { return; }

            float totalDamage = baseStats.GetStat(Status.Damage);

            if (_weaponEquipped != null && !_currentWeaponData.Value.HasProjectile())
            {
                _weaponEquipped.GetComponent<Weapon>().OnHit();
            }

            if (_currentWeaponData.Value.HasProjectile())
            {
                _currentWeaponData.Value.LaunchProjectile(_placeHolderWeaponRight, _placeHolderWeaponLeft, 
                                                                target, gameObject, totalDamage);
            }
            else
            {
                target.TakeDamage(gameObject, totalDamage);
            }
        }

        private void Shoot() => Hit();

        //End Animation Event

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_currentWeaponData.Value.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            string weaponName = state.ToObject<string>();
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveMod(Status stat)
        {
            if(stat == Status.Damage)
            {
                yield return _currentWeaponData.Value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetMultiplyMod(Status stat)
        {
            if (stat == Status.Damage)
            {
                yield return _currentWeaponData.Value.PercentageBonus;
            }
        }
    }
}

