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
        private HealthHandler _healthHandler;
        
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
                _healthHandler = healthHandler;
                
                while (true)
                {
                    transform.LookAt(_target.transform);
                    _animationHandler.SetTrigger("attack");
                    // This will trigger Hit() event in "attack" animation.
                    yield return Helpers.BetterWaitForSeconds(timeBetweenAttacks);
                }
            }
        }

        private void Hit()
        {
            _healthHandler.TakeDamage(weaponDamage);
        }
    }
}