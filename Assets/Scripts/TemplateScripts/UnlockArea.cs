using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockArea : PayArea
{
    private EnergyGenerator _energyGenerator;

    private void Awake()
    {
        _energyGenerator = GetComponentInParent<EnergyGenerator>();
    }

    protected override void CheckAndPerformActionIfPossible()
    {
        if (_payUpAmountTextValue != 0)return;
        
        _energyGenerator.UnlockEnergyGenerator();
        
        gameObject.SetActive(false);
    }
}
