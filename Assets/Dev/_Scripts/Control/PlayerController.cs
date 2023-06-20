using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour, ICharacter
    {
        [Header("Control Settings")]
        [SerializeField] private LayerMask movementLayer;
        [SerializeField] private LayerMask targetLayer;

        public bool IsDead() => _healthHandler.IsDead;

        private MovementHandler _movementHandler;
        private AnimationHandler _animationHandler;
        private CombatHandler _combatHandler;
        private HealthHandler _healthHandler;
        private CapsuleCollider _capsuleCollider;
        private RaycastHit[] _hits;
        private Ray _ray;

        private void Awake()
        {
            _movementHandler = GetComponent<MovementHandler>();
            _animationHandler = GetComponent<AnimationHandler>();
            _combatHandler = GetComponent<CombatHandler>();
            _healthHandler = GetComponent<HealthHandler>();
            _capsuleCollider = GetComponent<CapsuleCollider>();

            _combatHandler.Init(this);
            _healthHandler.Init(this);
        }

        private void Update()
        {
            _animationHandler.UpdateLocomotion(_movementHandler.GetVelocity());
        }

        public void ProcessInput(Vector2 pos)
        {
            if (_healthHandler.IsDead) return;

            _ray = Camera.main.ScreenPointToRay(pos);
            _hits = Physics.RaycastAll(_ray, Mathf.Infinity);

            if (_hits.Length == 0) return;

            foreach (var hit in _hits)
            {
                // Process attack if ray hit to a target
                if (CanAttack(hit))
                    break;

                // Process movement if ray hit to terrain
                Move(hit);
            }
        }

        public void ProcessDie()
        {
            _combatHandler.Cancel();
            _movementHandler.Cancel();
            _capsuleCollider.enabled = false;
            _animationHandler.SetTrigger("die");
        }

        public void ProcessSetTrigger(string triggerName)
        {
            _animationHandler.SetTrigger(triggerName);
        }

        public void ProcessResetTrigger(string triggerName)
        {
            _animationHandler.ResetTrigger(triggerName);
        }

        private bool CanAttack(RaycastHit hit)
        {
            if (hit.transform.gameObject.layer == targetLayer.LayerToInt() &&
                hit.transform.TryGetComponent(out HealthHandler target))
            {
                _combatHandler.Attack(target);
                _movementHandler.MoveTo(hit.point);
                return true;
            }

            return false;
        }

        private void Move(RaycastHit hit)
        {
            if (hit.transform.gameObject.layer == movementLayer.LayerToInt())
            {
                _combatHandler.Cancel();
                _movementHandler.MoveTo(hit.point);
            }
        }
    }
}