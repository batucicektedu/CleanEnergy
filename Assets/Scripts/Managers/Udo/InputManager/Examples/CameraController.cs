using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followSpeed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private List<CameraShakeTypeEntry> shakeTypes = new List<CameraShakeTypeEntry>();

    private Camera _camera;
    private bool _canFollow;
    private bool _isShaking;

    public Camera MainCamera => _camera;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _canFollow = true;
        _isShaking = false;
    }

    private void OnLevelStarted(Transform t)
    {
        TeleportCameraToFollowTarget();
    }

    private void LateUpdate()
    {
        if (!_canFollow || followTarget == null)
            return;

        Vector3 followPos = followTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, followPos, Time.deltaTime * followSpeed);
    }

    public void SetCanFollow(bool value)
    {
        _canFollow = value;
    }

    public void TeleportCameraToFollowTarget()
    {
        transform.position = followTarget.position + offset;
    }
}

[System.Serializable]
public class CameraShakeTypeEntry
{
    public ShakeType ShakeType;
    public float Duration;
    public float Strength;
    public int Vibrato;
}

public enum ShakeType { Die, Hit }
