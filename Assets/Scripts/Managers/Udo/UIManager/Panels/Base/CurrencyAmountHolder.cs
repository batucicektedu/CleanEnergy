using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CurrencyAmountHolder : MonoBehaviour
{
    [SerializeField] private CurrencyAmountHolderID holderID;
    [SerializeField] private TMP_Text currencyAmountText;
    [SerializeField] private Image currencyImage;
    [Tooltip("Use This If You Don't Use Object Pooling"), SerializeField] private GameObject flyingCurrencyImagePrefab;

    private Sequence _currencyAmountTextIncreaseDecreaseSequence;
    private Color _currencyAmountTextInitialColor;
    private Vector3 _currencyAmountTextInitialPosition;
    private bool _isAnimating;
    private bool _isInitialized;

    public CurrencyAmountHolderID HolderID => holderID;
    public bool IsAnimating => _isAnimating;
    public Image CurrencyImage => currencyImage;

    private void Awake()
    {
        _currencyAmountTextInitialColor = currencyAmountText.color;
        _currencyAmountTextInitialPosition = currencyAmountText.rectTransform.anchoredPosition;
        _isInitialized = true;
    }

    public void AnimateNormal(int finalAmount, PanelWithCurrencyAmountsSettings settings, Action<CurrencyAmountHolderID, bool> onAnimationCompleted)
    {
        bool isIncreasing = true;
        int currentAmount = int.Parse(currencyAmountText.text);
        if (finalAmount == currentAmount)
            return;
        else if (currentAmount > finalAmount)
            isIncreasing = false;
        
        _isAnimating = true;
        _currencyAmountTextIncreaseDecreaseSequence?.Kill();
        _currencyAmountTextIncreaseDecreaseSequence = DOTween.Sequence()
            .Join(currencyAmountText.transform.DOScale(settings.textScaleIncreaseAmount, settings.textAnimDurationScaleUp))
            .Join(currencyAmountText.DOColor(isIncreasing ? settings.textIncreaseColor : settings.textDecreaseColor, settings.textAnimDurationScaleUp))
            .AppendCallback(() => currencyAmountText.text = finalAmount.ToString())
            .AppendInterval(settings.waitingDurationBeforeAmountChangeOnNormalAnim)
            .Append(currencyAmountText.transform.DOScale(1f, settings.textAnimDurationScaleDown))
            .Join(currencyAmountText.DOColor(_currencyAmountTextInitialColor, settings.textAnimDurationScaleDown))
            .OnComplete(() => {
                onAnimationCompleted?.Invoke(this.holderID, false);
                _isAnimating = false;
            });
    }

    public void AnimateCounter(int finalAmount, PanelWithCurrencyAmountsSettings settings, Action<CurrencyAmountHolderID, bool> onAnimationCompleted, int previousValue)
    {
        if (_isAnimating)
        {
            currencyAmountText.text = previousValue.ToString();
        }
        
        bool isIncreasing = true;
        int currentAmount = int.Parse(currencyAmountText.text);
        if (finalAmount == currentAmount)
            return;
        else if (currentAmount > finalAmount)
            isIncreasing = false;

        _isAnimating = true;
        _currencyAmountTextIncreaseDecreaseSequence?.Kill();
        _currencyAmountTextIncreaseDecreaseSequence = DOTween.Sequence()
            .Join(currencyAmountText.transform.DOScale(settings.textScaleIncreaseAmount, settings.textAnimDurationScaleUp))
            .Join(currencyAmountText.DOColor(isIncreasing ? settings.textIncreaseColor : settings.textDecreaseColor, settings.textAnimDurationScaleUp))
            .AppendCallback(() =>
            {
                DOVirtual.Float((float)currentAmount, (float)finalAmount, settings.countDuration, (amt) =>
                {
                    currencyAmountText.text = ((int)amt).ToString();
                });
            })
            .AppendInterval(settings.countDuration)
            .Append(currencyAmountText.transform.DOScale(1f, settings.textAnimDurationScaleDown))
            .Join(currencyAmountText.DOColor(_currencyAmountTextInitialColor, settings.textAnimDurationScaleDown))
            .OnComplete(() => {
                onAnimationCompleted?.Invoke(this.holderID, false);
                _isAnimating = false;
            });
    }

    public void AnimateCurrencyImage(PanelWithCurrencyAmountsSettings settings)
    {
        currencyImage.transform.DOScale(settings.imageScaleIncreaseAmount, settings.imageAnimationDuration).OnComplete(() =>
        {
            currencyImage.transform.DOScale(1f, settings.imageAnimationDuration);
        });
    }

    public void UpdateCurrencyAmountText(int currentCurrencyAmount)
    {
        currencyAmountText.text = currentCurrencyAmount.ToString();
    }

    // Warning: If you want to use object pooling;
    // Create a new class inherited from this class then override this method.
    public virtual GameObject GetFlyingCurrencyGameObject()
    {
        return Instantiate(flyingCurrencyImagePrefab);
    }

    // Warning: If you want to use object pooling;
    // Create a new class inherited from this class then override this method.
    public virtual void DestroyFlyingCurrencyGameObject(GameObject flyingCurrencyGameObject)
    {
        Destroy(flyingCurrencyGameObject);
    }
    
    public void SetIsAnimating(bool value)
    {
        _isAnimating = value;
    }

    public void ResetCurrencyAmountHolder()
    {
        if (!_isInitialized)
            return;

        _currencyAmountTextIncreaseDecreaseSequence?.Kill();
        currencyAmountText.transform.localScale = Vector3.one;
        currencyAmountText.color = _currencyAmountTextInitialColor;
        _isAnimating = false;
    }
}
