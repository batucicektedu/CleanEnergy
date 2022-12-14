using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Collectable : MonoBehaviour
{
    public CollectableType collectableType;

    public CollectableState collectableState = CollectableState.Active;

    private Tween _activeMovementTween;

    private ThrashRecycleMachine _thrashRecycleMachine;

    private ObjectSpawnerMessageHandler _objectSpawnerMessageHandler;

    private void Awake()
    {
        _objectSpawnerMessageHandler = GetComponent<ObjectSpawnerMessageHandler>();
    }

    private void OnDisable()
    {
        if (collectableType == CollectableType.ThrashChunk)
        {
            collectableState = CollectableState.Active;
        }
    }

    public void GetCollected(Transform transformOfCollector, Vector3 newLocalPosOfCollectable,
        bool collectingFromSpawner = false)
    {
        if (collectableState != CollectableState.Active) return;

        collectableState = CollectableState.DeActive;

        transform.SetParent(transformOfCollector);
        
        if (collectingFromSpawner && collectableType == CollectableType.ThrashChunk)
        {
            _objectSpawnerMessageHandler.ObjectRemovedFromSpawner();
        }

        GettingCollectedJump(newLocalPosOfCollectable);
    }

    public void ToCablePieceJump(Transform newParent, CablePiece cablePiece)
    {
        transform.parent = newParent;
        
        _activeMovementTween?.Kill();
        _activeMovementTween = transform.DOLocalJump(Vector3.zero, 
                GameDataManager.Instance.CollectableJumpSettings.toCablePieceJumpPower, 1, 
                GameDataManager.Instance.CollectableJumpSettings.toCablePieceJumpDuration)
            .SetEase(GameDataManager.Instance.CollectableJumpSettings.toCablePieceJumpEase)
            .OnComplete(()=>
            {
                cablePiece.ActivateCable();
                
                PoolManager.Instance.Despawn(Pools.Types.CablePiece, gameObject);
            });
        
        transform.DOLocalRotate(Vector3.zero, 
            GameDataManager.Instance.CollectableJumpSettings.toCablePieceJumpDuration);
    }

    public void EnableCollectable()
    {
        collectableState = CollectableState.Active;
    }

    private void GettingCollectedJump(Vector3 newLocalPos)
    {
        _activeMovementTween?.Kill();
        _activeMovementTween = transform.DOLocalJump(newLocalPos, 
            GameDataManager.Instance.CollectableJumpSettings.collectionJumpPower, 1, 
            GameDataManager.Instance.CollectableJumpSettings.collectionJumpDuration)
            .SetEase(GameDataManager.Instance.CollectableJumpSettings.collectionJumpEase);
        
        transform.DOLocalRotate(/*Vector3.zero*/ Vector3.up* Random.Range(0, 360)
            , GameDataManager.Instance.CollectableJumpSettings.collectionJumpDuration);
    }

    public void ReOrderInStack(Vector3 newLocalPos)
    {
        _activeMovementTween?.Kill();
        _activeMovementTween = transform.DOLocalMove(newLocalPos,
            GameDataManager.Instance.CollectableJumpSettings.reOrderStackSpeed)
            .SetSpeedBased();
    }
    
    public void ToMachineJump(Vector3 newLocalPos)
    {
        _activeMovementTween?.Kill();
        _activeMovementTween = transform.DOLocalJump(newLocalPos, 
                GameDataManager.Instance.CollectableJumpSettings.toMachineJumpPower, 1, 
                GameDataManager.Instance.CollectableJumpSettings.toMachineJumpDuration)
            .SetEase(GameDataManager.Instance.CollectableJumpSettings.toMachineJumpEase)
            .Join(transform.DOLocalRotate(Vector3.zero, 
                GameDataManager.Instance.CollectableJumpSettings.toMachineJumpDuration))
            .OnComplete(() =>
            {
                _thrashRecycleMachine = GetComponentInParent<ThrashRecycleMachine>();
                _thrashRecycleMachine.IncreaseThrashInStackCount();
            });
    }

    public enum CollectableState
    {
        None = 0,
        Active = 5,
        DeActive = 10,
    }
}