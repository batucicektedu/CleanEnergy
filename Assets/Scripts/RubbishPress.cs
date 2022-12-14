using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RubbishPress : MonoBehaviour
{
    public Transform pressTransform;

    [Header("AnimationSettings")] 
    public float moveDownRelativeY;
    public float moveDownDuration;
    public Ease moveDownEase;
    public float moveUpDuration;
    public Ease moveUpEase;

    private Sequence _pressSequence;

    public void StartPressAnimation()
    {
        _pressSequence?.Kill(true);
        
        _pressSequence = DOTween.Sequence();
        _pressSequence.Append(pressTransform
                .DOMoveY(-moveDownRelativeY, moveDownDuration)
                .SetRelative().SetEase(moveDownEase))
            .Append(pressTransform
                .DOMoveY(moveDownRelativeY, moveUpDuration)
                .SetRelative().SetEase(moveUpEase));
    }
}
