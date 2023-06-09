using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class MovementHandler : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public Vector3 GetVelocity() => _navMeshAgent.velocity;

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }
    }
}