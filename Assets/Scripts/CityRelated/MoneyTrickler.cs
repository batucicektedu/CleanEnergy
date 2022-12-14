using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using udoEventSystem;
using UnityEngine;

public class MoneyTrickler : MonoBehaviour
{
    [Header("MoneyTrickleSettings")]
    public int moneyTrickleAmount = 50;
    public Vector3 textSpawnOffset = Vector3.up * 3;
    private float _lastMoneyTrickledTime  = Mathf.NegativeInfinity;

    public Pools.Types moneyTextPoolType = Pools.Types.PopUpMoneyText;

    private bool _tricklingMoney;

    [HideInInspector]public CityState _cityState;

    public Transform moneySpawnLocation;

    private void Awake()
    {
        if (GetComponentInParent<CityState>())
        {
            _cityState = GetComponentInParent<CityState>();
        }
    }

    private void Update()
    {
        if (_tricklingMoney)
        {
            TrickleMoney();
        }
    }

    private void TrickleMoney()
    {
        if (_lastMoneyTrickledTime + GameDataManager.Instance.MoneyTriggerInterval > Time.time) return;

        _lastMoneyTrickledTime = Time.time;

        if (moneySpawnLocation == null)
        {
            _cityState.SpawnMoney(Vector3.zero);
        }
        else
        {
            _cityState.SpawnMoney(moneySpawnLocation.position);
        }
        
        
        // GameObject spawnedText = PoolManager.Instance.Spawn(moneyTextPoolType, transform.position + textSpawnOffset,
        //     transform.rotation);
        //
        // spawnedText.GetComponent<TMP_Text>().text = "+" + moneyTrickleAmount;
        //
        // SaveManager.Instance.IncreaseCurrencyAmount(moneyTrickleAmount);
        // EventManager.Get<UpdateMoneyAmountText>().Execute();
    }

    public void StartMoneyTrickle()
    {
        _tricklingMoney = true;
    }
}
