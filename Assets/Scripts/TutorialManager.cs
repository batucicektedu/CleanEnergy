using System;
using System.Collections;
using System.Collections.Generic;
using udoEventSystem;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { private set; get; }
    
    public int tutorialThrashCount;

    public int tutorialStepCount = 5;

    public GameObject[] enableAfterTutorial;

    private bool _secondTutorialStarted;

    [SerializeField] private EnergyGenerator tutorialEnergyGenerator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("double instance this singleton", gameObject);
            Destroy(gameObject);
        }
        
        if (!SaveManager.Instance.state.isTutorialShown)
        {
            SaveManager.Instance.preventSaving = true;
        }

        if (SaveManager.Instance.state.isSecondTutorialComplete)
        {
            EnableObjectsAfterTutorial();
        }
    }

    public void SpawnTutorialThrash()
    {
        EventManager.Get<SpawnTutorialThrash>().Execute();
        TutorialArrowsLine.Instance.ChangeTarget();
    }

    public void FirstTutorialFinished()
    {
        SaveManager.Instance.preventSaving = false;
        SaveManager.Instance.state.isTutorialShown = true;
        SaveManager.Instance.Save();
        
        print("First tutorial complete");
    }

    public void ChangeToNextTutorialStep()
    {
        if (SaveManager.Instance.state.isTutorialShown) return;
        TutorialArrowsLine.Instance.ChangeTarget();
    }

    public void ChangeTutorialThrashCount(int amount)
    {
        if (amount < 0)
        {
            TutorialArrowsLine.Instance.StopLine();
        }
        
        tutorialThrashCount += amount;

        if (tutorialThrashCount <= 0)
        {
            ChangeToNextTutorialStep();
        }
    }

    public void SecondTutorialStarted()
    {
        _secondTutorialStarted = true;
        TutorialArrowsLine.Instance.ChangeTargetFromSecondTargets();
    }

    public void SecondTutorialComplete()
    {
        EnableObjectsAfterTutorial();
        TutorialArrowsLine.Instance.StopLine();
        SaveManager.Instance.state.isSecondTutorialComplete = true;
        SaveManager.Instance.Save();
        print("Second tutorial complete");
    }

    public void EnableSecondTutorialUpgradeLocation()
    {
        tutorialEnergyGenerator.upgradeArea.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        if (_secondTutorialStarted)
        {
            SecondTutorialComplete();
        }
    }

    private void EnableObjectsAfterTutorial()
    {
        foreach (var gaO in enableAfterTutorial)
        {
            gaO.SetActive(true);
        }
    }
}
