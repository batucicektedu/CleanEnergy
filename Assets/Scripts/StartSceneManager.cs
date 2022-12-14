using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private void Start()
    {
        GameAnalytics.Initialize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
