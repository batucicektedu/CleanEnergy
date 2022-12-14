using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class MainCameraAngle : MonoBehaviour
{
    public static Vector3Int offset = new Vector3Int(3, 13, -15);
    public static Vector3Int angle = new Vector3Int(24, -10, 0);

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTransposer _cinemachineTransposer;

    public static MainCameraAngle instance;

    [Header("Top Stack")] 
    public int riseStartStackCount = 15;
    public float riseAmountForEachBall = 0.5f;
    public int maxCameraY = 19;
    private Tween riseTween;
    public int cameraYBeforeRising;
    
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

    public void ChangeCameraOffsetSlowly()
    {
        riseTween?.Kill();
        riseTween = DOTween.To(() => _cinemachineTransposer.m_FollowOffset, x => _cinemachineTransposer.m_FollowOffset = x,
            offset, 0.5f);
        //_cinemachineTransposer.m_FollowOffset = offset;
    }
    
    public void ChangeCameraAngle()
    {
        transform.eulerAngles = angle;
    }

    public void ResetCameraToBeforeRising()
    {
        if (cameraYBeforeRising == 0) return;
        offset = new Vector3Int(offset.x, cameraYBeforeRising, offset.z);
        ChangeCameraOffsetSlowly();
    }

    public void ChangeYForTopStack(int stackCount)
    {
        if(stackCount <= riseStartStackCount) return;

        if (cameraYBeforeRising == 0)
        {
            SetCameraYBeforeRising();
        }

        var stackCountThatRequireRising = stackCount - riseStartStackCount;
        var riseAmount = stackCountThatRequireRising * riseAmountForEachBall;
        var finalRisenAmount = (int)(cameraYBeforeRising + riseAmount);

        finalRisenAmount = Mathf.Min(finalRisenAmount, maxCameraY);
        
        offset = new Vector3Int(offset.x, finalRisenAmount, offset.z);
        ChangeCameraOffsetSlowly();
    }

    public void SetCameraYBeforeRising()
    {
        cameraYBeforeRising = offset.y;
    }
}
