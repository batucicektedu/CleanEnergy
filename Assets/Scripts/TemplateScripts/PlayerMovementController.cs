using System;
using System.Collections;
using System.Collections.Generic;
using udoEventSystem;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    //[SerializeField] private float pathWidth = 6f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 0f;
    private AInputManager _inputManager;
    private Rigidbody _rigidbody;

    public float pathMinX = -3;
    public float pathMaxX = 3;

    [HideInInspector] public float initialPathMinX;
    [HideInInspector] public float initialPathMaxX;

    private float _initialMovementSpeed;

    public bool invertMovement;

    private void Start()
    {
        _initialMovementSpeed = movementSpeed;
    }

    private void Awake()
    {
        _inputManager = GetComponent<AInputManager>();
        _rigidbody = GetComponent<Rigidbody>();

        initialPathMinX = pathMinX;
        initialPathMaxX = pathMaxX;
    }

    private void OnEnable()
    {
        EventManager.Get<StopUserInput>().AddListener(StopUserInput);
        EventManager.Get<StartUserInput>().AddListener(StartUserInput);
    }
    
    private void OnDisable()
    {
        EventManager.Get<StopUserInput>().RemoveListener(StopUserInput);
        EventManager.Get<StartUserInput>().RemoveListener(StartUserInput);
    }

    private void Update()
    {
        Vector3 movePos = transform.localPosition;
        
        if (invertMovement)
        {
            movePos.x -= _inputManager.InputDirection.x;
        }
        else
        {
            movePos.x += _inputManager.InputDirection.x;
        }
        
        
        //movePos.z += _inputManager.InputDirection.z;
        movePos.x = Mathf.Clamp(movePos.x, pathMinX, pathMaxX);
        //movePos.z = Mathf.Clamp(movePos.z, pathMinX, pathMaxX);
        transform.localPosition = Vector3.Lerp(transform.localPosition, movePos, Time.deltaTime * movementSpeed);
        
        Quaternion lookRot = Quaternion.Euler(0f, _inputManager.InputDirection.x * rotationSpeed, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, Time.deltaTime * rotationSpeed);
    }

    private void StopUserInput()
    {
        movementSpeed = 0;
    }

    private void StartUserInput()
    {
        movementSpeed = _initialMovementSpeed;
    }
}
