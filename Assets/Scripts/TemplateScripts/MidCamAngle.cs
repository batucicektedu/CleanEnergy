using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class MidCamAngle : MonoBehaviour
{
    public static Vector3Int offset;
    public static Vector3Int angle;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTransposer _cinemachineTransposer;

    public static MidCamAngle instance;
    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _cinemachineTransposer.m_FollowOffset = offset;
        transform.eulerAngles = angle;
        instance = this;
    }

    public void ChangeCameraOffset()
    {
        _cinemachineTransposer.m_FollowOffset = offset;
    }

    public void ChangeCameraAngle()
    {
        transform.eulerAngles = angle;
    }

    public void FPSEndGameState()
    {
        offset = new Vector3Int(6, 10, 0);
        ChangeCameraOffset();

        _cinemachineTransposer.GetComponent<CinemachineComposer>().m_TrackedObjectOffset 
            = new Vector3(0, 6.5f, 0);
    }

    public void NonFPSEndGameState()
    {
        transform.rotation = Quaternion.Euler(new Vector3(30, -54, 0));
    }
}
