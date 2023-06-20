using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public abstract class ControllerBase : MonoBehaviour, ICharacter
    {
        public bool IsDead() => _healthHandler.IsDead;

        protected MovementHandler _movementHandler { get; private set; }
        protected AnimationHandler _animationHandler { get; private set; }
        protected CombatHandler _combatHandler { get; private set; }
        protected HealthHandler _healthHandler { get; private set; }
        protected CapsuleCollider _capsuleCollider { get; private set; }

        protected virtual void Awake()
        {
            _movementHandler = GetComponent<MovementHandler>();
            _animationHandler = GetComponent<AnimationHandler>();
            _combatHandler = GetComponent<CombatHandler>();
            _healthHandler = GetComponent<HealthHandler>();
            _capsuleCollider = GetComponent<CapsuleCollider>();

            _combatHandler.Init(this);
            _healthHandler.Init(this);
        }

        protected virtual void Update()
        {
            _animationHandler.UpdateLocomotion(_movementHandler.GetVelocity());
        }

        public virtual void ProcessDie()
        {
            _combatHandler.Cancel();
            _movementHandler.Cancel();
            _animationHandler.SetTrigger("die");
            _capsuleCollider.enabled = false;
        }

        public virtual void ProcessSetTrigger(string triggerName)
        {
            _animationHandler.SetTrigger(triggerName);
        }

        public virtual void ProcessResetTrigger(string triggerName)
        {
            _animationHandler.ResetTrigger(triggerName);
        }
    }
}