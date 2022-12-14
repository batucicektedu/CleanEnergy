using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[DefaultExecutionOrder(-1)]
public class SaveState
{
    // Level Manager Related Properties
    public bool isFirstPlay;
    public int lastLevelID;
    public int previousLevelID;
    public int lastRandomLevelID;
    public int levelCounter;
    public List<int> newOrOrderChangedLevels = new List<int>();
    public List<LevelOrderIndex> levelOrder = new List<LevelOrderIndex>();
    public List<int> completedLevels = new List<int>();
    public string levelOrderText;

    public bool isTutorialShown;
    public bool isSecondTutorialComplete;
    public int currentCurrencyAmount;

    public List<EnergyGeneratorSaveInfo> energyGeneratorLevelSaveInfos = new List<EnergyGeneratorSaveInfo>();
    public List<CitySaveInfo> citySaveInfos = new List<CitySaveInfo>();

    public void LoadPreCreatedSaveState()
    {
        energyGeneratorLevelSaveInfos = new List<EnergyGeneratorSaveInfo>
        {
            new EnergyGeneratorSaveInfo(EnergyGeneratorType.SolarPanel),
            new EnergyGeneratorSaveInfo(EnergyGeneratorType.Windmill),
            new EnergyGeneratorSaveInfo(EnergyGeneratorType.Dam),
            new EnergyGeneratorSaveInfo(EnergyGeneratorType.Geyser),
            new EnergyGeneratorSaveInfo(EnergyGeneratorType.HamsterWheel),
        };
        
        citySaveInfos = new List<CitySaveInfo>
        {
            new CitySaveInfo(0),
            new CitySaveInfo(0),
            new CitySaveInfo(0),
            new CitySaveInfo(0),
            new CitySaveInfo(0),
            new CitySaveInfo(0),
        };
        
        isFirstPlay = true;
        lastRandomLevelID = -1;
        levelCounter = 1;

        currentCurrencyAmount = 1200;

        isTutorialShown = false;
        isSecondTutorialComplete = false;
    }
}