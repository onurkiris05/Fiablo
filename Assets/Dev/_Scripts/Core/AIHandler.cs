using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class AIHandler : MonoBehaviour
    {
        [Header("Patrol Settings")]
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        [Space(20f)] [HorizontalLine]
        [Header("Chase Settings")]
        [SerializeField] private float chaseRadius = 5f;
        [SerializeField] float suspicionTime = 3f;

        private EnemyController _enemy;
        private PlayerController _player;
        private Vector3 _guardPos;
        private List<Coroutine> _coroutineList = new();
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
            ProcessPatrol();

            if (_player.IsDead()) return;
            ProcessChase();
            ProcessSuspicious();
        }

        private void ProcessChase()
        {
            if (InChaseRange())
                Chase();
        }

        private void ProcessSuspicious()
        {
            if (!InChaseRange() && _isChasing)
                ProcessCoroutine(Suspicious());
        }

        private void ProcessPatrol()
        {
            if (_isPatrolling || _player.IsDead())
                if (!_isDwelling)
                    Patrol();
        }

        private void ProcessCoroutine(IEnumerator routine)
        {
            if (_coroutineList.Count > 0)
                foreach (var coroutine in _coroutineList)
                    StopCoroutine(coroutine);

            _coroutineList.Clear();
            var newCoroutine = StartCoroutine(routine);
            _coroutineList.Add(newCoroutine);
        }

        private void Chase()
        {
            if (_enemy.IsAttacking) return;

            if (_player.TryGetComponent(out HealthHandler target))
            {
                _isChasing = true;
                _isPatrolling = false;
                _enemy.ProcessAttack(target);
            }
        }

        private void Patrol()
        {
            var waypoint = _guardPos;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    ProcessCoroutine(Dwell());
                    return;
                }

                waypoint = GetCurrentWaypoint();
            }

            _enemy.ProcessMove(waypoint);
        }

        private IEnumerator Suspicious()
        {
            _isChasing = false;
            _enemy.CancelCurrentAction();
            _enemy.ProcessMove(_player.transform.position);
            yield return Helpers.BetterWaitForSeconds(suspicionTime);
            _isPatrolling = true;
            _isDwelling = false;
        }

        private IEnumerator Dwell()
        {
            // Get next waypoint by adding +1
            _isDwelling = true;
            yield return Helpers.BetterWaitForSeconds(waypointDwellTime);
            _currentWaypointIndex = patrolPath.GetIndex(_currentWaypointIndex + 1);
            _isDwelling = false;
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