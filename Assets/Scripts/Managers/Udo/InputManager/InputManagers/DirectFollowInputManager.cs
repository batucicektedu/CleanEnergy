using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectFollowInputManager : AInputManager
{
    [SerializeField] private float sensitivity = 10f;
    private Vector3 _currentInputPosition;
    private Vector3 _inputTrackPosition;

    protected override void InputProvider_OnFingerDown(Vector3 inputPosition)
    {
        if (!canGetInput) return;

        isGettingInput = true;
        _currentInputPosition = inputPosition;
        _inputTrackPosition = _currentInputPosition;

        InvokeOnFingerDown();
    }
    protected override void InputProvider_OnFingerUpdate(Vector3 inputPosition)
    {
        if (!canGetInput || !isGettingInput) return;

        inputDirection = inputPosition - _inputTrackPosition;
        if (considerScreenResolution)
        {
            inputDirection.x /= Screen.width;
            inputDirection.y /= Screen.height;
        }
        inputSpeed = inputDirection.magnitude / Time.deltaTime;
        Vector3 direction = inputDirection.normalized * inputSpeed * sensitivity;
        inputDirection.x = direction.x;
        inputDirection.y = 0f;
        inputDirection.z = direction.y;

        InvokeOnFingerUpdate();
        _inputTrackPosition = inputPosition;
    }

    protected override void InputProvider_OnFingerUp(Vector3 inputPosition)
    {
        inputDirection = Vector3.zero;
        isGettingInput = false;

        InvokeOnFingerUp();
    }
}
