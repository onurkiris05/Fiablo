using Lean.Touch;
using UnityEngine;
using RPG.Core;
using RPG.Control;

public class PlayerController : ControllerBase
{
    [Header("Control Settings")]
    [SerializeField] private LayerMask movementLayer;
    [SerializeField] private LayerMask targetLayer;

    private InputHandler _inputHandler;
    private RaycastHit[] _hits;
    private Ray _ray;

    protected override void Awake()
    {
        base.Awake();

        _inputHandler = GetComponent<InputHandler>();
        _inputHandler.Init(this);
    }

    public void ProcessInputOnFingerDown(LeanFinger finger)
    {
        if (_healthHandler.IsDead) return;

        _hits = GetHits(finger.ScreenPosition);
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

    public void ProcessInputOnFingerMove(LeanFinger finger)
    {
        if (_healthHandler.IsDead) return;

        _hits = GetHits(finger.ScreenPosition);
        if (_hits.Length == 0) return;
        
        foreach (var hit in _hits)
        {
            // Process movement if ray hit to terrain
            Move(hit);
        }
    }

    private RaycastHit[] GetHits(Vector3 point)
    {
        _ray = Camera.main.ScreenPointToRay(point);
        return Physics.RaycastAll(_ray, Mathf.Infinity);
    }

    private bool CanAttack(RaycastHit hit)
    {
        if (hit.transform.gameObject.layer == targetLayer.LayerToInt() &&
            hit.transform.TryGetComponent(out HealthHandler target))
        {
            print("Attack launched");
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
            print("Move launched");
            _combatHandler.Cancel();
            _movementHandler.MoveTo(hit.point);
        }
    }
}