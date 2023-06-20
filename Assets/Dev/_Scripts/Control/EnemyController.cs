using RPG.Core;
using RPG.Control;
using UnityEngine;

    public class EnemyController : ControllerBase
    {
        public bool IsAttacking => _combatHandler.IsAttacking;

        public void ProcessAttack(HealthHandler target)
        {
            _combatHandler.Attack(target);
            _movementHandler.MoveTo(target.transform.position);
        }

        public void ProcessPatrol(Vector3 target)
        {
            _movementHandler.MoveTo(target);
        }
    }