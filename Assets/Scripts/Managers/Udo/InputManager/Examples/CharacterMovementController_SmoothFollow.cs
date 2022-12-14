using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController_SmoothFollow : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private AInputManager _inputController;
    private Rigidbody _rb;

    private void Awake()
    {
        _inputController = GetComponent<AInputManager>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 posToMove = _rb.position + transform.forward * movementSpeed * Time.fixedDeltaTime;
        posToMove.x += _inputController.InputDirection.x ;
        //transform.rotation = Quaternion.Euler(0f, _inputController.InputDirection.x * 200f, 0f);
        _rb.MovePosition(posToMove);
        Quaternion rot = Quaternion.Euler(0f, _inputController.InputDirection.x * rotationSpeed, 0f);
        _rb.MoveRotation(rot);
    }
}
