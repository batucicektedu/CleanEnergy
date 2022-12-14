using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[RequireComponent(typeof(AInputProvider))]
public abstract class AInputManager : MonoBehaviour
{
    [Header("Input Manager Common Settings"), Space(10)]
    [SerializeField] protected bool canGetInput;
    [SerializeField] protected bool considerScreenResolution = true;

    private AInputProvider _inputProvider;
    protected bool isGettingInput;
    protected Vector3 inputDirection;
    protected float inputSpeed;

    public Vector3 InputDirection => inputDirection;
    public float InputSpeed => inputSpeed;

    #region Input Events
    public event Action OnFingerDown;
    public event Action OnFingerUpdate;
    public event Action OnFingerUp;
    #endregion

    protected virtual void Awake()
    {
        _inputProvider = GetComponent<AInputProvider>();
        if (_inputProvider == null) Debug.Log("Input Provider Is Missing!");
    }

    protected virtual void OnEnable()
    {
        if(_inputProvider != null)
        {
            _inputProvider.OnFingerDown += InputProvider_OnFingerDown;
            _inputProvider.OnFingerUpdate += InputProvider_OnFingerUpdate;
            _inputProvider.OnFingerUp += InputProvider_OnFingerUp;
        }
    }

    protected virtual void OnDisable()
    {
        if (_inputProvider != null)
        {
            _inputProvider.OnFingerDown -= InputProvider_OnFingerDown;
            _inputProvider.OnFingerUpdate -= InputProvider_OnFingerUpdate;
            _inputProvider.OnFingerUp -= InputProvider_OnFingerUp;
        }
    }

    protected abstract void InputProvider_OnFingerDown(Vector3 inputPosition);
    protected abstract void InputProvider_OnFingerUpdate(Vector3 inputPosition);
    protected abstract void InputProvider_OnFingerUp(Vector3 inputPosition);

    protected void InvokeOnFingerDown() { OnFingerDown?.Invoke(); }
    protected void InvokeOnFingerUpdate() { OnFingerUpdate?.Invoke(); }
    protected void InvokeOnFingerUp() { OnFingerUp?.Invoke(); }

    public virtual void SetCanGetInput(bool value)
    {
        canGetInput = value;
    }
    public virtual void ResetInputManager()
    {
        isGettingInput = false;
        inputDirection = Vector3.zero;
        inputSpeed = 0f;
    }
}
