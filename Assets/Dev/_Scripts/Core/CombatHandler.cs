using System.Collections;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponDamage = 15f;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        private ActionScheduler _actionScheduler;
        private AnimationHandler _animationHandler;
        private CombatTarget _target;
        
        private Coroutine _attackCoroutine;
        private bool _isTriggered;

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _animationHandler = GetComponent<AnimationHandler>();
        }

        private void Update()
        {
            if (!_isTriggered) return;

            var distance = (_target.transform.position - transform.position).sqrMagnitude;
            if (distance <= weaponRange.Sqr())
            {
                _isTriggered = false;
                _actionScheduler.StartAction(this);
                _attackCoroutine = StartCoroutine(AttackBehaviour());
                print("Actually attacking");
            }
        }

        public void ProcessAttack(CombatTarget target)
        {
            if (_isTriggered) return;

            print("Going to Attack");
            _target = target;
            _isTriggered = true;
        }

        public void Cancel()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
            _isTriggered = false;
            _target = null;
        }

        private IEnumerator AttackBehaviour()
        {
            if (_target.TryGetComponent(out HealthHandler healthHandler))
            {
                while (true)
                {
                    _animationHandler.SetTrigger("attack");
                    healthHandler.TakeDamage(weaponDamage);
                    yield return Helpers.BetterWaitForSeconds(timeBetweenAttacks);
                }
            }
        } 
    }
}