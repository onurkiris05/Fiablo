using System.Collections;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponDamage = 15f;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        public float WeaponRange => weaponRange;

        private ControllerBase _character;
        private ActionScheduler _actionScheduler;
        private HealthHandler _target;
        private Coroutine _attackCoroutine;
        private bool _isTriggered;
        
        public void Init(ControllerBase character, ActionScheduler scheduler)
        {
            _character = character;
            _actionScheduler = scheduler;
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
                // This will trigger Hit() event in "attack" animation.
                yield return Helpers.BetterWaitForSeconds(timeBetweenAttacks);
            }
        }

        private void Hit()
        {
            if (_target == null) return;
            _target.TakeDamage(weaponDamage);
        }
    }
}