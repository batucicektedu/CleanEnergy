using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightableObject_ChildLighter : LightableObject
{
    private List<LightableObject> _childLightableObjects = new List<LightableObject>();

    protected override void Awake()
    {
        _childLightableObjects = GetComponentsInChildren<LightableObject>().ToList();
        _childLightableObjects.RemoveAt(0);
    }

    public override void LightUp()
    {
        foreach (var clo in _childLightableObjects)
        {
            clo.LightUp(out halfLit);
        }
        
        if (twinBuilding != null)
        {
            twinBuilding.LightUp(out halfLit);
        }
    }
}
