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

            if (_hits.Length == 0) return;

            foreach (var hit in _hits)
            {
                if (hit.transform.TryGetComponent(out CombatTarget combatTarget))
                {
                    ProcessAttack(combatTarget);
                    ProcessMovement(hit.point, _combatHandler.WeaponRange);
                    GameManager.Instance.InvokeOnStateChange(GameState.Attacking);
                    break;
                }

                if (hit.transform.gameObject.layer == targetLayer.LayerToInt())
                {
                    ProcessMovement(hit.point, 0f);
                    GameManager.Instance.InvokeOnStateChange(GameState.Moving);
                }
            }
        }

        private void ProcessMovement(Vector3 pos, float stoppingDist)
        {
            _targetPos = pos;
            _movementHandler.MoveTo(_targetPos, stoppingDist);
        }

        private void ProcessStopMovement()
        {
            _movementHandler.Stop();
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