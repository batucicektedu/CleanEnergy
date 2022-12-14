using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController_DynamicJoystick : MonoBehaviour
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
        if(_inputController.InputDirection == Vector3.zero)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            return;
        }

        _rb.velocity = _inputController.InputDirection * movementSpeed;
        RotateToDirection(_inputController.InputDirection);
    }

    private void RotateToDirection(Vector3 dir)
    {
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        lookRotation.x = 0f;
        lookRotation.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
            Time.fixedDeltaTime * rotationSpeed);
    }
}
