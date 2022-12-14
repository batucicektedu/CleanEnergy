using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using udoEventSystem;
using UnityEngine;

public class PayArea : MonoBehaviour
{
    public int payUpAmount;
    public TMP_Text _payUpAmountText;

    private int _payUpAmountLeft;
    private bool _inPayUpProcess;
    private float _lastMoneySpawnTime;
    private int _moneyPerStack;

    private Tween _textCounterTween;
    protected int _payUpAmountTextValue;

    private void Update()
    {
        CheckAndSpawnMoney();
    }

    private void OnDisable()
    {
        StopPayUpProcess();
    }

    private void CheckAndSpawnMoney()
    {
        if (!_inPayUpProcess) return;
        if (_lastMoneySpawnTime + GameDataManager.Instance.GetMoneySpawnInterval() > Time.time) return;
        if (SaveManager.Instance.state.currentCurrencyAmount == 0) return;
        if (_payUpAmountLeft == 0) return;
        
        _lastMoneySpawnTime = Time.time;
        
        var thisMoneyPerStack = _moneyPerStack;

        //Check If money is not enough for one mon ey stack
        if (SaveManager.Instance.state.currentCurrencyAmount < thisMoneyPerStack)
        {
            thisMoneyPerStack = SaveManager.Instance.state.currentCurrencyAmount;
        }
        //Check if pay up amount left is lower than one money stack amount
        if (_payUpAmountLeft < thisMoneyPerStack)
        {
            thisMoneyPerStack = _payUpAmountLeft;
        }

        _payUpAmountLeft -= thisMoneyPerStack;

        SaveManager.Instance.IncreaseCurrencyAmount(-thisMoneyPerStack);
        EventManager.Get<UpdateMoneyAmountTextWithCounter>().Execute();

        GameObject spawnedMoneyFromPool = PoolManager.Instance.Spawn(Pools.Types.MoneyStack);

        spawnedMoneyFromPool.transform.position = Player.Instance.transform.position + Vector3.up;

        spawnedMoneyFromPool.GetComponent<FlyingMoneyStack>().Movement(transform.position, ChangeMoneyAmountTextWithCounter, thisMoneyPerStack);
    }

    private void ChangeMoneyAmountTextWithCounter(int amount = 0)
    {
        _textCounterTween?.Kill(true);

        _payUpAmountTextValue -= amount;
        
        _textCounterTween = _payUpAmountText.DOCounter(_payUpAmountTextValue + amount, _payUpAmountTextValue,
            GameDataManager.Instance.GetMoneySpawnInterval() * 0.9f, false).OnComplete(CheckAndPerformActionIfPossible);
    }

    protected virtual void CheckAndPerformActionIfPossible()
    {
        
    }

    public void SetNewValue(int newValue)
    {
        payUpAmount = newValue;
        _payUpAmountText.text = newValue.ToString();
        _payUpAmountLeft = newValue;
        _payUpAmountTextValue = newValue;
        _moneyPerStack = GetAmountOfMoneyPerStack();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerMovementControllerOmniDirectional>().IsMoving())
            {
                _inPayUpProcess = false;
            }
            else
            {
                _inPayUpProcess = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inPayUpProcess = false;
        }
    }

    public void StopPayUpProcess()
    {
        _inPayUpProcess = false;
    }

    private int GetAmountOfMoneyPerStack()
    {
        var payUpDurationMultiplier = payUpAmount / GameDataManager.Instance.PayUpDurationDefaultAmount;

        if (payUpDurationMultiplier > GameDataManager.Instance.MaxPayUpDuration)
        {
            payUpDurationMultiplier = GameDataManager.Instance.MaxPayUpDuration;
        }
        else if (payUpDurationMultiplier < GameDataManager.Instance.TotalPayUpDuration)
        {
            payUpDurationMultiplier = GameDataManager.Instance.TotalPayUpDuration;
        }
        
        return (int)(payUpAmount / GameDataManager.Instance.GetTotalMoneySpawnsNeededForTotalPayUp() / payUpDurationMultiplier);
    }
}
