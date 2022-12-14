using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class PanelWithCurrencyAmounts : Panel
{
    [Header("Currency Amount Panel Settings")]
    [SerializeField] protected List<CurrencyAmountHolder> currencyAmountHolders;
    [SerializeField] protected PanelWithCurrencyAmountsSettings panelSettings;

    [Header("Test Values")]
    [SerializeField] private CurrencyAmountHolderID currencyAmountHolderIDTest;
    [SerializeField] private CurrencyAnimationType animationTypeTest;
    [SerializeField] private int finalAmountTest;
    [SerializeField] private RectTransform initialRectForFlyingCurrenciesTest;

    private Dictionary<CurrencyAmountHolderID, List<GameObject>> _createdFlyingCurrencies =
        new Dictionary<CurrencyAmountHolderID, List<GameObject>>();

    private int _previousAmountToBeSet;

    protected virtual void OnEnable()
    {
        ResetPanel();
    }

    /// <summary>
    /// Animates the selected holder's text to final amount.
    /// </summary>
    /// <param name="holderID"></param>
    /// <param name="increase"></param>
    /// <param name="animType"></param>
    /// <param name="onAnimationCompleted"></param>
    /// <param name="finalAmount"></param>
    public void AnimateCurrencyAmountText(CurrencyAmountHolderID holderID, CurrencyAnimationType animType, int finalAmount, Action<CurrencyAmountHolderID, bool> onAnimationCompleted = null)
    {
        var holder = currencyAmountHolders.Find(cah => cah.HolderID == holderID);
        if (holder == null)
            return;

        if (animType == CurrencyAnimationType.Normal)
            holder.AnimateNormal(finalAmount, panelSettings, onAnimationCompleted);
        else
            holder.AnimateCounter(finalAmount, panelSettings, onAnimationCompleted, _previousAmountToBeSet);

        _previousAmountToBeSet = finalAmount;
    }

    /// <summary>
    /// Sets the selected holder's text immediately with given currency amount.
    /// </summary>
    /// <param name="holderID"></param>
    /// <param name="currentCurrencyAmount"></param>
    public void SetCurrencyAmountText(CurrencyAmountHolderID holderID, int currentCurrencyAmount)
    {
        var holder = currencyAmountHolders.Find(cah => cah.HolderID == holderID);
        if (holder != null) holder.UpdateCurrencyAmountText(currentCurrencyAmount);
    }
    
    public void CreateFlyingCurrencies(CurrencyAmountHolderID holderID, CurrencyAnimationType animType, int finalAmount, RectTransform initialTransformForFlyingCurrencies, Action<CurrencyAmountHolderID, bool> onAnimationCompleted = null)
    {
        var holder = currencyAmountHolders.Find(cah => cah.HolderID == holderID);
        if (holder == null || holder.IsAnimating)
            return;

        // holder.ClearCreatedFlyingCurrencies();
        ClearCreatedFlyingCurrencies(holderID);
        holder.SetIsAnimating(true);

        for (int i = 0; i < panelSettings.flyingCurrencyAmountToBeCreated; i++)
        {
            Vector2 randPosAround = (Vector2)initialTransformForFlyingCurrencies.localPosition + Random.insideUnitCircle * panelSettings.randomPositionRadius;
            GameObject currencyImageGO = holder.GetFlyingCurrencyGameObject();
            currencyImageGO.transform.SetParent(initialTransformForFlyingCurrencies.transform, false);
            currencyImageGO.transform.localScale = Vector3.one;
            currencyImageGO.transform.localPosition = Vector3.zero;

            currencyImageGO.transform.DOLocalMove(randPosAround, panelSettings.randomPositionMoveDuration)
                .SetDelay(Random.Range(panelSettings.randomPositionMoveDelayRange.x, panelSettings.randomPositionMoveDelayRange.y))
                .SetEase(panelSettings.movingEase)
                .OnComplete(() =>
                {
                    currencyImageGO.transform.SetParent(holder.transform, true);
                    currencyImageGO.transform.localScale = Vector3.one;
                    DOTween.Sequence()
                        .Join(currencyImageGO.transform.DOMove(holder.CurrencyImage.rectTransform.position, panelSettings.currencyImagePositionMoveDuration).SetSpeedBased())
                        .Join(currencyImageGO.transform.DOScale(panelSettings.flyingCurrencyImageReachScale, panelSettings.currencyImagePositionMoveDuration))
                        .SetDelay(Random.Range(panelSettings.currencyImagePositionMoveDelayRange.x, panelSettings.currencyImagePositionMoveDelayRange.y))
                        .SetEase(panelSettings.movingEase)
                        .OnComplete(() =>
                        {
                            OnFlyingCurrencyReachThePosition(holder, currencyImageGO, animType, finalAmount, onAnimationCompleted);
                        });
                });
            // holder.AddCreatedFlyingCurrency(currencyImageGO);
            AddCreatedFlyingCurrency(holderID, currencyImageGO);
        }
    }

    private void AddCreatedFlyingCurrency(CurrencyAmountHolderID holderID, GameObject flyingCurrecyImageGO)
    {
        if (_createdFlyingCurrencies.ContainsKey(holderID))
            _createdFlyingCurrencies[holderID].Add(flyingCurrecyImageGO);
        else
            _createdFlyingCurrencies.Add(holderID, new List<GameObject>(){flyingCurrecyImageGO});
    }
    
    public CurrencyAmountHolder GetCurrencyAmountHolder(CurrencyAmountHolderID holderID)
    {
        return currencyAmountHolders.Find(cah => cah.HolderID == holderID);
    }

    public void SetCurrencyAmountHolderActive(CurrencyAmountHolderID holderID, bool active)
    {
        currencyAmountHolders.Find(cah => cah.HolderID == holderID).gameObject.SetActive(active);
    }
    
    private void OnFlyingCurrencyReachThePosition(CurrencyAmountHolder holder, GameObject flyingCurrency, CurrencyAnimationType currencyAnimationType, int finalAmount, Action<CurrencyAmountHolderID, bool> onAnimationCompleted)
    {
        // int indexOfFlyingCurrency = holder.GetFlyingCurrencyIndex(flyingCurrency);
        int indexOfFlyingCurrency = _createdFlyingCurrencies[holder.HolderID].IndexOf(flyingCurrency);
        
        holder.AnimateCurrencyImage(panelSettings);

        if (indexOfFlyingCurrency == 0 && currencyAnimationType == CurrencyAnimationType.Counter)
        {
            holder.AnimateCounter(finalAmount, panelSettings, null, _previousAmountToBeSet);
        }

        if (indexOfFlyingCurrency >= _createdFlyingCurrencies[holder.HolderID].Count - 1)
        {
            if(currencyAnimationType == CurrencyAnimationType.Normal)
                holder.AnimateNormal(finalAmount, panelSettings, null);
            
            Debug.Log("Fly Completed!");
            onAnimationCompleted?.Invoke(holder.HolderID, true);
            holder.SetIsAnimating(false);
        }

        holder.DestroyFlyingCurrencyGameObject(flyingCurrency);
    }

    private void ClearCreatedFlyingCurrencies(CurrencyAmountHolderID currencyAmountHolderID)
    {
        if (_createdFlyingCurrencies.Count > 0 && _createdFlyingCurrencies[currencyAmountHolderID] != null &&
            _createdFlyingCurrencies[currencyAmountHolderID].Count > 0)
        {
            var currencyAmountHolder = GetCurrencyAmountHolder(currencyAmountHolderID);
            if (currencyAmountHolder != null)
            {
                foreach (var flyingCurrency in _createdFlyingCurrencies[currencyAmountHolderID])
                {
                    currencyAmountHolder.DestroyFlyingCurrencyGameObject(flyingCurrency);
                }
                _createdFlyingCurrencies[currencyAmountHolderID].Clear();
            }
        }
    }

    protected virtual void ResetPanel()
    {
        foreach (CurrencyAmountHolder currencyAmountHolder in currencyAmountHolders)
            currencyAmountHolder.ResetCurrencyAmountHolder();
    }

    #region For Testing Purposes
    [ContextMenu("Animate Test")]
    private void AnimateCurrencyAmountText_Test()
    {
        if (finalAmountTest <= 0)
            AnimateCurrencyAmountText(currencyAmountHolderIDTest, animationTypeTest, finalAmountTest, AnimationCompleted_Test);
        else
            AnimateCurrencyAmountText(currencyAmountHolderIDTest, animationTypeTest, finalAmountTest, AnimationCompleted_Test);
    }

    private void AnimationCompleted_Test(CurrencyAmountHolderID holderID, bool isFlyingCurrencyAnim)
    {
        Debug.Log("Currency Holder Animation Completed => " + holderID.ToString() + isFlyingCurrencyAnim);
    }

    [ContextMenu("Create Flying Currencies Test")]
    private void CreateFlyingCurrencies()
    {
        if (initialRectForFlyingCurrenciesTest == null) return;
        CreateFlyingCurrencies(currencyAmountHolderIDTest, animationTypeTest, finalAmountTest, initialRectForFlyingCurrenciesTest,
            AnimationCompleted_Test);
    }
    #endregion
}

public enum CurrencyAnimationType { Normal = 1, Counter = 2 }
public enum CurrencyAmountHolderID { Coin = 1, Money = 2 }
