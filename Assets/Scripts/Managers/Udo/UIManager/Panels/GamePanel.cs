using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using udoEventSystem;
using TMPro;
using UnityEngine.UI;

public class GamePanel : PanelWithCurrencyAmounts
{
    public TMP_Text moneyAmountText;

    public TMP_Text levelText;

    private int _currentMoneyAmount;

    private void Start()
    {
        SetLevelText();
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        _currentMoneyAmount = SaveManager.Instance.state.currentCurrencyAmount;
        moneyAmountText.text = _currentMoneyAmount.ToString();
    }

    private void OnEnable()
    {
        EventManager.Get<UpdateMoneyAmountText>().AddListener(UpdateMoneyText);
        EventManager.Get<UpdateMoneyAmountTextWithCounter>().AddListener(SetLevelTextWithCounter);
    }
    private void OnDisable()
    {
        EventManager.Get<UpdateMoneyAmountText>().RemoveListener(UpdateMoneyText);
        EventManager.Get<UpdateMoneyAmountTextWithCounter>().RemoveListener(SetLevelTextWithCounter);
    }
    
    private void SetLevelText()
    {
        levelText.text = "Level " + SaveManager.Instance.GetLevelCounter();
    }
    
    private void SetLevelTextWithCounter()
    {
        AnimateCurrencyAmountText(currencyAmountHolders[0].HolderID, CurrencyAnimationType.Counter, SaveManager.Instance.state.currentCurrencyAmount);
    }

    private void DisableLevelText()
    {
        levelText.gameObject.SetActive(false);
    }
}
