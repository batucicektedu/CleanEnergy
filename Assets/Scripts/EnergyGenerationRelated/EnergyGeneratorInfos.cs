using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EnergyGeneratorInfo")]
public class EnergyGeneratorInfos : ScriptableObject
{
    public List<EnergyGeneratorInfo> energyGeneratorInfos;

    public EnergyGeneratorInfo GetEnergyInfoFromID(EnergyGeneratorType energyGeneratorType)
    {
        return energyGeneratorInfos.Find(egi => egi.generatorType == energyGeneratorType);
    }
}

[System.Serializable]
public class EnergyGeneratorInfo
{
    public EnergyGeneratorType generatorType;
    public List<EnergyGeneratorLevelInfo> generatorLevelInfos;
}

[System.Serializable]
public class EnergyGeneratorLevelInfo
{
    public int level;
    public int levelUnlockCost;
    public int energyCount;
    public Pools.Types prefabPoolID;
}