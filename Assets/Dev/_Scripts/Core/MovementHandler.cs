using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class MovementHandler : MonoBehaviour, IAction, ISaveable
    {
        private NavMeshAgent _navMeshAgent;
        private ActionScheduler _actionScheduler;
        private Transform _target;

        public void Init(NavMeshAgent agent, ActionScheduler scheduler)
        {
            _navMeshAgent = agent;
            _actionScheduler = scheduler;
        }

        public Vector3 GetVelocity() => _navMeshAgent.velocity;

        public void MoveToDestination(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);
        }

        public void Cancel()
        {
            _target = null;
            if (_navMeshAgent.enabled)
                _navMeshAgent.isStopped = true;
        }

        public JToken CaptureAsJToken()
        {
            return transform.position.ToToken();
        }

        public void RestoreFromJToken(JToken state)
        {
            var position = state.ToVector3();
            _navMeshAgent.Warp(position);
            _actionScheduler.CancelCurrentAction();
        }
    }
}