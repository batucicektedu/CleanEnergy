using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ThrashRecycleMachine : MonoBehaviour
{
    public float _processTime = 1;
    
    private List<GameObject> _thrashList = new List<GameObject>();
    private List<GameObject> _metalList = new List<GameObject>();

    private List<int> _emptySlotsInMiddle = new List<int>();

    public Transform thrashStack;
    public Transform metalStack;
    public Transform processPoint;

    [Header("ProcessSettings")] 
    public float thrashMoveToMiddleDuration;
    public Ease thrashMoveToMiddleEase;

    [Header("ThrashStackSettings")] 
    public int thrashStackColumnCount;
    public int thrashStackRowCount;
    public float thrashStackDistanceBetweenColumns;
    public float thrashStackDistanceBetweenRows;
    public float thrashStackDistanceHeightDifference = 0.2f;
    private int _thrashOneFloorCount;
    private int _metalOneFloorCount;
    
    [Header("MetalStackSettings")] 
    public int metalStackColumnCount;
    public int metalStackRowCount;
    public float metalStackDistanceBetweenColumns;
    public float metalStackDistanceBetweenRows;
    public float metalStackDistanceHeightDifference = 0.2f;

    private int _thrashesInStack;
    private int _metalsInStack;

    private bool _isProcessing;

    private GameObject _thrashInProcess;

    private RubbishPress _rubbishPress;

    private bool _tutorialThrashTaken;
    private bool _tutorialCableGiven;

    private void Awake()
    {
        _rubbishPress = GetComponentInChildren<RubbishPress>();
    }

    private void Start()
    {
        _thrashOneFloorCount = thrashStackColumnCount * thrashStackRowCount;
        _metalOneFloorCount = metalStackColumnCount * metalStackRowCount;

        StartCoroutine(ReOrderStackEveryOnceInAWhile());
    }

    private void Update()
    {
        ProcessThrash();
    }

    public void ReceiveThrash(Collectable collectable)
    {
        _thrashList.Add(collectable.gameObject);

        collectable.transform.parent = thrashStack;

        if (!_tutorialThrashTaken)
        {
            _tutorialThrashTaken = true;
            TutorialManager.Instance.ChangeToNextTutorialStep();
        }
        
        collectable.ToMachineJump(GetLocalPosOfNextThrash());
    }

    private void ProcessThrash()
    {
        if (_thrashesInStack <= 0) return;
        if (_isProcessing) return;

        _isProcessing = true;
        
        _thrashInProcess = _thrashList[_thrashesInStack - 1].gameObject;

        _thrashInProcess.transform.DOMove(processPoint.position
                , thrashMoveToMiddleDuration)
            .SetEase(thrashMoveToMiddleEase);
        
        _thrashList.Remove(_thrashInProcess);

        DOVirtual.DelayedCall(0.3f, () =>
        {
            _rubbishPress.StartPressAnimation();
        }, false);

        DOVirtual.DelayedCall(_processTime * 0.8f, () =>
        {
            PoolManager.Instance.Despawn(Pools.Types.ThrashBag, _thrashInProcess);
            SpawnMetal();
        }, false);
        
        DOVirtual.DelayedCall(_processTime, () =>
        {
            _isProcessing = false;
        }, false);
        
        DecreaseThrashInStackCount();
    }

    private void SpawnMetal()
    {
        Transform spawnedMetal = PoolManager.Instance.Spawn(Pools.Types.CablePiece,
            processPoint.position,
            processPoint.rotation, metalStack).transform;
        Transform spawnedMetal1 = PoolManager.Instance.Spawn(Pools.Types.CablePiece,
            processPoint.position,
            processPoint.rotation, metalStack).transform;

        DOVirtual.DelayedCall(_processTime * 0.2f, () =>
        {
            _metalList.Add(spawnedMetal.gameObject);
            
            spawnedMetal.DOLocalJump(GetLocalPosOfNextMetal(),
                    GameDataManager.Instance.CollectableJumpSettings.toMetalStackJumpPower, 1
                    , GameDataManager.Instance.CollectableJumpSettings.toMetalStackJumpDuration)
                .SetEase(GameDataManager.Instance.CollectableJumpSettings.toMetalStackJumpEase)
                .OnComplete(IncreaseMetalInStackCount);
            
            _metalList.Add(spawnedMetal1.gameObject);
            
            spawnedMetal1.DOLocalJump(GetLocalPosOfNextMetal(),
                    GameDataManager.Instance.CollectableJumpSettings.toMetalStackJumpPower, 1
                    , GameDataManager.Instance.CollectableJumpSettings.toMetalStackJumpDuration)
                .SetEase(GameDataManager.Instance.CollectableJumpSettings.toMetalStackJumpEase)
                .OnComplete(IncreaseMetalInStackCount);
        },false);
    }

    public bool IsMetalStackEmpty()
    {
        return _metalsInStack == 0;
    }

    public Collectable GiveMetalToPlayer(Transform playerTransform, Vector3 posForMetalToGo)
    {
        if (_metalsInStack <= 0) return null;
        
        if (!_tutorialCableGiven)
        {
            _tutorialCableGiven = true;
            TutorialManager.Instance.ChangeToNextTutorialStep();
        }

        GameObject metalToGiveAway = _metalList[_metalList.Count - 1];

        var metalToGiveAwayCollectable = metalToGiveAway.GetComponent<Collectable>();
        
        metalToGiveAwayCollectable.EnableCollectable();
        metalToGiveAwayCollectable.GetCollected(playerTransform, posForMetalToGo);

        _metalList.Remove(metalToGiveAway);
        
        DecreaseMetalInStackCount();

        return metalToGiveAwayCollectable;
    }
    
    private void ReOrderStackPositions()
    {
        for (int i = 0; i < _thrashList.Count; i++)
        {
            _thrashList[i].transform.DOLocalMove(GetLocalPosOfNextThrash(i + 1),
                0.25f);
        }
    }

    private IEnumerator ReOrderStackEveryOnceInAWhile()
    {
        while (true)
        {
            if (_thrashList.Count > 0)
            {
                ReOrderStackPositions();
            }

            yield return new WaitForSeconds(3);
        }
    }

    private Vector3 GetLocalPosOfNextThrash(int listCount = 0)
    {
        if (listCount == 0) listCount = _thrashList.Count;
        
        int currentRow = (listCount-1) % _thrashOneFloorCount % thrashStackRowCount;
        int currentColumn = (listCount-1) % _thrashOneFloorCount / thrashStackRowCount;
        int currentHeight = (listCount-1) / _thrashOneFloorCount;
        
        var currentRowOffset = Vector3.forward * (thrashStackDistanceBetweenRows * currentRow);
        var currentColumnOffset = Vector3.right * (thrashStackDistanceBetweenColumns * currentColumn);
        var currentHeightOffset = Vector3.up * (thrashStackDistanceHeightDifference * currentHeight);

        return currentRowOffset + currentColumnOffset + currentHeightOffset;
    }

    private Vector3 GetLocalPosOfNextMetal(int posOffset = 0)
    {
        int currentRow = (_metalList.Count + posOffset -1) % _metalOneFloorCount % metalStackRowCount;
        int currentColumn = (_metalList.Count + posOffset -1) % _metalOneFloorCount / metalStackRowCount;
        int currentHeight = (_metalList.Count + posOffset -1) / _metalOneFloorCount;
        
        var currentRowOffset = Vector3.forward * (metalStackDistanceBetweenRows * currentRow);
        var currentColumnOffset = Vector3.right * (metalStackDistanceBetweenColumns * currentColumn);
        var currentHeightOffset = Vector3.up * (metalStackDistanceHeightDifference * currentHeight);

        return currentRowOffset + currentColumnOffset + currentHeightOffset;
    }

    public void IncreaseThrashInStackCount()
    {
        _thrashesInStack++;
    }

    private void DecreaseThrashInStackCount()
    {
        _thrashesInStack--;
    }

    private void IncreaseMetalInStackCount()
    {
        _metalsInStack++;
    }

    private void DecreaseMetalInStackCount()
    {
        _metalsInStack--;
    }
}
