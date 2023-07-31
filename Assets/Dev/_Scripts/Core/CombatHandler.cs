using System.Collections;
using Newtonsoft.Json.Linq;
using RPG.Control;
using RPG.Core;
using RPG.Interfaces;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform weaponParent;
        [SerializeField] private Weapon defaultWeapon;

        public float WeaponRange => _currentWeapon.Range;

        private ControllerBase _character;
        private ActionScheduler _actionScheduler;
        private HealthHandler _target;
        private Weapon _currentWeapon;
        private Coroutine _attackCoroutine;
        private bool _isTriggered;

        private void OnEnable()
        {
            EquipWeapon(defaultWeapon);
        }

        public void Init(ControllerBase character, ActionScheduler scheduler)
        {
            _character = character;
            _actionScheduler = scheduler;
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (_currentWeapon != null)
                weaponParent.DestroyChildren();

            _currentWeapon = weapon;
            weapon.Init(weaponParent, _character);
        }

        public void Attack(HealthHandler target)
        {
            if (_isTriggered) return;

            _isTriggered = true;
            _target = target;
            _actionScheduler.StartAction(this);
            _attackCoroutine = StartCoroutine(AttackBehaviour());
        }

        public void Cancel()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _character.ProcessResetTrigger("attack");
                _character.ProcessSetTrigger("stopAttack");
            }

            _isTriggered = false;
            _target = null;
        }

        private IEnumerator AttackBehaviour()
        {
            while (true)
            {
                if (_target.IsDead)
                {
                    Cancel();
                    yield break;
                }

                transform.LookAt(_target.transform);
                _character.ProcessResetTrigger("stopAttack");
                _character.ProcessSetTrigger("attack");
                // This will trigger Hit() or Shoot() event in "attack" animation.
                yield return Helpers.BetterWaitForSeconds(_currentWeapon.TimeBetweenAttacks);
            }
        }

        // Method is called in melee attack animations as Event!
        private void Hit()
        {
            if (_target == null) return;

            _target.TakeDamage(_currentWeapon.Damage);
        }

        // Method is called in ranged attack animations as Event!
        private void Shoot()
        {
            if (_target == null) return;

            Projectile projectile = Instantiate(_currentWeapon.Projectile, weaponParent.position, Quaternion.identity);
            projectile.Init(_target, _currentWeapon.Damage);
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_currentWeapon.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            var weaponName = state.ToObject<string>();
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}