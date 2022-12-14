using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : MonoBehaviour
{
    public List<CityState> cityStates = new List<CityState>();

    public int cityID;

    private int _fullyLitStateCount;

    public GameObject nextCityCollider;

    private void Awake()
    {
        cityStates = GetComponentsInChildren<CityState>().ToList();
    }

    public void IncreaseFullyLitStateCount(bool loading)
    {
        _fullyLitStateCount++;

        if (_fullyLitStateCount == cityStates.Count)
        {
            if (cityID != 2 && !loading)
            {
                nextCityCollider.GetComponent<NextStageCollider>().ColliderDisableActions();
                return;
            }
            
            nextCityCollider.SetActive(false);
        }
    }

    public int GetFullyLitStateCount()
    {
        return _fullyLitStateCount;
    }
}
