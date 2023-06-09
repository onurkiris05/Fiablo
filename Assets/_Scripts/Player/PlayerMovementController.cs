using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] LayerMask targetLayer;

    public event Action<Ray, Vector3> OnMove;

    private NavMeshAgent _navMeshAgent;
    private Finger movementFinger;
    private RaycastHit hit;
    private Ray ray;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;

        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null)
        {
            movementFinger = touchedFinger;

            MoveToPoint();
        }
    }

    private void HandleFingerUp(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
        }
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            MoveToPoint();
        }
    }

    private void MoveToPoint()
    {
        ray = Camera.main.ScreenPointToRay(movementFinger.currentTouch.screenPosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
        {
            var targetPos = hit.point;
            _navMeshAgent.SetDestination(targetPos);
            OnMove?.Invoke(ray, targetPos);
        }
    }
}