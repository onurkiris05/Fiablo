using UnityEngine;

public class PlayerDebugger : MonoBehaviour
{
    private PlayerMovementController _playerMovementController;
    private Vector3 _target;
    
    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
    }

    private void OnEnable()
    {
        _playerMovementController.OnMove += DebugMovePoint;
    }

    private void OnDisable()
    {
        _playerMovementController.OnMove -= DebugMovePoint;
    }

    private void DebugMovePoint(Ray ray, Vector3 target)
    {
        _target = target;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_target, 0.5f);
    }
}