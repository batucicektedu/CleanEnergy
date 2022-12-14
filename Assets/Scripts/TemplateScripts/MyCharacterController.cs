using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using udoEventSystem;
public class MyCharacterController : MonoBehaviour
{
    public float dragSensitivity = 0.22f;
    public float lerpSpeed = 1;
    public float trackOffsetLimit = 2f;

    public bool rotation;
    public bool position;
    public float rotationSensitivity = 4.5f;
    public float rotationOffsetLimit = 1.5f;
    public float rotationResetDuration = 0.25f;
    public float rotationResetSmoothnessDelay = 0.5f;
    private float initialRotationResetSmoothnessDelay;

    private Vector3 _initialPos;
    private Vector3 _initialRot;
    private Tween _rotationTween;

    public bool invertControls;

    private bool initialRotationBool;
    private bool initialPositionBool;

    private void OnEnable()
    {
        InputManager.OnPointerMove += InputManager_OnPointerMove;
        EventManager.Get<StartUserInput>().AddListener(EnableInput);
        EventManager.Get<StopUserInput>().AddListener(DisableInput);
    }
    private void OnDisable()
    {
        InputManager.OnPointerMove -= InputManager_OnPointerMove;
        EventManager.Get<StartUserInput>().RemoveListener(EnableInput);
        EventManager.Get<StopUserInput>().RemoveListener(DisableInput);
    }

    private void EnableInput()
    {
        rotation = initialRotationBool;
        position = initialPositionBool;
    }

    private void DisableInput()
    {
        rotation = false;
        position = false;
    }

    void Awake()
    {
        _initialPos = transform.position;
        _initialRot = transform.rotation.eulerAngles;
        initialRotationResetSmoothnessDelay = rotationResetSmoothnessDelay;

        initialPositionBool = position;
        initialRotationBool = rotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && rotation)
        {
            _rotationTween.Kill();
            _rotationTween = transform.DOLocalRotate(Vector3.zero, rotationResetDuration);
        }
    }

    private void RotateTowardsMoveDirection(float offset)
    {
        offset = Mathf.Clamp(offset * rotationSensitivity,  -rotationOffsetLimit, rotationOffsetLimit);

        //Debug.Log("offset : " + offset);

        _rotationTween.Kill();
        _rotationTween = transform.DOLocalRotate(new Vector3(0, offset, 0), 0.5f);
    }
    
    private void FollowRoad(Vector2 offset)
    {
        if (offset == Vector2.zero && rotation)
        {
            rotationResetSmoothnessDelay -= Time.deltaTime;

            if(rotationResetSmoothnessDelay <= 0)
            {
                _rotationTween.Kill();
                _rotationTween = transform.DOLocalRotate(Vector3.zero, rotationResetDuration);
            }
        }
        else
        {
            rotationResetSmoothnessDelay = initialRotationResetSmoothnessDelay;

            Vector2 offsetDifference = offset * dragSensitivity * Time.deltaTime;
            offsetDifference.y = 0;

            Vector2 currentOffset = transform.localPosition;

            if (rotation)
            {
                RotateTowardsMoveDirection(offset.x);
            }
            // else
            // {
            //     transform.localRotation = Quaternion.identity;
            // }

            currentOffset += offsetDifference;

            if (currentOffset.x > trackOffsetLimit)
            {
                currentOffset.x = trackOffsetLimit;
            }
            else if (currentOffset.x < -trackOffsetLimit)
            {
                currentOffset.x = -trackOffsetLimit;
            }

            if (position)
            {
                //transform.localPosition = new Vector3(currentOffset.x, transform.localPosition.y, transform.localPosition.z);
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(currentOffset.x, transform.localPosition.y, transform.localPosition.z), lerpSpeed);
            }
        }
    }
    
    private void InputManager_OnPointerMove(Vector3 deltaPos, Vector3 pointerPos)
    {
        if (invertControls)
        {
            FollowRoad(-deltaPos);
        }
        else
        {
            FollowRoad(deltaPos);
        }
        
        
    }
    
    
    
}
