using System;
using System.Collections;
using System.Collections.Generic;
//using Dreamteck.Splines;
using udoEventSystem;
using UnityEngine;

public class SplineController : MonoBehaviour
{
    // public float speedModifierOnShiftPress = 4f;
    //
    // private SplineFollower _splineFollower;
    //
    // [HideInInspector] public float _initialSplineFollowSpeed;
    //
    //
    // private void OnEnable()
    // {
    //     _splineFollower = GetComponent<SplineFollower>();
    //     
    //     //_initialSplineFollowSpeed = Player.Instance.GetComponentInParent<SplineFollower>().followSpeed;
    //     
    //     EventManager.Get<StartForwardMovement>().AddListener(StartFollow);
    //     EventManager.Get<StopForwardMovement>().AddListener(StopFollow);
    // }
    //
    // private void OnDisable()
    // {
    //     EventManager.Get<StartForwardMovement>().RemoveListener(StartFollow);
    //     EventManager.Get<StopForwardMovement>().RemoveListener(StopFollow);
    // }
    //
    // private void StopFollow()
    // {
    //     //This line is because the components starts disabled
    //     _splineFollower = GetComponent<SplineFollower>();
    //     
    //     _splineFollower.follow = false;
    // }
    //
    // private void StartFollow()
    // {
    //     //This line is because the components starts disabled
    //     _splineFollower = GetComponent<SplineFollower>();
    //     
    //     _splineFollower.follow = true;
    // }
    //
    // private void Update()
    // {
    //     if (Input.GetKey(KeyCode.LeftShift))
    //     {
    //         _splineFollower.followSpeed = _initialSplineFollowSpeed * speedModifierOnShiftPress;
    //     }
    //     else
    //     {
    //         _splineFollower.followSpeed = _initialSplineFollowSpeed;
    //     }
    // }
}
