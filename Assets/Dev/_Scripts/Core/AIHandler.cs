using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class AIHandler : MonoBehaviour
    {
        [SerializeField] private float chaseRadius = 5f;

        private EnemyController _enemyController;
        private PlayerController _player;
        private bool _isChasing;

        private Vector3 _firstPos;

        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _firstPos = transform.position;
        }

        private void Update()
        {
            ProcessDetection();
        }

        private void ProcessDetection()
        {
            if (InChaseRange() && !_player.IsDead)
            {
                Chase();
            }
            else if (!InChaseRange() || _player.IsDead)
            {
                Patrol();
            }
        }

        private void Patrol()
        {
            if (!_isChasing) return;
            _isChasing = false;
            _enemyController.ProcessPatrol(_firstPos);
            print($"Leaving chase to {_player.gameObject.name}");
        }

        private void Chase()
        {
            if (_player.TryGetComponent(out HealthHandler target) &&
                !_enemyController.IsAttacking)
            {
                _isChasing = true;
                _enemyController.ProcessAttack(target);
                print($"Chasing {_player.gameObject.name}");
            }
        }

        private bool InChaseRange()
        {
            var distance = (_player.transform.position - transform.position).sqrMagnitude;
            return distance <= chaseRadius.Sqr();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}