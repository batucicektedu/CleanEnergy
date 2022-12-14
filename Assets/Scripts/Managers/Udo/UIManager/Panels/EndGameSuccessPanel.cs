using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSuccessPanel : Panel
{
    public void NextButtonPressed()
    {
        LevelManager.Instance.OpenCurrentLevel();
    }
}
