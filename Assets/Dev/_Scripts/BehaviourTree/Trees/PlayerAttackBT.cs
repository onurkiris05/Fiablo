using System.Collections.Generic;
using UnityEngine;

namespace RPG.BehaviourTree
{
    public class PlayerAttackBT : Tree
    {
        [SerializeField] private LayerMask targetLayer;

        private PlayerController _player;

        public void Init(PlayerController player)
        {
            _player = player;
        }

        protected override Node SetupTree()
        {
            var checkIsDead = new CheckIsDead(_player);
            var checkIsSafe = new CheckIsSafe(_player);
            var checkIsTargetInRange = new CheckTargetInAttackRange(
                targetLayer, transform, _player);
            var taskAttack = new TaskAttack(_player);
            var taskGoToEnemy = new TaskGoToEnemy(_player, transform);

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