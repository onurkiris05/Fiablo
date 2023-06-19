using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Control Settings")]
        [SerializeField] private LayerMask targetLayer;

        private MovementHandler _movementHandler;
        private AnimationHandler _animationHandler;
        private CombatHandler _combatHandler;
        private RaycastHit[] _hits;
        private Ray _ray;

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
                    _combatHandler.ProcessAttack(combatTarget);
                    _movementHandler.MoveTo(hit.point);
                    break;
                }

                if (hit.transform.gameObject.layer == targetLayer.LayerToInt())
                {
                    _combatHandler.Cancel();
                    _movementHandler.MoveTo(hit.point);
                }
            }
        }
    }
}