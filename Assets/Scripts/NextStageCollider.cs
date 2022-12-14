using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NextStageCollider : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    
    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void ColliderDisableActions()
    {
        MyCamSwitcher.Instance.SwitchMainCamTargetThenTurnBackDisablingInput(transform);

        DOVirtual.DelayedCall(2f, () =>
        {
            _particleSystem.transform.parent = null;
            _particleSystem.Play();

            DOVirtual.DelayedCall(0.5f, () =>
            {
                TutorialArrowsLine.Instance.SetTargetToBridgeCollider(transform);
            }, false);
            
            gameObject.SetActive(false);
        }, false);
    }
}
