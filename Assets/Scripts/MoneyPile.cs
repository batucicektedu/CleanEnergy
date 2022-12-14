using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyPile : MonoBehaviour
{
    public Transform moneyStackParent;
    
    [Header("MoneyStackSettings")]
    public int moneyStackColumnCount = 6;
    public int moneyStackRowCount = 10;
    public float moneyStackDistanceBetweenColumns = 1;
    public float moneyStackDistanceBetweenRows = 0.5f;
    public float moneyStackDistanceHeightDifference = 0.2f;
    private int _oneFloorCount;

    public List<CollectableMoney> _collectableMoneys = new List<CollectableMoney>();

    private void Start()
    {
        _oneFloorCount = moneyStackColumnCount * moneyStackRowCount;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TransferMoneyToPlayer(other);
        }
    }

    private void TransferMoneyToPlayer(Collider other)
    {
        if (_collectableMoneys.Count == 0) return;

        if (!SaveManager.Instance.state.isSecondTutorialComplete)
        {
            TutorialArrowsLine.Instance.ChangeTargetFromSecondTargets();
        }
        
        _collectableMoneys.Last().MoveToCollector(other.transform);
        _collectableMoneys.RemoveAt(_collectableMoneys.Count - 1);
    }

    public void SpawnNewMoney(Vector3 pos)
    {
        GameObject spawnedMoney = PoolManager.Instance.Spawn(Pools.Types.MoneyCollectable,
            pos, Quaternion.identity, moneyStackParent);

        CollectableMoney spawnedCollectableMoney = spawnedMoney.GetComponent<CollectableMoney>();
        
        spawnedCollectableMoney.ToMoneyPileJump(GetLocalPosOfNextThrash());
        
        _collectableMoneys.Add(spawnedCollectableMoney);
    }
    
    private Vector3 GetLocalPosOfNextThrash()
    {
        int currentRow = _collectableMoneys.Count % _oneFloorCount % moneyStackRowCount;
        int currentColumn = _collectableMoneys.Count % _oneFloorCount / moneyStackRowCount;
        int currentHeight = _collectableMoneys.Count / _oneFloorCount;
        
        var currentRowOffset = Vector3.forward * (moneyStackDistanceBetweenRows * currentRow);
        var currentColumnOffset = Vector3.right * (moneyStackDistanceBetweenColumns * currentColumn);
        var currentHeightOffset = Vector3.up * (moneyStackDistanceHeightDifference * currentHeight);

        return currentRowOffset + currentColumnOffset + currentHeightOffset;
    }
}
