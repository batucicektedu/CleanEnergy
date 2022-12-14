using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { private set; get; }

    private Follower _follower;
    private bool _hasFollower;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate of " + name, gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Follower"))
        {
            if (_hasFollower) return;
            
            _follower = other.GetComponent<Follower>();
            _follower.StartFollowingTarget(transform);
            
            _hasFollower = true;
        }
    }

    private void TakeFollower(Follower followre)
    {
        
    }

    private void ReleaseFollower()
    {
        _follower.StopFollowingTarget();

        _hasFollower = false;
        _follower = null;
    }
}
