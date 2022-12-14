using System;
using System.Collections;
using System.Collections.Generic;
using udoEventSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public Pools.Types objectType;

    public int cityID;

    private GameObject _spawnedObject;

    private bool _objectActive;

    public bool isTutorialThrash;
    
    public GameObject tutorialArrow;
    
    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.Get<CableConnectedFromCity>().AddListener(SpawnObjectFromEvent);
        EventManager.Get<SpawnTutorialThrash>().AddListener(SpawnTutorialThrash);
        EventManager.Get<SpawnThrashOnCity>().AddListener(SpawnObjectFromEvent);
    }

    private void OnDisable()
    {
        EventManager.Get<CableConnectedFromCity>().RemoveListener(SpawnObjectFromEvent);
        EventManager.Get<SpawnTutorialThrash>().RemoveListener(SpawnTutorialThrash);
        EventManager.Get<SpawnThrashOnCity>().RemoveListener(SpawnObjectFromEvent);
    }

    private void Start()
    {
        //Still at tutorial
        if (!SaveManager.Instance.state.isTutorialShown && cityID == 0) return;
        
        SpawnObject();
    }

    private void SpawnObject()
    {
        if (SaveManager.Instance.state.isTutorialShown && isTutorialThrash) return;
        if (_objectActive) return;
        
        _spawnedObject = PoolManager.Instance.Spawn(objectType, transform.position, 
            Quaternion.Euler(Vector3.up * Random.Range(0, 360)), transform);
        _objectActive = true;

        if (isTutorialThrash)
        {
            tutorialArrow.SetActive(true);
        
            TutorialManager.Instance.ChangeTutorialThrashCount(+1);
        }
    }

    private void SpawnObjectFromEvent(int eventCityID)
    {
        if (eventCityID != cityID) return;
        
        SpawnObject();
    }
    
    public void DetachObject()
    {
        _objectActive = false;

        if (isTutorialThrash)
        {
            tutorialArrow.SetActive(false);

            TutorialManager.Instance.ChangeTutorialThrashCount(-1);
            
            Destroy(gameObject);
        }
        else if(!SaveManager.Instance.state.isTutorialShown)
        {
            TutorialManager.Instance.ChangeToNextTutorialStep();
        }
    }

    private void SpawnTutorialThrash()
    {
        if (!isTutorialThrash) return;
        
        SpawnObject();
    }
}
