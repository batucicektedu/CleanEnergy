using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    [HideInInspector]
    public bool preventSaving;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Load();
        }
        else
        {
            Destroy(this);
        }
    }
    
    // Save the whole state of this saveState script to the player pref
    public void Save()
    {
        if (preventSaving)
        {
            return;
        }
        
        PlayerPrefs.SetString("save", Helper.Encrypt(Helper.Serialize<SaveState>(state)));
        PlayerPrefs.Save();
    }

    // Save the whole state of this saveState script to the player pref
    public void SavePreCreatedData()
    {
        if (preventSaving)
        {
            return;
        }

        state.LoadPreCreatedSaveState();
        PlayerPrefs.SetString("save", Helper.Encrypt(Helper.Serialize<SaveState>(state)));
        PlayerPrefs.Save();
    }

    // Load the previous saved state from the player prefs
    public void Load()
    {
        // Do we already have a save?
        if(PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(Helper.Decrypt(PlayerPrefs.GetString("save")));
        }
        else
        {
            state = new SaveState();
            state.LoadPreCreatedSaveState();
            Save();
            Debug.Log("No save file found, creating a new one!");
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }

    public int IncreaseLitObjectCount(int amount, int cityID, int stateID)
    {
        state.citySaveInfos[cityID].stateLitUpObjectCounts[stateID] += amount;
        Save();
        return state.citySaveInfos[cityID].stateLitUpObjectCounts[stateID];
    }

    public int GetLitObjectCount(int cityID, int stateID)
    {
        return state.citySaveInfos[cityID].stateLitUpObjectCounts[stateID];
    }

    public int GetEnergyGeneratorLevel(EnergyGeneratorType energyGeneratorType, int generatorID)
    {
        return state.energyGeneratorLevelSaveInfos.FirstOrDefault(egi 
                => egi.energyGeneratorType == energyGeneratorType)
            .individualGeneratorInfos[generatorID].level;
    }
    
    public int GetEnergyGeneratorConnectedCableCount(EnergyGeneratorType energyGeneratorType, int generatorID)
    {
        return state.energyGeneratorLevelSaveInfos.FirstOrDefault(egi 
                => egi.energyGeneratorType == energyGeneratorType)
            .individualGeneratorInfos[generatorID].connectedCableCount;
    }
    
    public void SetEnergyGeneratorLevel(EnergyGeneratorType energyGeneratorType, int generatorID
        , int level)
    {
        state.energyGeneratorLevelSaveInfos.FirstOrDefault(egi 
                => egi.energyGeneratorType == energyGeneratorType)
            .individualGeneratorInfos[generatorID].level = level;

        Save();
    }
    
    public void SetEnergyGeneratorConnectedCableCount(EnergyGeneratorType energyGeneratorType, int generatorID,
        int connectedCableCount)
    {
        state.energyGeneratorLevelSaveInfos.FirstOrDefault(egi 
                => egi.energyGeneratorType == energyGeneratorType)
            .individualGeneratorInfos[generatorID].connectedCableCount = connectedCableCount;
        Save();
    }

    public int IncreaseCurrencyAmount(int amount)
    {
        state.currentCurrencyAmount += amount;
        Save();
        return state.currentCurrencyAmount;
    }

    public void SetLastLevelID(int lastLevelID)
    {
        state.lastLevelID = lastLevelID;
        Save();
    }

    public int GetLastLevelID()
    {
        return state.lastLevelID;
    }

    public void SetPreviousLevelID(int previousLevelID)
    {
        state.previousLevelID = previousLevelID;
        Save();
    }

    public int GetPreviousLevelID()
    {
        return state.previousLevelID;
    }

    public void SetLastRandomLevelID(int lastRandomLevelID)
    {
        state.lastRandomLevelID = lastRandomLevelID;
        Save();
    }

    public int GetLastRandomLevelID()
    {
        return state.lastRandomLevelID;
    }

    public void IncreaseLevelCounter()
    {
        state.levelCounter++;
        Save();
    }
    
    public int GetLevelCounter()
    {
        return state.levelCounter;
    }

    public void SetIsFirstPlay()
    {
        state.isFirstPlay = false;
    }

    public bool IsFirstPlay()
    {
        return state.isFirstPlay;
    }

    public List<int> GetNewOrOrderChangedLevels()
    {
        return state.newOrOrderChangedLevels;
    }

    public void RemoveLevelFromNewOrOrderChangedLevels(int levelID)
    {
        if (state.newOrOrderChangedLevels.Contains(levelID))
        {
            state.newOrOrderChangedLevels.Remove(levelID);
            Save();
        }
    }

    public void AddLevelsToNewOrOrderChangedLevels(List<int> levelIDs)
    {
        foreach (int levelID in levelIDs)
        {
            if (state.newOrOrderChangedLevels.Contains(levelID))
                continue;

            state.newOrOrderChangedLevels.Add(levelID);
        }
        
        Save();
    }

    public void ClearNewOrOrderChangedLevels()
    {
        if (state.newOrOrderChangedLevels.Count > 0)
        {
            state.newOrOrderChangedLevels.Clear();
            Save();
        }
    }

    public List<LevelOrderIndex> GetLevelOrder()
    {
        return state.levelOrder;
    }

    public void SetNewLevelOrder(List<LevelOrderIndex> levelOrder)
    {
        state.levelOrder.Clear();
        foreach (LevelOrderIndex levelOrderLevel in levelOrder)
            state.levelOrder.Add(levelOrderLevel);
        Save();
    }

    public bool IsLevelCompleted(int levelID)
    {
        return state.completedLevels.Contains(levelID);
    }

    public void AddLevelToCompletedLevels(int levelID)
    {
        if (!state.completedLevels.Contains(levelID))
        {
            state.completedLevels.Add(levelID);
            Save();
        }
    }
}
