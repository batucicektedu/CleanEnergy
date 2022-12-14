using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InstantFollowPos : MonoBehaviour
{
    public bool autoSetOffset;
    
    public Transform target;
    public bool follow;

    public Vector3 offset;

    public Vector3 levelEndOffset;
    public Vector3 levelEndRotation;
    public float rotationDuration;

    public bool followWithSmoothDamp;
    
    [HideInInspector]public bool startLerpingOffset;

    public float smoothTime;

    private float offsetYTarget;

    private void Start()
    {
        if (autoSetOffset)
        {
            offset = transform.position - target.position;
        }
    }

    void FixedUpdate()
    {
        if (startLerpingOffset && offset.y >= offsetYTarget)
        {
            offset = new Vector3(offset.x, Mathf.Lerp(offset.y, offsetYTarget, Time.deltaTime * 10), offset.z);
        }

        if (follow)
        {
            if (followWithSmoothDamp)
            {
                Vector3 vel = Vector3.zero;
                
                Vector3 desiredPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref vel, smoothTime);
            }
            else
            {
                transform.position = target.position + offset;
            }
        }
    }

    public void ChangeOffsetToSlideCharactersDownward()
    {
        //offsetYTarget = offset.y - 3;
        offsetYTarget = 0;

        startLerpingOffset = true;
    }

    public void LevelEndTransition()
    {
        followWithSmoothDamp = true;
        
        offset = levelEndOffset;
        
        transform.DOLocalRotate(levelEndRotation, rotationDuration);
    }
}
