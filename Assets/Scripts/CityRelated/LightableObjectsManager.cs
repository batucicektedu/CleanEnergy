using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightableObjectsManager : MonoBehaviour
{
    public List<LightableObject> lightableObjects = new List<LightableObject>();

    private MoneyTrickler _moneyTrickler;
    
    private ParticleSystem[] _fogParticles;
    
    private Sparkles _sparkles;

    private Light _pointLight;

    public LightableObjectsManager twinBuilding;

    public bool halfLit;

    public bool fullyLit;

    private void Awake()
    {
        _sparkles = GetComponentInChildren<Sparkles>(true);
        _pointLight = GetComponentInChildren<Light>(true);

        _moneyTrickler = GetComponent<MoneyTrickler>();
        lightableObjects = GetComponentsInChildren<LightableObject>().ToList();
        
        _fogParticles = GetComponentsInChildren<ParticleSystem>();
        
        if (twinBuilding != null)
        {
            twinBuilding.GetComponent<MoneyTrickler>()._cityState = GetComponentInParent<CityState>();
        }
    }

    public void LightableObjectLitUp()
    {
        if (AreAllObjectsLitUp())
        {
            _moneyTrickler.StartMoneyTrickle();
            fullyLit = true;
            halfLit = false;
        }
        else
        {
            halfLit = true;
        }
    }

    private bool AreAllObjectsLitUp()
    {
        if (lightableObjects.FirstOrDefault(lo => !lo.LitUp) == null)
        {
            LightUpEffects();
            
            return true;
        }

        return false;
    }

    private void LightUpEffects()
    {
        foreach (var fp in _fogParticles)
        {
            fp.gameObject.SetActive(false);
        }
        
        _sparkles.gameObject.SetActive(true);
        
        _pointLight.gameObject.SetActive(true);
    }

    public void LightUpLightableObjects()
    {
        foreach (var clo in lightableObjects)
        {
            clo.LightUp();
        }
        
        if (twinBuilding != null)
        {
            twinBuilding.LightUpLightableObjects();
        }
    }
}
