using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsHandler : MonoBehaviour
{
    private Animator _animator;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void ToIdle()
    {
        _animator.SetTrigger(Idle);
    }

    public void ToRun()
    {
        _animator.SetTrigger(Run);
    }
    
    public void ToJump()
    {
        _animator.SetTrigger(Jump);
    }

    public void SetAnimatorSpeed(float newSpeed)
    {
        _animator.speed = newSpeed;
    }
}
