using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { private set; get; }
    
    [Header("PayUpArea")]
    [SerializeField] private float _moneyStackSpawnedPerSec;
    [SerializeField] private float _totalPayUpDuration;
    [SerializeField] private float _maxPayUpDuration;
    [SerializeField] private float _payUpDurationDefaultAmount;
    [SerializeField] private Material _litUpMat;
    [SerializeField] private EnergyGeneratorInfos _energyGeneratorInfos;
    [SerializeField] private CollectableJumpSettings _collectableJumpSettings;
    [SerializeField] private float _camSwitchDuration;
    [SerializeField] private float _moneyTriggerInterval;
    
    [Header("ElectricTransfer")]
    [SerializeField] private float _transferSpeed = 40;
    [SerializeField] private float _yellowBigParticleDuration = 1;
    [SerializeField] private float _electricParticleInterval = 2.5f;

    public float MoneyStackSpawnedPerSec => _moneyStackSpawnedPerSec;
    public float TotalPayUpDuration => _totalPayUpDuration;
    public float MaxPayUpDuration => _maxPayUpDuration;
    public float PayUpDurationDefaultAmount => _payUpDurationDefaultAmount;
    public Material LitUpMat => _litUpMat;
    public EnergyGeneratorInfos EnergyGeneratorInfos => _energyGeneratorInfos;
    public CollectableJumpSettings CollectableJumpSettings => _collectableJumpSettings;
    public float CamSwitchDuration => _camSwitchDuration;
    public float TransferSpeed => _transferSpeed;
    public float YellowBigParticleDuration => _yellowBigParticleDuration;
    public float ElectricParticleInterval => _electricParticleInterval;
    public float MoneyTriggerInterval => _moneyTriggerInterval;
    
    public float GetMoneySpawnInterval()
    {
        return 1 / _moneyStackSpawnedPerSec;
    }

    public float GetTotalMoneySpawnsNeededForTotalPayUp()
    {
        return _moneyStackSpawnedPerSec * _totalPayUpDuration;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}
