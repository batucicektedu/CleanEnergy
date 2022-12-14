using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "PanelWithCurrencyAmountsSettings", menuName = "UIData/PanelWithCurrencyAmountsSettings")]
public class PanelWithCurrencyAmountsSettings : ScriptableObject
{
    [Header("General Animation Properties")]
    public float textScaleIncreaseAmount = 1.1f;
    public float textAnimDurationScaleUp = 0.3f;
    public float textAnimDurationScaleDown = 0.15f;
    public Color textIncreaseColor = Color.green;
    public Color textDecreaseColor = Color.red;
    public float countDuration = 1f;
    public float imageScaleIncreaseAmount = 1.1f;
    public float imageAnimationDuration = 0.3f;
    public float waitingDurationBeforeAmountChangeOnNormalAnim = 0.25f;

    [Header("Flying Currency Properties")]
    public int flyingCurrencyAmountToBeCreated = 15;
    public float randomPositionRadius = 250f;
    public float randomPositionMoveDuration = 0.7f;
    public Vector2 randomPositionMoveDelayRange = new Vector2(0.01f, 0.15f);
    public float currencyImagePositionMoveDuration = 0.6f;
    public Vector2 currencyImagePositionMoveDelayRange = new Vector2(0.01f, 0.15f);
    public float flyingCurrencyImageReachScale = 1f;
    public Ease movingEase = Ease.InOutSine;
}