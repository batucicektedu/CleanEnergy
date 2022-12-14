using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackAndForthMovement : MonoBehaviour
{
    public float moveAmount;

    public float moveDuration;

    public Axis axis;

    public AnimationCurve ease;
    
    public enum Axis
    {
        X,
        Y,
        Z
    }

    private void Start()
    {
        if (axis == Axis.X)
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOLocalMove(transform.localPosition + moveAmount * transform.right, moveDuration))
                .Append(transform.DOLocalMove(transform.localPosition, moveDuration))
                .SetLoops(-1).SetEase(ease);
        }
        else if(axis == Axis.Y)
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOLocalMove(transform.localPosition + moveAmount * transform.up, moveDuration))
                .Append(transform.DOLocalMove(transform.localPosition, moveDuration))
                .SetLoops(-1).SetEase(ease);
        }
        else if(axis == Axis.Z)
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOLocalMove(transform.localPosition + moveAmount * transform.forward, moveDuration))
                .Append(transform.DOLocalMove(transform.localPosition, moveDuration))
                .SetLoops(-1).SetEase(ease);
        }
        
    }
}
