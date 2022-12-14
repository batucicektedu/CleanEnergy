using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using udoEventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Play,
    Pause,
    Success,
    EndingStarted,
    EndingProcess,
    Failed
}

[DefaultExecutionOrder(0)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float successUIDelay = 1;
    public float failUIDelay = 1;

    private GameState _state;
    public GameState State
    {
        get => _state;
        set
        {
            if (value == _state) return;
            _state = value;
            EventManager.Get<OnGameStateChanged>().Execute(_state);
            if (_state == GameState.Success)
            {
                EventManager.Get<LevelCompleted>().Execute();
            }
            if (_state == GameState.EndingProcess)
            {
                EventManager.Get<PauseGame>().Execute();
                EventManager.Get<StopUserInput>().Execute();
            }

            if (_state == GameState.Play)
            {
                EventManager.Get<StartGame>().Execute();
                EventManager.Get<StartUserInput>().Execute();
            }
            if (_state == GameState.Pause)
            {
                EventManager.Get<PauseGame>().Execute();
                EventManager.Get<StopUserInput>().Execute();
            }

            if (_state == GameState.EndingStarted)
            {
                //ElephantAdvertisementManager.SendAdjustLevelEvent(SaveManager.Instance.state.levelCounter);
            }

            if (_state == GameState.Failed)
            {
                EventManager.Get<StopUserInput>().Execute();
                EventManager.Get<PauseGame>().Execute();
                EventManager.Get<LevelFailed>().Execute();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
         #if UNITY_EDITOR
        //SRDebug.Init();
        #endif

        DOTween.SetTweensCapacity(1000, 500);
    }

    private void OnEnable()
    {
        EventManager.Get<LevelCompleted>().AddListener(LevelCompleted);
        EventManager.Get<LevelFailed>().AddListener(LevelFailed);
    }

    private void OnDisable()
    {
        EventManager.Get<LevelCompleted>().RemoveListener(LevelCompleted);
        EventManager.Get<LevelFailed>().RemoveListener(LevelFailed);
    }

    private void Start()
    {
        //Elephant.LevelStarted(SaveManager.Instance.state.levelCounter);

        //State = GameState.Pause;
        
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    public void RestartScene()
    {
        LevelManager.Instance.OpenCurrentLevel();
    }

    public void NextScene()
    {
        LevelManager.Instance.ChangeToNextLevel(true);

    }

    public void StartGame()
    {
        State = GameState.Play;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SaveManager.Instance.IncreaseCurrencyAmount(1000);
            EventManager.Get<UpdateMoneyAmountText>().Execute();
        }
    }

    private void LevelCompleted()
    {
        //moneyGainedThisLevel = currentMoneyGainPerLevel + yardIncomeIncreaseThisLevel;

        //Elephant.LevelCompleted(SaveManager.Instance.state.levelCounter);

        DOVirtual.DelayedCall(successUIDelay, () =>
        {
            UIManager.Instance.ShowPanel(Panel.Type.LevelCompleted);
        });
    }

    private void LevelFailed()
    {
        //Elephant.LevelFailed(SaveManager.Instance.state.levelCounter);

        DOVirtual.DelayedCall(failUIDelay, () =>
        {
            UIManager.Instance.ShowPanel(Panel.Type.FailedLevel);
        });
    }
}
