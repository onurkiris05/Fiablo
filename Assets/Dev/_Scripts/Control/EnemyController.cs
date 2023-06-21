using RPG.Core;
using RPG.Control;
using UnityEngine;

    public class EnemyController : ControllerBase
    {
        private AIHandler _aiHandler;

        protected override void Awake()
        {
            base.Awake();
            
            _aiHandler = GetComponent<AIHandler>();
            _aiHandler.Init(this);
        }

        public void ProcessAttack(HealthHandler target)
        {
            _combatHandler.Attack(target);
            _movementHandler.MoveToTarget(target.transform);
        }

        public void ProcessMove(Vector3 target)
        {
            _movementHandler.MoveToDestination(target);
        }
    }