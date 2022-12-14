using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassicInputProvider : AInputProvider
{
    private void Update()
    {
        bool fingerOverUI = false;
        if (considerFingerOverUI)
            fingerOverUI = IsPointerOverUI();

        if (Input.GetMouseButtonDown(0) && !fingerOverUI)
        {
            InvokeOnFingerDown(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0) && !fingerOverUI)
        {
            InvokeOnFingerUpdate(Input.mousePosition);
        }
        else if(Input.GetMouseButtonUp(0) && !fingerOverUI)
        {
            InvokeOnFingerUp(Input.mousePosition);
        }
    }

    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        return Input.GetTouch(0).phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId); // Doesn't work correctly on mobile without TouchPhase.Began
#endif
    }
}
