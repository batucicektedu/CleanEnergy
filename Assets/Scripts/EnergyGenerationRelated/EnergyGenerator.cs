using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using udoEventSystem;
using UnityEngine;

public class EnergyGenerator : MonoBehaviour
{
    public EnergyGeneratorType energyGeneratorType;
    public int saveID;
    public GameObject unlockArea;
    public GameObject upgradeArea;
    public Transform generatorSpawnPoint;
    private GameObject _spawnedGeneratorObject;
    public CityState cityState;

    public float upgradeLocationSpawnDelay;

    public int maxLevelDecrease;
    
    private List<CablePieceGroup> _cablePieceGroups = new List<CablePieceGroup>();

    private int _level;

    private int _currentEnergyCount;

    private int _finishedConnectionCount;

    private int _lastEnabledCableIndex;

    public Trafo _trafo { private set; get; }

    private void Awake()
    {
        _trafo = GetComponentInChildren<Trafo>();
        _cablePieceGroups = GetComponentsInChildren<CablePieceGroup>(true).ToList();

        cityState.moneyPile = GetComponentInChildren<MoneyPile>();
    }

    private void Start()
    {
        LoadConnectionCount();

        LoadGeneratorLevel();
        
        SetCostTextValue();
    }

    public void UpgradeEnergyGenerator()
    {
        _level++;

        if (!SaveManager.Instance.state.isSecondTutorialComplete)
        {
            TutorialManager.Instance.SecondTutorialComplete();
        }

        if (_finishedConnectionCount == _level / 2 && _level % 2 == 0)
        {
            SpawnObjectOfLevel(_level, false, true);
        }
        else
        {
            SpawnObjectOfLevel(_level);
        }
        

        SaveGeneratorLevel();

        SetCostTextValue();
        
        upgradeArea.SetActive(false);

        if (_level != GetMaxLevel())
        {
            DOVirtual.DelayedCall(upgradeLocationSpawnDelay, () =>
            {
                upgradeArea.SetActive(true);
            }, false);
        }
        
        if (energyGeneratorType == EnergyGeneratorType.Dam)
        {
            MyCamSwitcher.Instance.SwitchToCamThenTurnBackDisablingInput(MyCamSwitcher.CamStates.DamViewCam);
            SendHouseLightUpData();
        }
        else if (_finishedConnectionCount == _level / 2 && _level % 2 == 0)
        {
            MyCamSwitcher.Instance.ChangeCamTargetToCityCamWithID(cityState.cityViewCameraTarget, cityState.city.cityID);
            MyCamSwitcher.Instance.SwitchToCamThenTurnBackDisablingInput(cityState.city.cityID);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                _cablePieceGroups[(_level-1) / 2]
                    .GetComponent<ElectricityParticleTransferController>().ElectricTransferYellow();
            }, false);
            
            DOVirtual.DelayedCall(GameDataManager.Instance.YellowBigParticleDuration + 0.5f
                , SendHouseLightUpData, false);
        }
    }
    
    public void UnlockEnergyGenerator()
    {
        if (!SaveManager.Instance.state.isTutorialShown)
        {
            TutorialManager.Instance.SpawnTutorialThrash();
        }
        
        _level++;
        
        SpawnObjectOfLevel(_level);

        SaveGeneratorLevel();

        SetCostTextValue();
        
        unlockArea.SetActive(false);
        
        if (_level != GetMaxLevel())
        {
            DOVirtual.DelayedCall(upgradeLocationSpawnDelay, () =>
            {
                if (SaveManager.Instance.state.isTutorialShown)
                {
                    upgradeArea.SetActive(true);
                }
            }, false);
        }
        
        if (energyGeneratorType == EnergyGeneratorType.Dam)
        {
            MyCamSwitcher.Instance.SwitchToCamThenTurnBackDisablingInput(MyCamSwitcher.CamStates.DamViewCam);
        }
    }

    private void LoadGeneratorLevel()
    {
        var loadedLevel = SaveManager.Instance.GetEnergyGeneratorLevel(energyGeneratorType, saveID);

        if (loadedLevel == 0) return;

        _level = loadedLevel;
        
        SpawnObjectOfLevel(_level, true);

        _currentEnergyCount = GameDataManager.Instance.EnergyGeneratorInfos
            .GetEnergyInfoFromID(
                energyGeneratorType).generatorLevelInfos[_level - 1].energyCount;
        
        unlockArea.SetActive(false);

        if (_level != GetMaxLevel())
        {
            if (SaveManager.Instance.state.isSecondTutorialComplete)
            {
                upgradeArea.SetActive(true);
            }
        }
    }

    private void LoadConnectionCount()
    {
        var loadedLevel = SaveManager.Instance.GetEnergyGeneratorLevel(energyGeneratorType, saveID);
        
        _finishedConnectionCount =
            SaveManager.Instance.GetEnergyGeneratorConnectedCableCount(energyGeneratorType, saveID);

        if (loadedLevel == 0) return;

        _level = loadedLevel;
        
        for (int i = 0; i < (_level+1) / 2; i++)
        {
            EnableCable(i);
        }
        
        for (int i = 0; i < _finishedConnectionCount; i++)
        {
            _cablePieceGroups[i].ActivateAllCables();
        }

        // if (_finishedConnectionCount < _cablePieceGroups.Count)
        // {
        //     _cablePieceGroups[_finishedConnectionCount].StartIndicator();
        // }
    }

    private void SaveGeneratorLevel()
    {
        SaveManager.Instance.SetEnergyGeneratorLevel(energyGeneratorType, saveID, _level);
    }

    private void SendHouseLightUpData()
    {
        if (_finishedConnectionCount == 0) return;
        
        int energyLevel = Mathf.Min(_level, _finishedConnectionCount * 2);
        
        var energyOfCurrentLevel = GameDataManager.Instance.EnergyGeneratorInfos
            .GetEnergyInfoFromID(
                energyGeneratorType).generatorLevelInfos[energyLevel - 1].energyCount;
        
        cityState.LightObjectsTillAmountReached(energyOfCurrentLevel);
    }

    private void SetCostTextValue()
    {
        if (_level == GetMaxLevel()) return;
        
        var nextLevelUnlockValue = GameDataManager.Instance.EnergyGeneratorInfos.GetEnergyInfoFromID(
            energyGeneratorType).generatorLevelInfos[_level].levelUnlockCost;
        
        if (_level == 0)
        {
            unlockArea.GetComponent<UnlockArea>().SetNewValue(nextLevelUnlockValue);
        }
        else
        {
            upgradeArea.GetComponent<UpgradeArea>().SetNewValue(nextLevelUnlockValue);
        }
    }

    private void SpawnObjectOfLevel(int level, bool loading = false, bool dontSwitchCam = false)
    {
        if (_level > 0)
        {
            _trafo.Electrify();
        }
        
        if (_finishedConnectionCount == (_level-1) / 2)
        {
            if (loading)
            {
                EnableCable((_level-1) / 2);
            
                _cablePieceGroups[(_level-1) / 2].StartIndicator();
            }
            else
            {
                if (!dontSwitchCam)
                {
                    MyCamSwitcher.Instance.SwitchMainCamTargetThenTurnBackDisablingInputWithDampening(_trafo.transform);
                }

                DOVirtual.DelayedCall(GameDataManager.Instance.CamSwitchDuration / 2, () =>
                {
                    EnableCable((_level-1) / 2);
            
                    _cablePieceGroups[(_level-1) / 2].StartIndicator();
                }, false);
            }
        }
        else// if (_lastEnabledCableIndex < (_level-1) / 2)
        {
            if (loading)
            {
                EnableCable((_level-1) / 2);
            }
            else
            {
                if (!dontSwitchCam)
                {
                    MyCamSwitcher.Instance.SwitchMainCamTargetThenTurnBackDisablingInputWithDampening(_trafo.transform);
                }
                
                DOVirtual.DelayedCall(GameDataManager.Instance.CamSwitchDuration / 2, () =>
                {
                    EnableCable((_level-1) / 2);
                }, false);
            }
        }
        // else
        // {
        //     _cablePieceGroups[_level / 2].DisableIndicator();
        // }

        if (loading)
        {
            _spawnedGeneratorObject = PoolManager.Instance.Spawn(
                GameDataManager.Instance.EnergyGeneratorInfos.GetEnergyInfoFromID(
                    energyGeneratorType).generatorLevelInfos[_level - 1].prefabPoolID, 
                generatorSpawnPoint.position, generatorSpawnPoint.rotation);
        }
        else
        {
            DOVirtual.DelayedCall(GameDataManager.Instance.CamSwitchDuration / 2, () =>
            {
                if (_level != 1)
                {
                    PoolManager.Instance.Despawn(GameDataManager.Instance.EnergyGeneratorInfos.GetEnergyInfoFromID(
                        energyGeneratorType).generatorLevelInfos[_level - 2].prefabPoolID, _spawnedGeneratorObject);
                }
                
                _spawnedGeneratorObject = PoolManager.Instance.Spawn(
                    GameDataManager.Instance.EnergyGeneratorInfos.GetEnergyInfoFromID(
                        energyGeneratorType).generatorLevelInfos[_level - 1].prefabPoolID, 
                    generatorSpawnPoint.position, generatorSpawnPoint.rotation);
            }, false);
        }
    }

    private int GetMaxLevel()
    {
        return GameDataManager.Instance.EnergyGeneratorInfos.GetEnergyInfoFromID(
            energyGeneratorType).generatorLevelInfos.Count - maxLevelDecrease;
    }

    public void ACableConnectionFinished()
    {
        if (!SaveManager.Instance.state.isSecondTutorialComplete)
        {
            TutorialManager.Instance.SecondTutorialStarted();
        }
        
        _finishedConnectionCount++;
        
        SaveManager.Instance.SetEnergyGeneratorConnectedCableCount(energyGeneratorType
            , saveID, _finishedConnectionCount);
        
        MyCamSwitcher.Instance.ChangeCamTargetToCityCamWithID(cityState.cityViewCameraTarget, cityState.city.cityID);
        MyCamSwitcher.Instance.SwitchToCamThenTurnBackDisablingInput(cityState.city.cityID);
        
        DOVirtual.DelayedCall(0.5f, () =>
        {
            _cablePieceGroups[(_level-1) / 2]
                .GetComponent<ElectricityParticleTransferController>().ElectricTransferYellow();
        }, false);

        DOVirtual.DelayedCall(GameDataManager.Instance.YellowBigParticleDuration + 0.5f, () =>
        {
            SendHouseLightUpData();
            
            if (_finishedConnectionCount < (_level+1)/2 && _finishedConnectionCount < _cablePieceGroups.Count)
            {
                _cablePieceGroups[_finishedConnectionCount].StartIndicator();
            }
            
            EventManager.Get<CableConnectedFromCity>().Execute(cityState.city.cityID);
        
            cityState.ElectrifyTrafo(_finishedConnectionCount);
        
            Debug.Log("A cable connection finished");
        }, false);
    }

    private void EnableCable(int index)
    {
        _cablePieceGroups[index].gameObject.SetActive(true);

        _lastEnabledCableIndex = index;
        
        cityState.EnableTrafo(index, _finishedConnectionCount);
    }
}
