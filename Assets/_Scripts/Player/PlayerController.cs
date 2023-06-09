using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Control Settings")]
        [SerializeField] LayerMask targetLayer;

        private MovementHandler _movementHandler;
        private AnimationHandler _animationHandler;
        private CombatHandler _combatHandler;
        private RaycastHit[] _hits;
        private Ray _ray;
        private Vector3 _targetPos;

        private void Awake()
        {
            _movementHandler = GetComponent<MovementHandler>();
            _animationHandler = GetComponent<AnimationHandler>();
            _combatHandler = GetComponent<CombatHandler>();
        }

        private void Update()
        {
            _animationHandler.UpdateAnimation(_movementHandler.GetVelocity());
        }

        public void ProcessInput(Vector2 pos)
        {
            _ray = Camera.main.ScreenPointToRay(pos);
            _hits = Physics.RaycastAll(_ray, Mathf.Infinity);

            foreach (var hit in _hits)
            {
                if (hit.transform.gameObject.layer == targetLayer.LayerToInt())
                    ProcessMovement(hit.point);

                if (hit.transform.TryGetComponent(out CombatTarget combatTarget))
                    ProcessAttack(combatTarget);
            }
        }

        private void ProcessMovement(Vector3 pos)
        {
            _targetPos = pos;
            _movementHandler.MoveTo(_targetPos);
        }

        private void ProcessAttack(CombatTarget combatTarget)
        {
            _combatHandler.Attack(combatTarget);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_targetPos, 0.5f);
        }
    }
}