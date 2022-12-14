using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndGameFailPanel : Panel
{
    // public GameObject background;
    //
    // public GameObject followerImage;
    //public GameObject beforeMultiplicationAmount;
    //public GameObject beforeMultiplicationAmountdollarSign;

    //public float delayAfterBeforeMultiplicationAmount = 0.5f;
    public float delayAfterMultiplier = 0.5f;
    //public float delayAfterTotalCount = 0.5f;

    //public GameObject multiplierCount;
    //public GameObject totalCount;
    //public GameObject dollarSign;

    public GameObject restartButton;
    //
    // public AnimationCurve totalFollowerGainedAnimEase;
    //
    //public float beforeMultiplicationAmountAnimationDuration;
    //public AnimationCurve beforeMultiplicationAmountTextAnimationEase;
    //public float animationDuration;
    //public AnimationCurve textAnimationEase;

    public void RestartButtonPressed()
    {
        LevelManager.Instance.OpenCurrentLevel();
    }

    private void OnEnable()
    {
        // Destroy(UIManager.Instance.gamePanel.gameObject);
        //
        // followerImage.SetActive(true);
        // followerCount.SetActive(true);
        //followerCount.GetComponent<TMP_Text>().text = FollowersManager.Instance._followersGathered.Count.ToString();
    }

    public void StartFailPanelAnimations()
    {
        StartCoroutine(PanelOpenWithDelays());
    }

    private IEnumerator PanelOpenWithDelays()
    {
        //beforeMultiplicationAmount.SetActive(true);
        //beforeMultiplicationAmountdollarSign.SetActive(true);

        //beforeMultiplicationAmount.GetComponent<TextMeshProUGUI>().DOCounter(0, Player.Instance.totalLevelValue, beforeMultiplicationAmountAnimationDuration).SetEase(textAnimationEase);

        //yield return new WaitForSeconds(delayAfterBeforeMultiplicationAmount);

        //multiplierCount.SetActive(true);
        //multiplierCount.GetComponent<TMP_Text>().text = LevelEndManager.Instance.currentMultiplierGained + "X";

        yield return new WaitForSeconds(delayAfterMultiplier);

        restartButton.SetActive(true);

        //totalCount.SetActive(true);
        //dollarSign.SetActive(true);

        // totalCount.GetComponent<TextMeshProUGUI>().DOCounter(0, Player.Instance.totalLevelValue * LevelEndManager.Instance.currentMultiplierGained, animationDuration).SetEase(textAnimationEase).OnComplete(()=> 
        // {
        //     DOVirtual.DelayedCall(delayAfterTotalCount ,() =>
        //     {
        //         nextButton.SetActive(true);
        //     });
        //
        // });

        //totalCount.GetComponent<TMP_Text>().text = (Player.Instance.multiplier * FollowersManager.Instance._followersGathered.Count
        //    * FollowersManager.Instance.followerCountMultiplierAtEndGame).ToString();

        //DoTween's text mesh pro module is needed for DOCounter
        // totalCount.GetComponent<TMP_Text>().DOCounter(0, (int)
        //     (Player.Instance.multiplier * FollowersManager.Instance._followersGathered.Count
        //                                 * FollowersManager.Instance.followerCountMultiplierAtEndGame), 
        //     delayAfterTotalCount - 0.5f).SetEase(totalFollowerGainedAnimEase);

    }
}
