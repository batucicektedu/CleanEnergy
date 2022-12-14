using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using udoEventSystem;
using Unity.Mathematics;
using UnityEngine;

public class MoneyReceiveController : MonoBehaviour
{
    public static MoneyReceiveController Instance { private set; get; }

    public float moneyAccumulateTimer = 2;

    private int _accumulatedMoneyAmount;

    private float _timerStartTime;
    
    public Transform moneyPopUpTransform;
    
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
    }

    private void Update()
    {
        if (_timerStartTime + moneyAccumulateTimer < Time.time && _accumulatedMoneyAmount != 0)
        {
            SaveManager.Instance.IncreaseCurrencyAmount(_accumulatedMoneyAmount);
            EventManager.Get<UpdateMoneyAmountText>().Execute();

            TMP_Text spawnedText = PoolManager.Instance.Spawn(Pools.Types.PopUpMoneyText, moneyPopUpTransform.position,
                Quaternion.identity, moneyPopUpTransform).GetComponent<TMP_Text>();

            spawnedText.text = "+" + _accumulatedMoneyAmount;
            
            _accumulatedMoneyAmount = 0;
        }
    }

    public void IncreaseAccumulatedMoneys(int moneyAmount)
    {
        _timerStartTime = Time.time;
        
        _accumulatedMoneyAmount += moneyAmount;
    }
}
