using System.Collections.Generic;
using UnityEngine;

namespace RPG.BehaviourTree
{
    public class PlayerAttackBT : Tree
    {
        [SerializeField] private LayerMask targetLayer;

        private PlayerController _controller;

        public void Init(PlayerController controller)
        {
            _controller = controller;
        }

        protected override Node SetupTree()
        {
            var checkIsDead = new CheckIsDead(_controller);
            var checkIsSafe = new CheckIsSafe(_controller);
            var checkIsTargetInRange = new CheckTargetInAttackRange(
                targetLayer, transform, _controller.GetAttackRange);
            var taskAttack = new TaskAttack(_controller);
            var taskGoToEnemy = new TaskGoToEnemy(_controller, transform);

            Sequence isTargetInRangeSequence = new Sequence(new List<Node>
            {
                checkIsTargetInRange,
                taskAttack
            });

            Node root = new Selector(new List<Node>
            {
                checkIsDead,
                checkIsSafe,
                isTargetInRangeSequence,
                taskGoToEnemy
            });

            return root;
        }
    }
}