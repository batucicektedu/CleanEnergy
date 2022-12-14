using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { private set; get; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static event Action<Vector3, Vector3> OnPointerMove;
    public static event Action<Vector3> OnSwipe;
    public static event Action<Vector3> OnClickStart;
    public static event Action<Vector3> OnClickEnd;
    public static event Action<Vector2> OnJoystickMove;
    public Joystick joystick;
    public float swipeTreshold = 1f;
 
    Vector3 pointerPosition = Vector3.zero;
    Vector3 startPos = Vector3.zero; 
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 ? (Input.touches[0].phase == TouchPhase.Began) : false))
        {
            if (Input.touchCount == 0)
            {
                pointerPosition = Input.mousePosition;
            }
            else
            {
                pointerPosition = Input.touches[0].position;
            }
            startPos = pointerPosition;
             
            //if (EventSystem.current != null)
            //{
            //    if (EventSystem.current.IsPointerOverGameObject())
            //    {

            //    }

            //    else
            //    {
            //        OnClickStart?.Invoke(startPos);
            //    }
            //}
            //else {
            //    OnClickStart?.Invoke(startPos);
            //}

            OnClickStart?.Invoke(startPos);
       
        }

        if (Input.GetMouseButton(0) || (Input.touchCount > 0 ? Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary : false))
        {

            Vector3 deltaPos = Vector3.zero;

            if (Input.touchCount == 0)
            {
                deltaPos = Input.mousePosition - pointerPosition;
                pointerPosition = Input.mousePosition;
            }
            else
            {
                deltaPos = Input.touches[0].deltaPosition;
                pointerPosition = Input.touches[0].position;
            }
   
            OnPointerMove?.Invoke(deltaPos, pointerPosition);
            if (joystick != null)
            {
                OnJoystickMove?.Invoke(joystick.Direction);
            }
        }
         
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 ? (Input.touches[0].phase == TouchPhase.Ended) : false))
        {
            if (Input.touchCount == 0)
            {
                pointerPosition = Input.mousePosition;
            }
            else
            {
                pointerPosition = Input.touches[0].position;
            }


            OnClickEnd?.Invoke(pointerPosition);
            // OnStearing?.Invoke(0,0);
             
            if (Vector3.Distance(pointerPosition, startPos) > swipeTreshold)
            {
                Vector3 deltaPos = Vector3.zero;
                deltaPos = pointerPosition - startPos; 
                OnSwipe?.Invoke(deltaPos); 
            }

            pointerPosition = Vector3.zero;
        }

    }

}
