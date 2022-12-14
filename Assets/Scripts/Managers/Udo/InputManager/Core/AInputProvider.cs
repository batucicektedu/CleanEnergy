using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AInputProvider : MonoBehaviour
{
    [SerializeField] protected bool considerFingerOverUI;

    public event Action<Vector3> OnFingerDown;
    public event Action<Vector3> OnFingerUpdate;
    public event Action<Vector3> OnFingerUp;

    protected void InvokeOnFingerDown(Vector3 inputPosition) { OnFingerDown?.Invoke(inputPosition); }
    protected void InvokeOnFingerUpdate(Vector3 inputPosition) { OnFingerUpdate?.Invoke(inputPosition); }
    protected void InvokeOnFingerUp(Vector3 inputPosition) { OnFingerUp?.Invoke(inputPosition); }
}
