using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCamera : MonoBehaviour
{
    public Transform player;

    private Transform mainCam;
    private Vector3 startOffset;

    private Vector3 mainCamRotation;

    private void Awake()
    {
        if (player != null)
            startOffset = transform.position - player.position;
    }

    private void Start()
    {
        if (Camera.main != null)
        {
            mainCam = Camera.main.transform;
            mainCamRotation = mainCam.rotation.eulerAngles;
        }
    }

    private void OnEnable()
    {
        CameraAlignment();
        SetPositionWithOffsetWithPlayer();
    }

    private void LateUpdate()
    {
        SetPositionWithOffsetWithPlayer();
    }

    private void CameraAlignment()
    {
        transform.rotation = 
            Quaternion.Euler(new Vector3(mainCamRotation.x, transform.rotation.y, transform.rotation.z));
    }

    private void SetPositionWithOffsetWithPlayer()
    {
        if (player != null)
            transform.position = player.position + startOffset;
    }
}
