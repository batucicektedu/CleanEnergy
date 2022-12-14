using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToStartPanel : Panel
{
    public void StartGameAndDisableThisPanel()
    {
        GameManager.Instance.State = GameState.Play;
        
        gameObject.SetActive(false);
    }
}
