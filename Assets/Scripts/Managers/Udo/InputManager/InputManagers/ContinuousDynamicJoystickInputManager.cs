using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuousDynamicJoystickInputManager : AInputManager
{
    [SerializeField] private Image joystickBG;
    [SerializeField] private Image knob;
    [SerializeField] private bool useDebugImages;
    [SerializeField] private float joystickMaxDistance;
    private Vector3 _joystickCenter;
    private Vector3 _joystickCurrentPosition;
    private float _distBetweenJoystickCenterAndCurrentPosition;

    protected override void Awake()
    {
        base.Awake();
        
        if(joystickBG != null) joystickBG.gameObject.SetActive(useDebugImages);
        if(knob != null) knob.gameObject.SetActive(useDebugImages);
    }

    private void Start()
    {
        if (considerScreenResolution)
            joystickMaxDistance *= (Screen.height / 1080f);
    }

    protected override void InputProvider_OnFingerDown(Vector3 inputPosition)
    {
        if (!canGetInput) return;

        isGettingInput = true;
        _joystickCenter = inputPosition;
        _joystickCurrentPosition = _joystickCenter;

        UpdateDebugImagePositions();
        
        InvokeOnFingerDown();
    }

    protected override void InputProvider_OnFingerUpdate(Vector3 inputPosition)
    {
        if (!canGetInput || !isGettingInput) return;

        _joystickCurrentPosition = inputPosition;
        _distBetweenJoystickCenterAndCurrentPosition = Vector3.Magnitude(_joystickCurrentPosition - _joystickCenter);
        if (_distBetweenJoystickCenterAndCurrentPosition >= joystickMaxDistance)
            _joystickCenter = _joystickCenter + (_joystickCurrentPosition - _joystickCenter).normalized * (_distBetweenJoystickCenterAndCurrentPosition - joystickMaxDistance);

        Vector3 direction = _joystickCurrentPosition - _joystickCenter;
        inputDirection.x = direction.x / joystickMaxDistance;
        inputDirection.z = direction.y / joystickMaxDistance;
        inputSpeed = direction.magnitude / Time.deltaTime;

        UpdateDebugImagePositions();
        
        InvokeOnFingerUpdate();
    }

    protected override void InputProvider_OnFingerUp(Vector3 inputPosition)
    {
        _distBetweenJoystickCenterAndCurrentPosition = 0f;
        inputDirection = Vector3.zero;
        isGettingInput = false;

        UpdateDebugImagePositions();
        
        InvokeOnFingerUp();
    }

    private void UpdateDebugImagePositions()
    {
        if (!useDebugImages || joystickBG == null || knob == null) return;
        joystickBG.rectTransform.position = _joystickCenter;
        knob.rectTransform.position = _joystickCurrentPosition;
    }
    
    public override void ResetInputManager()
    {
        base.ResetInputManager();
        _distBetweenJoystickCenterAndCurrentPosition = 0f;
    }
}
