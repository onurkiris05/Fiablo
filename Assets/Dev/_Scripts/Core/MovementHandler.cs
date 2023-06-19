using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class MovementHandler : MonoBehaviour, IAction
    {
        private NavMeshAgent _navMeshAgent;
        private ActionScheduler _actionScheduler;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        public Vector3 GetVelocity() => _navMeshAgent.velocity;

        public void MoveTo(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}