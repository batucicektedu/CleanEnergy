using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayDotweenAnimationOnEnable : MonoBehaviour
{
    private DOTweenAnimation[] _doTweenAnimation;

    private void Awake()
    {
        _doTweenAnimation = GetComponentsInChildren<DOTweenAnimation>();
    }

    private void OnEnable()
    {
        foreach (var dA in _doTweenAnimation)
        {
            dA.DORestart();
        }
    }
}
