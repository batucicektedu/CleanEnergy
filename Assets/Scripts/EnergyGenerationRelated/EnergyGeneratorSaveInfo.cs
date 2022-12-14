using System;
using System.Collections.Generic;

[System.Serializable]
public class EnergyGeneratorSaveInfo
{
    public EnergyGeneratorType energyGeneratorType;

    public List<IndividualGeneratorInfo> individualGeneratorInfos;

    public EnergyGeneratorSaveInfo(EnergyGeneratorType newEnergyGeneratorType)
    {
        energyGeneratorType = newEnergyGeneratorType;
        individualGeneratorInfos= new List<IndividualGeneratorInfo>
        {
            new IndividualGeneratorInfo(),
            new IndividualGeneratorInfo(),
            new IndividualGeneratorInfo(),
            new IndividualGeneratorInfo(),
            new IndividualGeneratorInfo(),
        };;
    }

    public EnergyGeneratorSaveInfo()
    {
        //For serialization
    }

    [Serializable]
    public class IndividualGeneratorInfo
    {
        public int level;
        public int connectedCableCount;

        public IndividualGeneratorInfo()
        {
            level = 0;
            connectedCableCount = 0;
        }
    }
}
