using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ElectricityParticleTransferController : MonoBehaviour
{
    private const Pools.Types ElectricParticlePoolType = Pools.Types.CableElectricityTransferParticle;
    private const Pools.Types ElectricParticleYellowPoolType = Pools.Types.CableElectricityTransferParticleYellow;
    private const PathType PathType = DG.Tweening.PathType.CatmullRom;
    
    private EnergyGenerator _energyGenerator;
    private CablePieceGroup _cablePieceGroup;

    private float _lastElectricParticleTime;

    private void Awake()
    {
        _cablePieceGroup = GetComponent<CablePieceGroup>();
        _energyGenerator = GetComponentInParent<EnergyGenerator>();
    }

    private void OnEnable()
    {
        _lastElectricParticleTime = Time.time;
    }

    private void Update()
    {
        CheckForElectricTransferNeedAndTransfer();
    }

    private void CheckForElectricTransferNeedAndTransfer()
    {
        if (!_cablePieceGroup.fullyActivated) return;
        if (_lastElectricParticleTime + GameDataManager.Instance.ElectricParticleInterval <= Time.time)
        {
            ElectricTransfer();
            _lastElectricParticleTime = Time.time;
        }
    }

    private void ElectricTransfer()
    {
        Transform particleTransform = PoolManager.Instance.Spawn(ElectricParticlePoolType
            , _energyGenerator._trafo.transform.position, Quaternion.identity).transform;

        particleTransform.DOPath(_cablePieceGroup.GetElectricParticleTransferPointList()
            , GameDataManager.Instance.TransferSpeed, PathType).SetEase(Ease.Linear).SetSpeedBased().OnComplete(()=>
            PoolManager.Instance.Despawn(ElectricParticlePoolType, particleTransform.gameObject));
    }

    public void ElectricTransferYellow()
    {
        Transform particleTransform = PoolManager.Instance.Spawn(ElectricParticleYellowPoolType
            , _energyGenerator._trafo.transform.position, Quaternion.identity).transform;

        particleTransform.DOPath(_cablePieceGroup.GetElectricParticleTransferPointList()
            , GameDataManager.Instance.YellowBigParticleDuration, PathType).SetEase(Ease.Linear).OnComplete(() =>
            DOVirtual.DelayedCall(0.5f, () =>
            {
                PoolManager.Instance.Despawn(ElectricParticleYellowPoolType, particleTransform.gameObject);
            }, false));
    }
}
