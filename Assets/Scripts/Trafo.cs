using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trafo : MonoBehaviour
{
    private List<Electricity> _electricParticles = new List<Electricity>();

    private void Awake()
    {
        _electricParticles = GetComponentsInChildren<Electricity>(true).ToList();
    }

    public void Electrify()
    {
        foreach (var ep in _electricParticles)
        {
            ep.gameObject.SetActive(true);
        }
    }
}
