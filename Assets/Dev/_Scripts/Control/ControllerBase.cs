using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public abstract class ControllerBase : MonoBehaviour
    {
        protected MovementHandler _movementHandler { get; private set; }
        protected AnimationHandler _animationHandler { get; private set; }
        protected CombatHandler _combatHandler { get; private set; }
        protected HealthHandler _healthHandler { get; private set; }
        protected ActionScheduler _actionScheduler { get; private set; }
        protected NavMeshAgent _navMeshAgent { get; private set; }
        protected Animator _animator { get; private set; }
        
        public bool IsDead => _healthHandler.IsDead;
        public float GetAttackRange => _combatHandler.WeaponRange;

        protected virtual void Awake()
        {
            _movementHandler = GetComponent<MovementHandler>();
            _animationHandler = GetComponent<AnimationHandler>();
            _combatHandler = GetComponent<CombatHandler>();
            _healthHandler = GetComponent<HealthHandler>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();

            _healthHandler.Init(this);
            _combatHandler.Init(this, _actionScheduler);
            _movementHandler.Init(_navMeshAgent, _actionScheduler);
            _animationHandler.Init(_animator);
        }

        protected virtual void Update()
        {
            _animationHandler.UpdateLocomotion(_movementHandler.GetVelocity());
        }

        public virtual void ProcessDie(bool isImmediate = false)
        {
            if (isImmediate)
                _animationHandler.SetImmediate("Death");
            else
                _animationHandler.SetTrigger("die");
            
            _actionScheduler.CancelCurrentAction();
            _navMeshAgent.enabled = false;
        }

        public virtual void ProcessSetTrigger(string triggerName)
        {
            _animationHandler.SetTrigger(triggerName);
        }

        public virtual void ProcessResetTrigger(string triggerName)
        {
            _animationHandler.ResetTrigger(triggerName);
        }

        public virtual void CancelCurrentAction()
        {
            _actionScheduler.CancelCurrentAction();
        }
        
        public void ProcessMove(Vector3 target)
        {
            _movementHandler.MoveToDestination(target);
        }
        
        public void ProcessAttack(HealthHandler target)
        {
            _combatHandler.Attack(target);
        }
    }
}