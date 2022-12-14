using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ToggleCameraAngle : MonoBehaviour
{
    public Vector3 testOffset;
    public Vector3 testRotation;
    
    private Vector3 initialFollowOffset;
    private Vector3 initialRotation;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTransposer _cinemachineTransposer;
    
    private bool toggle;
    
    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        
        initialFollowOffset = _cinemachineTransposer.m_FollowOffset;
        initialRotation = transform.rotation.eulerAngles;
    }

    public void ChangeCameraAngle()
    {
        if (toggle)
        {
            _cinemachineTransposer.m_FollowOffset = initialFollowOffset;
            transform.rotation = Quaternion.Euler(initialRotation);

        }
        else
        {
            _cinemachineTransposer.m_FollowOffset = testOffset;
            transform.rotation = Quaternion.Euler(testRotation);
        }

        toggle = !toggle;
    }
}
