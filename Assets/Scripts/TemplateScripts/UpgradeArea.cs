using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeArea : PayArea
{
    private EnergyGenerator _energyGenerator;

    private void Awake()
    {
        _energyGenerator = GetComponentInParent<EnergyGenerator>();
    }
    
    protected override void CheckAndPerformActionIfPossible()
    {
        if (_payUpAmountTextValue != 0)return;
        
        _energyGenerator.UpgradeEnergyGenerator();
        
        gameObject.SetActive(false);
    }
}
