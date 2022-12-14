using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CablePiece : MonoBehaviour
{
    public bool curved;
    
    private GameObject _activeCable;
    
    private bool _cableThrowStarted;

    public bool activated;

    private CablePieceGroup _cablePieceGroup;

    private Collider _collider;

    private void Awake()
    {
        _activeCable = transform.GetChild(1).gameObject;
        _cablePieceGroup = GetComponentInParent<CablePieceGroup>();
        _collider = GetComponent<Collider>();
    }

    public void SetCableThrowStartedTrue()
    {
        _cableThrowStarted = true;
        _cablePieceGroup.MoveIndicatorToNextCablePosition();
    }

    public bool IsCableThrowStarted()
    {
        return _cableThrowStarted;
    }

    public void CableThrowAction(Collectable stackableCable)
    {
        stackableCable.ToCablePieceJump(transform, this);
    }

    public void ActivateCable()
    {
        if (activated) return;
        
        if (_activeCable != null)
        {
            if (curved)
            {
                PoolManager.Instance.Despawn(Pools.Types.CableTransparentCurved, _activeCable);
            }
            else
            {
                PoolManager.Instance.Despawn(Pools.Types.CableTransparent, _activeCable);
            }
        }
        
        tag = "CableActivated";

        activated = true;

        if (curved)
        {
            _activeCable = PoolManager.Instance.Spawn(Pools.Types.CableActivatedCurved, transform.position,
                transform.rotation, transform);
        }
        else
        {
            _activeCable = PoolManager.Instance.Spawn(Pools.Types.CableActivated, transform.position,
                transform.rotation, transform);
        }
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}
