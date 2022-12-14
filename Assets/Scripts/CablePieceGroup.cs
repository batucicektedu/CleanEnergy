using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CablePieceGroup : MonoBehaviour
{
    public List<CablePiece> cablePieces = new List<CablePiece>();

    public EnergyGenerator energyGenerator;

    public Pools.Types indicatorPoolType = Pools.Types.CableIndicator;

    private List<Vector3> cableElectricTransferPoints = new List<Vector3>();

    private int _activeCableCount;

    private GameObject _indicator;

    [HideInInspector] public bool fullyActivated;

    private void Awake()
    {
        cablePieces = GetComponentsInChildren<CablePiece>().ToList();

        foreach (var cp in cablePieces)
        {
            if (cp.curved)
            {
                cableElectricTransferPoints.Add(cp.transform.GetChild(1).position);
            }
            else
            {
                cableElectricTransferPoints.Add(cp.transform.GetChild(0).position);
            }
        }
    }

    private void Start()
    {
        energyGenerator = GetComponentInParent<EnergyGenerator>();
    }

    public void IncreaseActiveCableCount()
    {
        _activeCableCount++;

        CheckForFullActivation();
    }

    private void CheckForFullActivation()
    {
        if (cablePieces.Count == _activeCableCount)
        {
            fullyActivated = true;
            energyGenerator.ACableConnectionFinished();
        }
    }

    public void ActivateAllCables()
    {
        foreach (var cablePiece in cablePieces)
        {
            cablePiece.ActivateCable();
        }

        fullyActivated = true;
    }

    public void StartIndicator()
    {
        if (cablePieces[_activeCableCount].activated) return;
        if (_indicator != null) return;
        
        _indicator = PoolManager.Instance.Spawn(indicatorPoolType, 
            cablePieces[_activeCableCount].transform.position,
            Quaternion.identity);
        
        cablePieces[0].EnableCollider();
    }

    public void MoveIndicatorToNextCablePosition()
    {
        IncreaseActiveCableCount();
        
        if (_activeCableCount >= cablePieces.Count)
        {
            DisableIndicator();
            return;
        }
        
        _indicator.transform.position = cablePieces[_activeCableCount].transform.position;
        
        cablePieces[_activeCableCount].EnableCollider();
    }

    public void DisableIndicator()
    {
        if (_indicator != null)
        {
            PoolManager.Instance.Despawn(indicatorPoolType, _indicator);
        }
    }

    public Vector3[] GetElectricParticleTransferPointList()
    {
        return cableElectricTransferPoints.ToArray();
    }
}
