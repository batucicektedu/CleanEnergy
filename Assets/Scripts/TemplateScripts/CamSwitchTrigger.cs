using System;
using System.Collections;
using System.Collections.Generic;
using udoEventSystem;
using UnityEngine;

public class CamSwitchTrigger : MonoBehaviour
{
    public MyCamSwitcher.CamStates camStates;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MyCamSwitcher.Instance.SwitchToCam(camStates);
        }
    }
}
