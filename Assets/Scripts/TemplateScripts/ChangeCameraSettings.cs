using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using udoEventSystem;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ChangeCameraSettings : MonoBehaviour
{
    public Vector3 newOffset;
    //public Vector3 newRotation;
    
    private Vector3 initialFollowOffset;
    //private Vector3 initialRotation;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTransposer _cinemachineTransposer;
    
    private bool toggle;

    private void OnEnable()
    {
        EventManager.Get<LevelCompleted>().AddListener(ChangeSettings);
    }

    private void OnDisable()
    {
        EventManager.Get<LevelCompleted>().RemoveListener(ChangeSettings);
    }

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        
        initialFollowOffset = _cinemachineTransposer.m_FollowOffset;
        //initialRotation = transform.rotation.eulerAngles;
    }

    public void ChangeCameraAngle()
    {
        if (toggle)
        {
            _cinemachineTransposer.m_FollowOffset = initialFollowOffset;
            //transform.rotation = Quaternion.Euler(initialRotation);

        }
        else
        {
            _cinemachineTransposer.m_FollowOffset = newOffset;
            //transform.rotation = Quaternion.Euler(newRotation);
        }

        toggle = !toggle;
    }

    public void ChangeSettings()
    {
        Debug.Log("Dotween Started");
        DOTween.To(()=>_cinemachineTransposer.m_FollowOffset, x=> _cinemachineTransposer.m_FollowOffset = x, 
            _cinemachineTransposer.m_FollowOffset = newOffset, 1).SetEase(Ease.Linear);
    }
}
