using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class LevelManager : LevelManagerBase
{
    public static LevelManager Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    #region Override Methods For Saving and Gettings Data From SaveManager
    protected override void AddLevelToCompletedLevels(int levelID)
    {
        if (!SaveManager.Instance.state.completedLevels.Contains(levelID))
            SaveManager.Instance.state.completedLevels.Add(levelID);
    }

    protected override void AddNewOrOrderChangedLevels(List<int> newOrOrderChangedLevelIDs)
    {
        foreach (int levelID in newOrOrderChangedLevelIDs)
        {
            if (SaveManager.Instance.state.newOrOrderChangedLevels.Contains(levelID))
                continue;

            SaveManager.Instance.state.newOrOrderChangedLevels.Add(levelID);
        }
    }

    protected override void ClearNewOrOrderChangedLevelsFromSaveData()
    {
        SaveManager.Instance.state.newOrOrderChangedLevels.Clear();
    }

    protected override int GetLastLevelID()
    {
        return SaveManager.Instance.state.lastLevelID;
    }

    protected override int GetLastRandomLevelID()
    {
        return SaveManager.Instance.state.lastRandomLevelID;
    }

    protected override List<LevelOrderIndex> GetLevelOrderFromSaveData()
    {
        return SaveManager.Instance.state.levelOrder;
    }

    protected override List<int> GetNewOrOrderChangedLevels()
    {
        return SaveManager.Instance.state.newOrOrderChangedLevels;
    }

    protected override int GetPreviousLevelID()
    {
        return SaveManager.Instance.state.previousLevelID;
    }

    protected override int GetLevelCounter()
    {
        return SaveManager.Instance.state.levelCounter;
    }

    protected override void IncreaseLevelCounter()
    {
        SaveManager.Instance.state.levelCounter++;
    }

    protected override bool IsFirstPlay()
    {
        return SaveManager.Instance.state.isFirstPlay;
    }

    protected override bool IsLevelCompleted(int levelID)
    {
        return SaveManager.Instance.state.completedLevels.Contains(levelID);
    }

    protected override void RemoveLevelFromNewOrOrderChangedLevels(int levelID)
    {
        if (SaveManager.Instance.state.newOrOrderChangedLevels.Contains(levelID))
            SaveManager.Instance.state.newOrOrderChangedLevels.Remove(levelID);
    }

    protected override void SetIsFirstPlay()
    {
        SaveManager.Instance.state.isFirstPlay = false;
    }

    protected override void SetLastLevelID(int lastLevelID)
    {
        SaveManager.Instance.state.lastLevelID = lastLevelID;
    }

    protected override void SetLastRandomLevelID(int lastRandomLevelID)
    {
        SaveManager.Instance.state.lastRandomLevelID = lastRandomLevelID;
    }

    protected override void SetLevelOrder(List<LevelOrderIndex> levelOrder)
    {
        SaveManager.Instance.state.levelOrder = levelOrder;
    }

    protected override void SetPreviousLevelID(int previousLevelID)
    {
        SaveManager.Instance.state.previousLevelID = previousLevelID;
    }

    protected override void SaveData()
    {
        SaveManager.Instance.Save();
    }
    #endregion
}
