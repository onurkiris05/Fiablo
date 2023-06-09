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

        public bool IsAttacking => _isAttacking;

        private ControllerBase _character;
        private ActionScheduler _actionScheduler;
        private HealthHandler _target;
        private Coroutine _attackCoroutine;
        private bool _isTriggered;
        private bool _isAttacking;
        
        public void Init(ControllerBase character, ActionScheduler scheduler)
        {
            _character = character;
            _actionScheduler = scheduler;
        }

        private void Update()
        {
            if (!_isTriggered || _character.IsDead()) return;

            if (InAttackRange())
            {
                _isTriggered = false;
                _actionScheduler.StartAction(this);
                _attackCoroutine = StartCoroutine(AttackBehaviour());
            }
        }

        public void Attack(HealthHandler target)
        {
            if (_isTriggered) return;

            _target = target;
            _isTriggered = true;
        }

        public void Cancel()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _character.ProcessResetTrigger("attack");
                _character.ProcessSetTrigger("stopAttack");
            }

            _isAttacking = false;
            _isTriggered = false;
            _target = null;
        }

        private bool InAttackRange()
        {
            var distance = (_target.transform.position - transform.position).sqrMagnitude;
            _isAttacking = distance <= weaponRange.Sqr();
            return _isAttacking;
        }

        private IEnumerator AttackBehaviour()
        {
            while (true)
            {
                if (_target.IsDead || !InAttackRange())
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