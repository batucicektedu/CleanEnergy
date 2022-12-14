using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    private Transform _followTarget;

    private NavMeshAgent _navMeshAgent;

    private bool _followingTarget;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    public void StartFollowingTarget(Transform newFollowTarget)
    {
        _followTarget = newFollowTarget;

        _followingTarget = true;
    }

    public void StopFollowingTarget()
    {
        _followingTarget = false;

        _followTarget = null;
    }

    private void FollowTarget()
    {
        if (_followingTarget) return;
        
        _navMeshAgent.SetDestination(_followTarget.position);
    }
}
