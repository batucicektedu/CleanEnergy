using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using udoEventSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableMoney : MonoBehaviour
{
    public int moneyGain = 250;

    public float speed = 50;
    
    private Tween _activeMovementTween;

    private Transform _target;

    private void Update()
    {
        MoveToTarget();
    }

    public void MoveToCollector(Transform collector)
    {
        StartMovementToCollector(collector);
        
        //transform.SetParent(collector);

        //GettingCollectedJump(newLocalPosOfCollectable);
    }

    private void StartMovementToCollector(Transform collector)
    {
        _target = collector;
    }

    private void MoveToTarget()
    {
        if (_target == null) return;

        Vector3 targetPos = _target.transform.position + Vector3.up * 3;
        
        transform.position = Vector3.Lerp(transform.position, 
            targetPos, speed * Time.smoothDeltaTime / Vector3.Distance(transform.position, targetPos));

        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            _target = null;
            
            MoneyReceiveController.Instance.IncreaseAccumulatedMoneys(moneyGain);
            
            PoolManager.Instance.Despawn(Pools.Types.MoneyCollectable, gameObject);
        }
    }

    public void ToMoneyPileJump(Vector3 newLocalPos)
    {
        _activeMovementTween?.Kill();
        _activeMovementTween = transform.DOLocalJump(newLocalPos,
                GameDataManager.Instance.CollectableJumpSettings.toMoneyPileJumpPower, 1,
                GameDataManager.Instance.CollectableJumpSettings.toMoneyPileJumpDuration)
            .SetEase(GameDataManager.Instance.CollectableJumpSettings.toMoneyPileJumpEase)
        .Join(transform.DOPunchScale(Vector3.one * 
                                     GameDataManager.Instance.CollectableJumpSettings.toMoneyPunchScaleAmount, 
                GameDataManager.Instance.CollectableJumpSettings.toMoneyPilePunchScaleDuration
                , 1, 0).SetEase(Ease.OutExpo))
            .Join(transform.DOLocalRotate(Vector3.up * GameDataManager.Instance.CollectableJumpSettings.toMoneyPileRotationAmount, 
                GameDataManager.Instance.CollectableJumpSettings.toMoneyPileJumpDuration
            , RotateMode.FastBeyond360));
    }

    private void GettingCollectedJump(Vector3 newLocalPos)
    {
        _activeMovementTween?.Kill();
        _activeMovementTween = transform.DOLocalJump(newLocalPos, 
                GameDataManager.Instance.CollectableJumpSettings.collectionJumpPower, 1, 
                GameDataManager.Instance.CollectableJumpSettings.collectionJumpDuration)
            .SetEase(GameDataManager.Instance.CollectableJumpSettings.collectionJumpEase);
        
        transform.DOLocalRotate(Vector3.zero, 
            GameDataManager.Instance.CollectableJumpSettings.collectionJumpDuration);
    }
}
