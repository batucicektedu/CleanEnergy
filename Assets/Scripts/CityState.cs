using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityState : MonoBehaviour
{
    public List<LightableObjectsManager> lightableObjectManagers = new List<LightableObjectsManager>();

    public int stateID;
    
    private int _nextLightableObjectIndex;

    [HideInInspector] public MoneyPile moneyPile;

    [HideInInspector] public City city;

    private int _currentLitObjectCount;

    private List<Trafo> _trafos = new List<Trafo>();

    public Transform cityViewCameraTarget;

    private void Awake()
    {
        cityViewCameraTarget = GetComponentInChildren<CityViewCameraTarget>().transform;
        _trafos = GetComponentsInChildren<Trafo>(true).ToList();
        lightableObjectManagers = GetComponentsInChildren<LightableObjectsManager>().ToList();
        city = GetComponentInParent<City>();
    }
    
    private void Start()
    {
        LoadLitObjects();
    }
    
    public void LightObjects(int amount, bool loading = false)
    {
        for (int i = 0; i < amount; i++)
        {
            LightNextObject(loading);
        }
    }

    public void LightObjectsTillAmountReached(int amount)
    {
        var litObjectCountAtThisTime = _currentLitObjectCount;
        for (int i = 0; i < amount - litObjectCountAtThisTime; i++)
        {
            LightNextObject();
        }
    }

    private void LightNextObject(bool loading = false)
    {
        lightableObjectManagers[_nextLightableObjectIndex].LightUpLightableObjects();

        _currentLitObjectCount++;

        if (lightableObjectManagers[_nextLightableObjectIndex].fullyLit)
        {
            _nextLightableObjectIndex++;
            
            if (_nextLightableObjectIndex == lightableObjectManagers.Count)
            {
                city.IncreaseFullyLitStateCount(loading);
            }
        }
        
        if (!loading)
        {
            IncreaseSavedLitObjectCount();
        }
    }
    
    private void IncreaseSavedLitObjectCount()
    {
        //Make specific to state and city ID
        SaveManager.Instance.IncreaseLitObjectCount(1, city.cityID, stateID);
    }

    private void LoadLitObjects()
    {
        for (int i = 0; i < SaveManager.Instance.GetLitObjectCount(city.cityID, stateID); i++)
        {
            //lightableObjects[i].LightUp();
            LightNextObject(true);
        }
    }
    
    public void SpawnMoney(Vector3 spawnPos)
    {
        moneyPile.SpawnNewMoney(spawnPos);
    }

    public void EnableTrafo(int index, int finishedConnectionCount)
    {
        _trafos[index].gameObject.SetActive(true);

        ElectrifyTrafo(finishedConnectionCount);
    }

    public void ElectrifyTrafo(int finishedConnectionCount)
    {
        for (int i = 0; i < finishedConnectionCount; i++)
        {
            _trafos[i].Electrify();
        }
    }

    private List<T> GetDeepChildren<T>()
    {
        var listOfT = new List<T>();

        foreach (Transform childTransform in transform)
        {
            if (!childTransform.gameObject.activeSelf) continue;
            if (childTransform.childCount > 0)
            {
                foreach (Transform childsChildTransform in childTransform)
                {
                    if (!childsChildTransform.gameObject.activeSelf) continue;

                    listOfT.Add(childsChildTransform.GetComponent<T>());
                }
            }
            else
            {
                listOfT.Add(childTransform.GetComponent<T>());
            }
        }

        return listOfT;
    }
}
