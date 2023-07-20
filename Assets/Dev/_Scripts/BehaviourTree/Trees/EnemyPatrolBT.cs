using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.BehaviourTree
{
    public class EnemyPatrolBT : Tree
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float fovRange;
        [SerializeField] private float suspiciousTime;
        [SerializeField] private PatrolPath path;
        [SerializeField] private float dwellTime;

        private EnemyController _controller;

        public void Init(EnemyController controller)
        {
            _controller = controller;
        }

        protected override Node SetupTree()
        {
            var checkIsDead = new CheckIsDead(_controller);
            var taskAttack = new TaskAttack(_controller);
            var checkEnemyInAttackRange = new CheckTargetInAttackRange(
                targetLayer, transform, _controller.GetAttackRange);
            var checkEnemyInFovRange = new CheckTargetInFOVRange(targetLayer, transform, fovRange);
            var taskToGoTarget = new TaskGoToPlayer(_controller, transform, suspiciousTime);
            var taskPatrol = new TaskPatrol(_controller, path, dwellTime);

            Sequence checkTargetInAttackRange = new Sequence(new List<Node>
            {
                checkEnemyInAttackRange,
                taskAttack
            });

            Sequence checkTargetInFovRange = new Sequence(new List<Node>
            {
                checkEnemyInFovRange,
                taskToGoTarget
            });

            Node root = new Selector(new List<Node>
            {
                checkIsDead,
                checkTargetInAttackRange,
                checkTargetInFovRange,
                taskPatrol
            });

            return root;
        }
    }
}