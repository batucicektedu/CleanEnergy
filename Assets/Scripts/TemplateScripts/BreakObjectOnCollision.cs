using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObjectOnCollision : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;

    public Renderer _rendererToDisable;

    public GameObject parentOfFractures;

    public float otherObjectSpeedNeededToBreak = 25;

    private void Awake()
    {
        _rigidbodies = parentOfFractures.GetComponentsInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("speed " + other.GetComponent<Rigidbody>().velocity.magnitude);
            
            if (other.GetComponent<Rigidbody>().velocity.magnitude > otherObjectSpeedNeededToBreak)
            {
                Break();
            }
        }
    }

    private void Break()
    {
        _rendererToDisable.enabled = false;
        
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].isKinematic = false;
            
            // _rigidbodies[i].AddForce(Vector3.forward * TheBoss.Instance.hipRigidbody.velocity.magnitude / 2 
            //                          + Vector3.left * TheBoss.Instance.hipRigidbody.velocity.magnitude / 2, 
            //     ForceMode.Force);
        }
    }
}
