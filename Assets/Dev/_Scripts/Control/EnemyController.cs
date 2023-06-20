using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyController : MonoBehaviour, ICharacter
    {
        public bool IsAttacking => _combatHandler.IsAttacking;
        
        private MovementHandler _movementHandler;
        private AnimationHandler _animationHandler;
        private CombatHandler _combatHandler;
        private HealthHandler _healthHandler;
        private CapsuleCollider _capsuleCollider;

        private void Awake()
        {
            _movementHandler = GetComponent<MovementHandler>();
            _animationHandler = GetComponent<AnimationHandler>();
            _combatHandler = GetComponent<CombatHandler>();
            _healthHandler = GetComponent<HealthHandler>();
            _capsuleCollider = GetComponent<CapsuleCollider>();

            _combatHandler.Init(this);
            _healthHandler.Init(this);
        }

        private void Update()
        {
            _animationHandler.UpdateLocomotion(_movementHandler.GetVelocity());
        }

        public void ProcessAttack(HealthHandler target)
        {
            _combatHandler.Attack(target);
            _movementHandler.MoveTo(target.transform.position);
        }

        public void ProcessPatrol(Vector3 target)
        {
            _movementHandler.MoveTo(target);
        }

        public void ProcessDie()
        {
            _animationHandler.SetTrigger("die");
            _capsuleCollider.enabled = false;
        }

        public void ProcessSetTrigger(string triggerName)
        {
            _animationHandler.SetTrigger(triggerName);
        }

        public void ProcessResetTrigger(string triggerName)
        {
            _animationHandler.ResetTrigger(triggerName);
        }
    }
}