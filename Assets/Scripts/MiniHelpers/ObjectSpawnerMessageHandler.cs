using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerMessageHandler : MonoBehaviour
{
    private ObjectSpawner _objectSpawner;

    private void OnEnable()
    {
        _objectSpawner = GetComponentInParent<ObjectSpawner>();
    }

    public void ObjectRemovedFromSpawner()
    {
        _objectSpawner.DetachObject();
    }

}