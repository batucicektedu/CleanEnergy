using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int levelID;

    public int LevelID => levelID;

    public void SetLevelID(int levelID) 
    {
        this.levelID = levelID;
    }
    public virtual void ResetLevel(ILevelResetProperties levelResetProperties) { }
}
