using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using udoEventSystem;

public class PlayerMovementControllerOmniDirectional : MonoBehaviour
{
    //public Transform playerFollower;
    
    public LayerMask groundCheckLayerMask;
    
    public float moveSpeed;
    
    public bool takingInput;
    
    private CharacterController _characterController;
    
    private Vector3 _lastTakenInputVector;
    private bool _inputTakenThisFrame;
    private bool _isMoving;
    private Vector3 direction;
    private float heading;
    private AnimationsHandler _animationsHandler;

    private bool _movementSpedUp;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animationsHandler = GetComponentInChildren<AnimationsHandler>();
    }

    private void OnEnable()
    {
        InputManager.OnJoystickMove += InputManager_OnJoystickMove;
        EventManager.Get<StartUserInput>().AddListener(EnableInput);
        EventManager.Get<StopUserInput>().AddListener(DisableInput);
    }
    
    private void FixedUpdate()
    {
        //KeepDistanceToGround();
    }

    private void ToggleSpeed()
    {
        if (_movementSpedUp)
        {
            moveSpeed /= 5;
        }
        else
        {
            moveSpeed *= 5;
        }
        
        _movementSpedUp = !_movementSpedUp;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleSpeed();
        }
        
        if (!_inputTakenThisFrame && _isMoving)
        {
            _lastTakenInputVector = Vector3.zero;
            
            CheckForAnimationChange();
        
            _isMoving = false;
        }
        
        _inputTakenThisFrame = false;
        
        KeepDistanceToGround();
    }

    private void OnDisable()
    {
        InputManager.OnJoystickMove -= InputManager_OnJoystickMove;
        EventManager.Get<StartUserInput>().RemoveListener(EnableInput);
        EventManager.Get<StopUserInput>().RemoveListener(DisableInput);
    }

    private void DisableInput()
    {
        takingInput = false;
        _isMoving = false;
        
        _animationsHandler.ToIdle();
    }
    
    private void EnableInput()
    {
        takingInput = true;
        _isMoving = true;
    }
    
    private void Move()
    {
        if (_lastTakenInputVector == Vector3.zero) return;
        
        Vector3 positionDifference = Time.deltaTime * _lastTakenInputVector;
        
        positionDifference = positionDifference.normalized;
        
        //If we want to turn the characters input direction
        // positionDifference = RotatePointAroundPivot(positionDifference, Vector3.zero, 
        //     playerFollower.rotation.eulerAngles);
        
        transform.forward = positionDifference;
        
        positionDifference *= moveSpeed;
        
        _characterController.Move(positionDifference);
        
    }
    
    private void InputManager_OnJoystickMove(Vector2 deltaPos)
    {
        if (!takingInput) return;
        
        var deltaPosV3 = InputVectorToMovementDirection(deltaPos);
        
        _lastTakenInputVector = deltaPosV3;
        
        _inputTakenThisFrame = true;
        
        CheckForAnimationChange();
        
        Move();
    }
    
    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    private void CheckForAnimationChange()
    {
        if (_lastTakenInputVector == Vector3.zero && _isMoving)
        {
            _isMoving = false;
            _animationsHandler.ToIdle();
        }

        if (_lastTakenInputVector != Vector3.zero && !_isMoving)
        {
            _isMoving = true;
            _animationsHandler.ToRun();
        }
    }

    private void KeepDistanceToGround()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, 
                out RaycastHit raycastHit, Mathf.Infinity, groundCheckLayerMask, 
                QueryTriggerInteraction.Ignore))
        {
            var position = transform.position;
            
            transform.position = new Vector3(position.x, raycastHit.point.y, position.z);
        }
        
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down * 5, Color.blue);
    }

    private Vector3 InputVectorToMovementDirection(Vector2 input)
    {
        return new Vector3(input.x, 0, input.y);
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}
