using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigToSmallDoScale : MonoBehaviour
{
    public float startScaleMultiplier = 2;
    public float scaleDownDuration = 0.75f;
    public Ease ease = Ease.InQuart;
    
    private void Start()
    {
        transform.DOScale(transform.localScale * startScaleMultiplier, scaleDownDuration)
            .SetEase(ease).From();
    }
}
