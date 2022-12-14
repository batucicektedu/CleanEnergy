using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothFollowInputManager : AInputManager
{
    [SerializeField] private Image inputCurrentPositionIndicator;
    [SerializeField] private Image inputTrackerPositionIndicator;
    [SerializeField] private bool useDebugImages;
    [SerializeField] private float sensitivity = 20f;
    private Vector3 _currentInputPosition;

    protected override void Awake()
    {
        base.Awake();
        
        if(inputCurrentPositionIndicator != null) inputCurrentPositionIndicator.gameObject.SetActive(useDebugImages);
        if(inputTrackerPositionIndicator != null) inputTrackerPositionIndicator.gameObject.SetActive(useDebugImages);
    }

    protected override void InputProvider_OnFingerDown(Vector3 inputPosition)
    {
        if (!canGetInput) return;

        isGettingInput = true;
        _currentInputPosition = inputPosition;
        
        UpdateDebugImagePositions(inputPosition, _currentInputPosition);
        
        InvokeOnFingerDown();
    }

    protected override void InputProvider_OnFingerUpdate(Vector3 inputPosition)
    {
        if (!canGetInput || !isGettingInput) return;

        _currentInputPosition = Vector3.Lerp(_currentInputPosition, inputPosition, Time.deltaTime * sensitivity);
        inputDirection = inputPosition - _currentInputPosition;
        if (considerScreenResolution)
        {
            inputDirection.x /= Screen.width;
            inputDirection.y /= Screen.height;
        }
        inputSpeed = inputDirection.magnitude / Time.deltaTime;
        inputDirection.z = inputDirection.y;
        inputDirection.y = 0f;
        
        UpdateDebugImagePositions(inputPosition, _currentInputPosition);
        
        InvokeOnFingerUpdate();
    }

    protected override void InputProvider_OnFingerUp(Vector3 inputPosition)
    {
        isGettingInput = false;
        inputDirection = Vector3.zero;
        _currentInputPosition = inputPosition;
        
        UpdateDebugImagePositions(inputPosition, _currentInputPosition);
        
        InvokeOnFingerUp();
    }

    private void UpdateDebugImagePositions(Vector3 inputCurrentPos, Vector3 inputTrackPosition)
    {
        if (!useDebugImages || inputCurrentPositionIndicator == null || inputTrackerPositionIndicator == null) return;
        inputCurrentPositionIndicator.rectTransform.position = inputCurrentPos;
        inputTrackerPositionIndicator.rectTransform.position = inputTrackPosition;
    }
}
