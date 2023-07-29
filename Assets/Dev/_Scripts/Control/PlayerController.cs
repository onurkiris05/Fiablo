using Lean.Touch;
using RPG.BehaviourTree;
using RPG.Combat;
using UnityEngine;
using RPG.Core;
using RPG.Control;

public class PlayerController : ControllerBase
{
    [Header("Control Settings")]
    [SerializeField] private LayerMask movementLayer;
    [SerializeField] private LayerMask targetLayer;

    public Transform Target => _target;

    private InputHandler _inputHandler;
    private PickupHandler _pickupHandler;
    private PlayerAttackBT _playerAttackBt;
    private Transform _target;
    private RaycastHit[] _hits;
    private Ray _ray;

    protected override void Awake()
    {
        base.Awake();

        _pickupHandler = GetComponent<PickupHandler>();
        _inputHandler = GetComponent<InputHandler>();
        _playerAttackBt = GetComponent<PlayerAttackBT>();
        _pickupHandler.Init(this);
        _inputHandler.Init(this);
        _playerAttackBt.Init(this);
    }

    public void EquipWeapon(Weapon weapon)
    {
        _combatHandler.EquipWeapon(weapon);
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
            CancelCurrentAction();
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
            _target = target.transform;
            return true;
        }

        _target = null;
        return false;
    }

    private void Move(RaycastHit hit)
    {
        if (hit.transform.gameObject.layer == movementLayer.LayerToInt())
        {
            _target = null;
            ProcessMove(hit.point);
        }
    }
}