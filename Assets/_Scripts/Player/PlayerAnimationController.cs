using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        var velocity = _navMeshAgent.velocity;
        var localVelocity = transform.InverseTransformDirection(velocity);
        var speed = localVelocity.z;
        _animator.SetFloat("forwardSpeed", speed);
    }
}