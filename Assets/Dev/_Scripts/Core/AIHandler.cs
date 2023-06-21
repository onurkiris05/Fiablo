using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class AIHandler : MonoBehaviour
    {
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] private float chaseRadius = 5f;
        [SerializeField] float suspicionTime = 3f;

        private EnemyController _enemy;
        private PlayerController _player;
        private Vector3 _guardPos;
        private bool _isPatrolling = true;
        private bool _isChasing;
        private bool _isDwelling;
        private int _currentWaypointIndex;

        public void Init(EnemyController enemy)
        {
            _enemy = enemy;
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _guardPos = transform.position;
        }

        private void Update()
        {
            if (_enemy.IsDead()) return;

            ProcessChase();
            ProcessSuspicious();
            ProcessPatrol();
        }

        private void ProcessChase()
        {
            if (InChaseRange() && !_player.IsDead())
                Chase();
        }

        private void ProcessSuspicious()
        {
            if (!InChaseRange() && !_player.IsDead() && _isChasing)
                StartCoroutine(Suspicion());
        }

        private void ProcessPatrol()
        {
            if (_isPatrolling || _player.IsDead())
                if (!_isDwelling)
                    StartCoroutine(Patrol());
        }

        private void Chase()
        {
            if (!_enemy.IsAttacking &&
                _player.TryGetComponent(out HealthHandler target))
            {
                _isChasing = true;
                _isPatrolling = false;
                _enemy.ProcessAttack(target);
            }
        }
        
        private IEnumerator Suspicion()
        {
            _isChasing = false;
            _enemy.ProcessMove(_player.transform.position);
            yield return Helpers.BetterWaitForSeconds(suspicionTime);
            _isPatrolling = true;
        }

        private IEnumerator Patrol()
        {
            var waypoint = _guardPos;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    // Get next waypoint by adding +1
                    _isDwelling = true;
                    yield return Helpers.BetterWaitForSeconds(waypointDwellTime);
                    _currentWaypointIndex = patrolPath.GetIndex(_currentWaypointIndex + 1);
                    _isDwelling = false;
                }

                waypoint = GetCurrentWaypoint();
            }

            _enemy.ProcessMove(waypoint);
        }

        private bool AtWaypoint()
        {
            var distance = (GetCurrentWaypoint() - transform.position).sqrMagnitude;
            return distance <= waypointTolerance.Sqr();
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWaypointIndex);
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