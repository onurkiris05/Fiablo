using UnityEngine;
using RPG.Core;
using RPG.Control;

public class PlayerController : ControllerBase
{
    [Header("Control Settings")]
    [SerializeField] private LayerMask movementLayer;
    [SerializeField] private LayerMask targetLayer;

    private RaycastHit[] _hits;
    private Ray _ray;

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