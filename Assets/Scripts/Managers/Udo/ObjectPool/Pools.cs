using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
    public enum Types
    {
        Example = 5,
        MoneyStack = 10,
        MoneyCollectable = 11,
        PopUpMoneyText = 15,
        
        SolarPanelLevel1 = 50,
        SolarPanelLevel2 = 55,
        SolarPanelLevel3 = 60,
        SolarPanelLevel4 = 65,
        SolarPanelLevel5 = 70,
        SolarPanelLevel6 = 75,
        
        WindMillLevel1 = 100,
        WindMillLevel2 = 105,
        WindMillLevel3 = 110,
        WindMillLevel4 = 115,
        WindMillLevel5 = 120,
        WindMillLevel6 = 125,
        
        DamLevel1 = 200,
        DamLevel2 = 205,
        DamLevel3 = 210,
        DamLevel4 = 215,
        DamLevel5 = 220,
        DamLevel6 = 225,
        
        GeyserLevel1 = 300,
        GeyserLevel2 = 305,
        GeyserLevel3 = 310,
        
        HamsterWheel1 = 400,
        HamsterWheel2 = 405,
        HamsterWheel3 = 410,
        
        ThrashBag = 500,
        CablePiece = 505,
        
        CableTransparent = 510,
        CableTransparentCurved = 511,
        CableActivated = 515,
        CableActivatedCurved = 516,
        CableIndicator = 520,
        
        CableElectricityTransferParticle = 600,
        CableElectricityTransferParticleYellow = 605,
    }

    public static string GetTypeStr(Types poolType)
    {
        return Enum.GetName(typeof(Types), poolType);
    }
}
