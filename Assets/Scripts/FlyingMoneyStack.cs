using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FlyingMoneyStack : MonoBehaviour
{
    [Header("TweenSettings")] 
    public float tweenSpeed;
    public Ease tweenEase;
    public float jumpPower = 2;

    private Tween _movementTween;
    
    public delegate void TweenCompletionMethodCallback(int moneyAmount);
    
    public void Movement(Vector3 destination, TweenCompletionMethodCallback tweenCompletionMethodCallback
        , int moneyAmount)
    {
        _movementTween?.Kill();
        _movementTween = transform.DOJump(destination, jumpPower, 1, tweenSpeed)
            .SetEase(tweenEase)
            .OnComplete(()=>
            {
                tweenCompletionMethodCallback(moneyAmount);
                DeSpawnToPool();
            });
    }

    private void DeSpawnToPool()
    {
        _movementTween?.Kill();
        PoolManager.Instance.Despawn(Pools.Types.MoneyStack, gameObject);
    }
}
